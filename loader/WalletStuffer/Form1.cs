using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

using block_io_sharp;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

namespace WalletLoader
{
    public partial class frmSender : Form
    {
        public frmSender()
        {
            InitializeComponent();
        }

        frmAddressChecker frmChecker;

        private static string currentNet;

        private static coinFraction currentFraction;

        private class coinFraction
        {
            public string caption;
            public string prefix;

            public int denominator;

            public coinFraction(string _caption, string _prefix, int _denominator)
            {
                caption = _caption;
                prefix = _prefix;
                denominator = _denominator;
            }

            public override string ToString()
            {
                if (currentNet != null)
                {
                    return string.Format("{0}{1} {2}", prefix, currentNet, caption);
                }
                else
                {
                    return string.Format("{0}Coins {1}", prefix, caption);
                }

            }

            public decimal toFraction(decimal wholeCoinValue)
            {
                return wholeCoinValue * denominator;
            }

            public decimal toWholeCoin(decimal fractionValue)
            {
                return fractionValue / denominator;
            }

            public string formatFraction(decimal wholeCoinValue)
            {
                return string.Format("{0:#,0.#,############} {1}{2}", toFraction(wholeCoinValue), prefix, currentNet);
            }

            public string formatFractionNumberOnly(decimal wholeCoinValue)
            {
                return string.Format("{0:#,0.#,############}", toFraction(wholeCoinValue));
            }

        }

        private List<coinFraction> fractions;

        private class LabelAddress
        {
            public string label;
            public decimal balance;
            public string net;

            public override string ToString()
            {
                return string.Format("{0} ({1})", label, currentFraction.formatFraction(balance));
            }
        }

        private List<LabelAddress> labels;

        private decimal pickTxFeeBonus(string net)
        {
            decimal feeAdd;

            switch (net)
            {
                case "DOGE":
                case "DOGETEST":
                    feeAdd = 1;
                    break;
                case "BTC":
                case "BTCTEST":
                    feeAdd = new decimal(0.0001);
                    break;
                case "LTC":
                case "LTCTEST":
                    feeAdd = new decimal(0.001);
                    break;
                default:
                    feeAdd = 0;
                    break;
            }

            return feeAdd;
        }

        private void setupFractions()
        {
            fractions = new List<coinFraction>();

            currentFraction = new coinFraction("", "", 1);

            fractions.Add(currentFraction);
            fractions.Add(new coinFraction("(1/1000)", "m", 1000));
            fractions.Add(new coinFraction("(1/Million)", "u", 1000000));

        }

        private void reloadFractions()
        {
            if (ddlCoinFraction.DataSource == null)
            {
                ddlCoinFraction.DataSource = fractions;

                //currentFraction = (coinFraction)ddlCoinFraction.SelectedItem;

            }
            else
            {
                inhibitFractionChange = true;

                int prevFractionIndex = ddlCoinFraction.SelectedIndex;

                ddlCoinFraction.DataSource = null;
                ddlCoinFraction.DataSource = fractions;

                ddlCoinFraction.SelectedIndex = prevFractionIndex;

                inhibitFractionChange = false;
            }
        }

        private void reloadLabels()
        {
            if (ddlTakeFromLabel.DataSource == null)
            {
                ddlTakeFromLabel.DataSource = labels;
            }
            else
            {
                int prevLabelIndex = ddlTakeFromLabel.SelectedIndex;

                ddlTakeFromLabel.DataSource = null;
                ddlTakeFromLabel.DataSource = labels;

                ddlTakeFromLabel.SelectedIndex = prevLabelIndex;
            }
        }

        private void updateFeeAddCheckbox()
        {
            decimal feeAdd = pickTxFeeBonus(currentNet);
            
            chkIncludeWithdrawFee.Text = string.Format("Add Withdraw Fee on top ({0})", currentFraction.formatFraction(feeAdd));
        }


        private void frmSender_Load(object sender, EventArgs e)
        {
            setupFractions();
            reloadFractions();
        }

        #region menu handlers


        // open the Address Checker form when the Menu\Address Checker menu item is clicked
        private void addressCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChecker = new frmAddressChecker();
            frmChecker.ShowDialog(this);
        }

        // Close the app from the Menu\Exit menu item
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Show the About box from the Help\About menu item
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string aboutText = string.Format("Multi-Coin Wallet Loader by Buxton The Red.\nVersion: {0}\nSee reddit.com/r/walletprint for further help and support.", Application.ProductVersion);

            MessageBox.Show(aboutText, "About Wallet Loader", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        #endregion

        // actually do the work
        private void btnSend_Click(object sender, EventArgs e)
        {
            // validate API Key
            string apikey = txtApiKey.Text.Trim();

            if (!isValidApiKey(apikey))
            {
                lblApiKeyInvalidFormat.Visible = true;

                MessageBox.Show("API Key is required and must be in expected format (like '0123-4567-890a-bcde')", "Invalid API Key format", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            // load in address-list, check we don't exceed 100
            List<string> addresses = txtAddresses.Lines.Where(L => L.Trim() != "").ToList();

            if (addresses.Count > 100)
            {
                MessageBox.Show(String.Format("Can't send to {0} addresses in one shot, Block.io's maximum is 100", addresses.Count), "Too many Addresses", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal perAddressValue;

            // check the send amount is sensible (a valid number, and greater than zero)
            if (decimal.TryParse(txtSendAmount.Text, out perAddressValue))
            {
                if (perAddressValue <= 0)
                {
                    MessageBox.Show("Send Amount must be greater than zero!", "Invalid Send Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Send Amount field is not a valid number", "Invalid Send Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // a block.io "Label" is required, to be the source of the coins
            string sourceLabel = ((LabelAddress)ddlTakeFromLabel.SelectedValue).label; //txtFromLabels.Text.Trim();

            if (sourceLabel.Length < 1)
            {
                MessageBox.Show("Take From Label must be provided", "Missing 'Take From Label'", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // the block.io Secret Pin is necessary too
            string secretPin = txtSecretPin.Text.Trim();

            if (secretPin == "")
            {
                MessageBox.Show("Secret PIN must be provided", "Missing 'Secret PIN'", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            perAddressValue = currentFraction.toWholeCoin(perAddressValue);

            // calculate the absolute minimal cost of this operation (Minimal here means 'just the value of the coins, not including any Fees')
            decimal projectedCostMinimal = perAddressValue * addresses.Count;

            // create an instance of the BlockIO API Client, using their library code
            BlockIO client = new BlockIO(txtApiKey.Text.Trim());

            decimal AvailableBalance;
            string net;

            // look up the available Balance of the specified Label, which will also tell us which Coin (BTC/LTC/DOGE) is being used

            APIResponse response;
            try
            {
                response = client.getAddressByLabel(sourceLabel);
            }
            catch (Exception ex)
            {
                // that didn't work...
                string errmsg = string.Format("Error when asking Block.Io for Label Balance.\nInternal Error message is:\n{0}", ex.Message);
                MessageBox.Show(errmsg, "Error when getting Label Balance", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // bail out of this Function entirely (give up the process)
                return;
            }

            AvailableBalance = decimal.Parse((string)response.Data["available_balance"]);
            net = (string)response.Data["network"];
            string networkName;

            if (net != null)
            {
                networkName = net;
            }
            else
            {
                networkName = "[unknown]";
            }

            // now we know the network in use, we can optionally bump the per-wallet amount and recalculate the projected cost (if user has chosen the "Include Network Fee" option)
            if (chkIncludeWithdrawFee.Checked)
            {
                decimal feeAdd = 0;

                // select fee-included amount based on the coin
                // these are currently hard-coded (erring on the side of being a little over-generous to try to ensure the resultant wallets can be redeemed reasonably quickly and net the recipient the intended amount after fee-adding)

                // a future version may see these turned in to user-facing settings. for now they are hard-coded
                // user can choose to not use this option and adjust the main send amount value instead, if they wish to get finer control

                feeAdd = pickTxFeeBonus(net);

                perAddressValue += feeAdd;
                projectedCostMinimal = perAddressValue * addresses.Count;
            }

            // compare Label Balance against projected cost
            if (AvailableBalance < projectedCostMinimal)
            {
                MessageBox.Show(string.Format("Not enough funds in selected Account Label - you have {0}, but I think you need at least {1}", currentFraction.formatFraction(AvailableBalance), currentFraction.formatFraction(projectedCostMinimal)), "Insufficient Funds", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // build the list of send-values
            // right now, we only send the same amount to each address
            List<string> sendValues = Enumerable.Repeat(perAddressValue.ToString(), addresses.Count).ToList();

            // estimate the network fee on the intended transaction
            response = client.getNetworkFeeEstimate(sendValues, addresses);

            decimal estFee = decimal.Parse((string)response.Data["estimated_network_fee"]);

            // ensure that the per-wallet amount is a sensible amount - I don't want this app to be used to create Dust Transactions
            // The amount sent to each recipient address must be at least a certain multiple of the Total Fee estimated to send the entire initial transaction.
            // The ratio below is totally artificial and I just pulled it out of thin air. If you REALLY want to send smaller values than I'm allowing, you can just change the value in the next line
            int feeToValueMinRatio = 5;
            if (perAddressValue < (estFee * feeToValueMinRatio))
            {
                MessageBox.Show("This app declines to send your transaction, because your Per-Wallet value is too small compared to the Network Fee about to be incurred. Please increase your Amount To Send, or decrease your number of target wallets. (Note: this is NOT a Block.Io limitation)", "Wallet Value too small", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // final balance check, now I know what the Estimated Send Fee is...
            decimal finalEstimate = projectedCostMinimal + estFee;

            if (AvailableBalance < finalEstimate)
            {
                MessageBox.Show(string.Format("Not enough funds in nominated Block.Io Label - you have {0}, but after calculating TX Fee you need at least {1}", currentFraction.formatFraction(AvailableBalance), currentFraction.formatFraction(finalEstimate)), "Insufficient Funds", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // show a final YES/NO confirmation dialog now we've done the pre-flight tests
            if (MessageBox.Show(String.Format("Sure you want to send {0} to each of {1} addresses, with a TX Fee of {2}, total spend {3}?", currentFraction.formatFraction(perAddressValue), addresses.Count, currentFraction.formatFraction(estFee), currentFraction.formatFraction(finalEstimate)), "Final Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                // they chose not to proceed
                return;
            }

            List<string> labelsList = new List<string>();
            labelsList.Add(sourceLabel);

            // Ask Block.io to perform the intended transaction. Note: This is NOT using client-side tx signing.
            try
            {
                response = client.withdrawFromLabels(labelsList, sendValues, secretPin, addresses);

                MessageBox.Show(string.Format("Amount Sent:{0}, Amount Withdrawn:{1}, Block.IO Fee:{2}, Network Fee:{3}, TX ID:{4}", response.Data["amount_sent"], response.Data["amount_withdrawn"], response.Data["blockio_fee"], response.Data["network_fee"], response.Data["txid"]), "Transaction Completed OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // that didn't work...
                string errmsg = string.Format("Error when asking Block.Io to Send the Transaction - please check your block.io account before retrying, just in case the transaction WAS actually sent.\nInternal Error Message is:\n{0}", ex.Message);
                MessageBox.Show(errmsg, "Error when Sending Transaction", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // bail out of this Function entirely (give up the process)
                return;
            }
            
        }


        private void txtApiKey_Validating(object sender, CancelEventArgs e)
        {
            string apikey = txtApiKey.Text.Trim();

            if (isValidApiKey(apikey))
            {
                lblApiKeyInvalidFormat.Visible = false;

                this.UseWaitCursor = true;
                lblPleaseWait.Visible = true;

                // yes, calling DoEvents is slightly poor form, and really anything which takes "long" rather than "instant" time should ideally be threaded... 
                Application.DoEvents();

                if (tryGetAddresses(apikey))
                {
                    // success
                    lblSetApiKey.Visible = false;
                    ddlCoinFraction.Enabled = true;
                }
                else
                {
                    // call didn't work
                    lblSetApiKey.Visible = true;
                    ddlCoinFraction.Enabled = false;
                }

                this.UseWaitCursor = false;
                lblPleaseWait.Visible = false;

            }
            else
            {
                lblApiKeyInvalidFormat.Visible = true;
            }
        }

        // local regex sanity-check to ensure the provided apikey is even slightly right
        private bool isValidApiKey(string apikey)
        {
            Regex r = new Regex("[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}");

            return r.IsMatch(apikey);
        }

        // ask block.io for the Addresses (really we care about the Labels) for a given apikey, and update the UI if successful (this is also how we detect the cointype)
        private bool tryGetAddresses(string apikey)
        {

            try
            {
                BlockIO client = new BlockIO(apikey);
                APIResponse r = client.getMyAddresses();

                string net = (string)r.Data["network"];
                currentNet = net;

                // reload the Fractions dropdown to follow the potential change in coin network
                reloadFractions();

                // update the text on the "add tx fee" checkbox, again to follow potential network change
                updateFeeAddCheckbox();

                lblCoinType.Text = string.Format("Using coin type: {0}", net);

                // gather list of addresses-labels-values and put in to a nice dropdown list (replacing current silly "label" textbox)
                ArrayList addresses = (ArrayList)r.Data["addresses"];

                // devnote: store the data properly structured internally (make a class, store as array-of-class), then build display strings for the DDL
                // and in a SelectedIndexChanged handler for that new DDL, update a new Label to show available balance

                labels = new List<LabelAddress>();

                foreach (Dictionary<string, object> a in addresses)
                {
                    LabelAddress la = new LabelAddress();
                    la.net = net;
                    la.label = (string)a["label"];
                    la.balance = decimal.Parse((string)a["available_balance"]);

                    labels.Add(la);
                }

                ddlTakeFromLabel.DataSource = labels;
                ddlTakeFromLabel.SelectedIndex = 0;
                //reloadLabels();


            }
            catch (Exception ex)
            {
                // that didn't work...
                string errmsg = string.Format("Error when asking Block.Io for Address/Label List.\nInternal Error message is:\n{0}", ex.Message);
                MessageBox.Show(errmsg, "Error getting Label List", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;

        }

        private bool inhibitFractionChange = false;

        private void ddlCoinFraction_SelectedValueChanged(object sender, EventArgs e)
        {

            if (inhibitFractionChange)
            {
                return;
            }
            
            // when the ddlCoinFraction dropdown selection is changed, rebase the value of txtSendAmount to follow along

            // first, check that the textbox contains an intelligible Decimal. if not, no reprocessing happens
            decimal currentBoxValue;

            if (decimal.TryParse(txtSendAmount.Text, out currentBoxValue))
            {
                // ok there's a proper number in the box
                decimal wholeCoinEquivalent = currentFraction.toWholeCoin(currentBoxValue);
                currentFraction = (coinFraction)ddlCoinFraction.SelectedItem;

                //decimal newBoxValue = currentFraction.toFraction(wholeCoinEquivalent);
                //txtSendAmount.Text = string.Format("{0:0,0.#,############}", newBoxValue);

                txtSendAmount.Text = currentFraction.formatFractionNumberOnly(wholeCoinEquivalent);

            }
            else
            {
                // not a parseable decimal, so we don't care
                currentFraction = (coinFraction)ddlCoinFraction.SelectedItem;
            }

            // finally, refresh the Labels DDL so that values there are shown in the same Fraction setting
            reloadLabels();

            // and refresh the "add tx fee" checkbox for the same reason
            updateFeeAddCheckbox();
        }

        // check that the entered amount, if a number, does not go longer than 8 decimal places once re-based back to Whole Coins
        private void txtSendAmount_Validating(object sender, CancelEventArgs e)
        {
            decimal currentBoxValue;

            if (decimal.TryParse(txtSendAmount.Text, out currentBoxValue))
            {
                // ok there's a proper number in the box
                decimal wholeCoinEquivalent = currentFraction.toWholeCoin(currentBoxValue);

                string formattedEquivalent = string.Format("{0:0.########}", wholeCoinEquivalent);

                decimal parsedBackAgain = decimal.Parse(formattedEquivalent);

                if (parsedBackAgain != wholeCoinEquivalent)
                {
                    // number must have more decimal places than we just formatted for...
                    txtSendAmount.Text = currentFraction.formatFractionNumberOnly(parsedBackAgain);
                    MessageBox.Show("Value cannot go further than 8 decimal places once converted to Whole Coins - your entry has been rounded to fit.");
                } else
                {
                    // just for fun, we'll set the textbox to the pretty-formatted version so that thousand-separators are there
                    txtSendAmount.Text = currentFraction.formatFractionNumberOnly(wholeCoinEquivalent);
                }
            }
            else
            {
                // not a number? ok, not fussed then
            }
        }

        private void txtSendAmount_TextChanged(object sender, EventArgs e)
        {

        }

        // load a logfile produced by the Wallet Printer app and get the generated Addresses from it
        private void btnLoadAddressesFromLog_Click(object sender, EventArgs e)
        {
            // show the Open dialog, and only proceed if the user properly selected a file
            if (ofdLoadPrinterLog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            List<string> addresses = new List<string>();
            bool seenAddressBlock = false;  // track if we see an address block (marked by "Generated Addresses:") at any point in the file

            // as soon as we exit this Using block, the StreamReader will be properly disposed automatically
            using (StreamReader r = new StreamReader(ofdLoadPrinterLog.FileName))
            {
                string line;
                bool inAddressBlock = false;
                while ((line = r.ReadLine()) != null)
                {
                    line = line.Trim();

                    switch (line)
                    {
                        // start paying attention to lines after we see "Generated Addresses:"
                        case "Generated Addresses:":
                            inAddressBlock = true;
                            seenAddressBlock = true;
                            break;
                        // stop paying attention after we see "end"
                        case "end":
                            inAddressBlock = false;
                            break;
                        default:
                            // if we are in the midst of a paying-attention block, add this line to the Addresses list
                            if (inAddressBlock)
                            {
                                addresses.Add(line);
                            }
                            break;
                    }
                }
            }

            // if we got any addresses out of the file, put them in the textbox
            if (addresses.Count > 0)
            {
                txtAddresses.Lines = addresses.Where(L => L.Trim() != "").ToArray();
            }
            else
            {
                // didn't find anything in the file...
                if (seenAddressBlock)
                {
                    MessageBox.Show("No addresses found in file, but it looked like the right format", "No addresses loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("File does not contain expected marker", "No addresses loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }




        }

        // "Help\Go To Wiki Page" implementation - invokes default web browser
        private void tsmiGoToWikiPage_Click(object sender, EventArgs e)
        {
            string url = "https://www.reddit.com/r/walletprint/wiki/loader";

            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.ErrorCode == -2147467259)
                {
                    MessageBox.Show(ex.Message, "No System Default Browser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Launching Browser", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
