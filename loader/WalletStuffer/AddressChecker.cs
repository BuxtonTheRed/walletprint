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
using System.Collections;


// The form frmAddressChecker (yes I like Hungarian Notation) is a little sub-utility to bulk-check the values of lists of Addresses against a desired target value

// It's the easiest way to check that you have correctly sent the intended value to each of your Paper Wallet Addresses!


namespace WalletLoader
{
    public partial class frmAddressChecker : Form
    {
        public frmAddressChecker()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            BlockIO client = new BlockIO(txtApiKey.Text.Trim());

            decimal TargetBalance;

            bool includePending = chkIncludePendingBalance.Checked;

            // basic input validations
            if (!decimal.TryParse(txtTargetBalance.Text, out TargetBalance))
            {
                MessageBox.Show("Target Balance must be a valid number", "Error: Target Balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (TargetBalance <= 0)
            {
                MessageBox.Show("Target Balance must be greater than zero", "Error: Target Balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> addresses = txtInputAddresses.Lines.Where(L => L.Trim() != "").ToList();

            lblSummaryInfo.Text = "... Working ...";


            block_io_sharp.APIResponse response; 

            // ask block.io for the balances of all these addresses, in a slightly non-crashy manner
            try
            {
                response = client.getAddressBalance("addresses", addresses);
            }
            catch (Exception ex)
            {
                lblSummaryInfo.Text = "Request failed.";
                return;
            }

            string network = (string)response.Data["network"];

            decimal totalBalanceAvail = decimal.Parse((string)response.Data["available_balance"]);
            decimal totalPending = decimal.Parse((string)response.Data["pending_received_balance"]);

            List<string> addressesExact = new List<string>();   // addresses with exactly the desired balance
            List<string> addressesZero = new List<string>();    // addresses with an exactly-zero balance
            List<string> addressesLow = new List<string>();     // addresses with a non-zero but under-spec balance
            List<string> addressesOver = new List<string>();    // addresses with a balance over the target amount

            // loop round each balance response received and stick it into the appropriate list
            foreach (Dictionary<string, object> b in (ArrayList)response.Data["balances"])
            {
                Decimal balance = decimal.Parse((string)b["available_balance"]);
                Decimal pending = decimal.Parse((string)b["pending_received_balance"]);;

                if (includePending)
                {
                    balance += pending;
                }
                
                string address = b["address"].ToString();

                if (balance == TargetBalance)
                {
                    addressesExact.Add(address);
                }
                else if (balance == 0)
                {
                    addressesZero.Add(address);
                }
                else if (balance < TargetBalance)
                {
                    addressesLow.Add(address + " (" + balance.ToString() + ")");
                }
                else if (balance > TargetBalance)
                {
                    addressesOver.Add(address + " (" + balance.ToString() + ")");
                }

            }

            // output the single-line summary
            lblSummaryInfo.Text = string.Format(
                "Coin: {0}, Total Value Available {1}, Total Value Pending {2}. Of {3} Addresses, found {4} with Zero, {5} with Under, {6} with Exact and {7} with Over",
               network,
                totalBalanceAvail,
                totalPending,
                addresses.Count,
                addressesZero.Count,
                addressesLow.Count,
                addressesExact.Count,
                addressesOver.Count);

            // and output the full lists
            txtInputAddresses.Lines = addresses.ToArray();

            txtResultsZero.Lines = addressesZero.ToArray();
            txtResultsLow.Lines = addressesLow.ToArray();
            txtResultsOK.Lines = addressesExact.ToArray();
            txtResultsOver.Lines = addressesOver.ToArray();

        }
    }
}
