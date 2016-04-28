using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace DogeAddress.Forms
{
    public partial class frmTemplateEditor : Form
    {
        public frmTemplateEditor()
        {
            InitializeComponent();

            // wire up Validation handlers for number-centric text boxes
            txtPageLeftMargin.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtPageTopMargin.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtPageCols.Validating += new CancelEventHandler(HandlerValidateInt);
            txtPageRows.Validating += new CancelEventHandler(HandlerValidateInt);
            txtPageGapCols.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtPageGapRows.Validating += new CancelEventHandler(HandlerValidateDouble);

            txtWalletWidth.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtWalletHeight.Validating += new CancelEventHandler(HandlerValidateDouble);

            //Address
            txtAddressQrLeft.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtAddressQrTop.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtAddressQrSize.Validating += new CancelEventHandler(HandlerValidateDouble);

            txtAddressTextLeft.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtAddressTextTop.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtAddressTextWidth.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtAddressTextHeight.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtAddressTextSize.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtAddressTextLineLength.Validating += new CancelEventHandler(HandlerValidateInt);

            //Privkey
            txtPrivkeyQrLeft.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtPrivkeyQrTop.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtPrivkeyQrSize.Validating += new CancelEventHandler(HandlerValidateDouble);

            txtPrivkeyTextLeft.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtPrivkeyTextTop.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtPrivkeyTextWidth.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtPrivkeyTextHeight.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtPrivkeyTextSize.Validating += new CancelEventHandler(HandlerValidateDouble);
            txtPrivkeyTextLineLength.Validating += new CancelEventHandler(HandlerValidateInt);

        }

        private void frmTemplateEditor_Load(object sender, EventArgs e)
        {
            ddlPaperSize.DataSource = Enum.GetValues(typeof(PdfSharp.PageSize));

            ddlAddressTextColour.DataSource = Enum.GetValues(typeof(walletprint.LimitedColourPallete));
            ddlPrivkeyTextColour.DataSource = Enum.GetValues(typeof(walletprint.LimitedColourPallete));

            ddlAddressTextStyle.DataSource = Enum.GetValues(typeof(PdfSharp.Drawing.XFontStyle));
            ddlPrivkeyTextStyle.DataSource = Enum.GetValues(typeof(PdfSharp.Drawing.XFontStyle));

            ddlAddressTextRotation.DataSource = Enum.GetValues(typeof(walletprint.TextRotation));
            ddlPrivkeyTextRotation.DataSource = Enum.GetValues(typeof(walletprint.TextRotation));


            IList<string> fontNames = FontFamily.Families.Select(f => f.Name).ToList();
            ddlAddressTextFont.DataSource = fontNames;

            // for some weird reason, the two Font dropdowns ended up kinda bound to one another. Using two distinct data source objects of the same damn contents fixes it.
            IList<string> fontNamesAgain = FontFamily.Families.Select(f => f.Name).ToList();
            ddlPrivkeyTextFont.DataSource = fontNamesAgain;

            coindefs = walletprint.CoinLibrary.AllCoins();

            ddlCoinSelect.DataSource = coindefs;

            bundle = new walletprint.WalletBundle();

        }

        #region validation
        private void textValidateDouble(TextBox txt)
        {
            try
            {
                double x = double.Parse(txt.Text);
                errorProvider1.SetError(txt, "");
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txt, "Not a valid number");
            }
        }

        private void textValidateInt(TextBox txt)
        {
            try
            {
                int x = int.Parse(txt.Text);
                errorProvider1.SetError(txt, "");
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txt, "Not a valid whole (integer) number");
            }
        }

        private void HandlerValidateDouble(object sender, CancelEventArgs e)
        {
            textValidateDouble((TextBox)sender);
        }

        private void HandlerValidateInt(object sender, CancelEventArgs e)
        {
            textValidateInt((TextBox)sender);
        }


        private bool ParseOrComplainDouble(TextBox txt, ref double result, string fieldname)
        {
            if (!double.TryParse(txt.Text, out result))
            {
                MessageBox.Show(fieldname + " is not a valid number, must fix before Saving", "Error in '" + fieldname + "'", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ParseOrComplainInt(TextBox txt, ref int result, string fieldname)
        {
            if (!int.TryParse(txt.Text, out result))
            {
                MessageBox.Show(fieldname + " is not a valid whole (integer) number, must fix before Saving", "Error in '" + fieldname + "'", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        #endregion

        #region Private Members
        
        walletprint.WalletBundle bundle;

        int artworkPixelWidth = 0;

        List<walletprint.CoinDef> coindefs;

        bool closingMyself = false;

        #endregion


        #region PRIVATE: Load and Save from-and-to WalletTemplate objects
        
        void LoadTemplate(walletprint.WalletTemplate template)
        {
            txtTemplateDescription.Text = template.TemplateDescription;

            // try to identify the stored Coin as being one of the existing entries...

            walletprint.CoinDef matchedCoin = coindefs.Where(c => c.Version == template.CoinAddressType && c.isWIFstupid == template.CoinIsWIFstupid && c.Name.ToLower() == template.CoinName.ToLower()).FirstOrDefault();

            if (matchedCoin != null)
            {
                ddlCoinSelect.SelectedItem = matchedCoin;
            }
            else
            {
                AddGuestCoin(template.CoinName, template.CoinAddressType, template.CoinIsWIFstupid);
            }

            //page
            ddlPaperSize.SelectedItem = template.pagePrintPaperSize;
            txtPageLeftMargin.Text = template.pagePrintLeftMarginMM.ToString();
            txtPageTopMargin.Text = template.pagePrintTopMarginMM.ToString();
            txtPageCols.Text = template.pagePrintCols.ToString();
            txtPageRows.Text = template.pagePrintRows.ToString();
            txtPageGapCols.Text = template.pagePrintColGap.ToString();
            txtPageGapRows.Text = template.pagePrintRowGap.ToString();

            txtWalletWidth.Text = template.widthMM.ToString();
            txtWalletHeight.Text = template.heightMM.ToString();

            //Address
            txtAddressQrLeft.Text = template.addressQrLeftMM.ToString();
            txtAddressQrTop.Text = template.addressQrTopMM.ToString();
            txtAddressQrSize.Text = template.addressQrSizeMM.ToString();

            txtAddressTextLeft.Text = template.addressTextLeftMM.ToString();
            txtAddressTextTop.Text = template.addressTextTopMM.ToString();
            txtAddressTextWidth.Text = template.addressTextWidthMM.ToString();
            txtAddressTextHeight.Text = template.addressTextHeightMM.ToString();

            
            switch(template.addressTextContentVariant)
            {
                case walletprint.AddressPrintVariant.FullAddress:
                    rbFullAddress.Checked = true;
                    break;
                case walletprint.AddressPrintVariant.OneLine:
                    rbOneLineAddress.Checked = true;
                    break;
                case walletprint.AddressPrintVariant.TimestampInstead:
                    rbTimestampAndSerialInstead.Checked = true;
                    break;
            }

            IList<string> fontNames = FontFamily.Families.Select(f => f.Name).ToList();

            if (fontNames.Contains(template.addressTextFontName))
            {
                ddlAddressTextFont.SelectedItem = template.addressTextFontName;
            } else
            {
                MessageBox.Show("This template uses a font '" + template.addressTextFontName + "' which you don't have! Please select alternative font for Address Text.", "Address Text Font missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            txtAddressTextSize.Text = template.addressTextFontSize.ToString();
            txtAddressTextLineLength.Text = template.addressTextCharsPerLine.ToString();

            ddlAddressTextColour.SelectedItem = template.addressTextColour;
            ddlAddressTextStyle.SelectedItem = template.addressTextFontStyle;
            ddlAddressTextRotation.SelectedItem = template.addressTextRotation;
            
            //Privkey
            txtPrivkeyQrLeft.Text = template.privkeyQrLeftMM.ToString();
            txtPrivkeyQrTop.Text = template.privkeyQrTopMM.ToString();
            txtPrivkeyQrSize.Text = template.privkeyQrSizeMM.ToString();

            txtPrivkeyTextLeft.Text = template.privkeyTextLeftMM.ToString();
            txtPrivkeyTextTop.Text = template.privkeyTextTopMM.ToString();
            txtPrivkeyTextWidth.Text = template.privkeyTextWidthMM.ToString();
            txtPrivkeyTextHeight.Text = template.privkeyTextHeightMM.ToString();

            if (fontNames.Contains(template.addressTextFontName))
            {
                ddlPrivkeyTextFont.SelectedItem = template.privkeyTextFontName;
            }
            else
            {
                MessageBox.Show("This template uses a font '" + template.privkeyTextFontName + "' which you don't have! Please select alternative font for Private Key Text.", "Private Key Text Font missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            txtPrivkeyTextSize.Text = template.privkeyTextFontSize.ToString();
            txtPrivkeyTextLineLength.Text = template.privkeyTextCharsPerLine.ToString();

            ddlPrivkeyTextColour.SelectedItem = template.privkeyTextColour;
            ddlPrivkeyTextStyle.SelectedItem = template.privkeyTextFontStyle;
            ddlPrivkeyTextRotation.SelectedItem = template.privkeyTextRotation;
        }

        walletprint.WalletTemplate SaveTemplate()
        {
            walletprint.WalletTemplate t = new walletprint.WalletTemplate();

            t.TemplateDescription = txtTemplateDescription.Text;

            walletprint.CoinDef coin = (walletprint.CoinDef)ddlCoinSelect.SelectedItem;
            t.CoinName = coin.Name;
            t.CoinAddressType = coin.Version;
            t.CoinIsWIFstupid = coin.isWIFstupid;

            t.pagePrintPaperSize = (PdfSharp.PageSize)ddlPaperSize.SelectedItem;

            bool IsOk = true;
            //page
            IsOk = IsOk & ParseOrComplainDouble(txtPageLeftMargin, ref t.pagePrintLeftMarginMM, "Page Left Margin");
            IsOk = IsOk & ParseOrComplainDouble(txtPageTopMargin, ref t.pagePrintTopMarginMM, "Page Top Margin");
            IsOk = IsOk & ParseOrComplainInt(txtPageCols, ref t.pagePrintCols, "Page Columns");
            IsOk = IsOk & ParseOrComplainInt(txtPageRows, ref t.pagePrintRows, "Page Rows");
            IsOk = IsOk & ParseOrComplainDouble(txtPageGapCols, ref t.pagePrintColGap, "Page Gap Between Columns");
            IsOk = IsOk & ParseOrComplainDouble(txtPageGapRows, ref t.pagePrintRowGap, "Page Gap Between Rows");

            // wallet
            IsOk = IsOk & ParseOrComplainDouble(txtWalletWidth, ref t.widthMM, "Wallet Width");
            IsOk = IsOk & ParseOrComplainDouble(txtWalletHeight, ref t.heightMM, "Wallet Height");

            // address
            IsOk = IsOk & ParseOrComplainDouble(txtAddressQrLeft, ref t.addressQrLeftMM, "Address QR Left");
            IsOk = IsOk & ParseOrComplainDouble(txtAddressQrTop, ref t.addressQrTopMM, "Address QR Top");
            IsOk = IsOk & ParseOrComplainDouble(txtAddressQrSize, ref t.addressQrSizeMM, "Address QR Size");

            IsOk = IsOk & ParseOrComplainDouble(txtAddressTextLeft, ref t.addressTextLeftMM, "Address Text Left");
            IsOk = IsOk & ParseOrComplainDouble(txtAddressTextTop, ref t.addressTextTopMM, "Address Text Top");
            IsOk = IsOk & ParseOrComplainDouble(txtAddressTextWidth, ref t.addressTextWidthMM, "Address Text Width");
            IsOk = IsOk & ParseOrComplainDouble(txtAddressTextHeight, ref t.addressTextHeightMM, "Address Text Height");

            t.addressTextFontName = (string)ddlAddressTextFont.SelectedItem;
            IsOk = IsOk & ParseOrComplainDouble(txtAddressTextSize, ref t.addressTextFontSize, "Address Text Font Size");
            IsOk = IsOk & ParseOrComplainInt(txtAddressTextLineLength, ref t.addressTextCharsPerLine, "Address Text Line Length");

            t.addressTextColour = (walletprint.LimitedColourPallete)ddlAddressTextColour.SelectedItem;
            t.addressTextFontStyle = (PdfSharp.Drawing.XFontStyle)ddlAddressTextStyle.SelectedItem;
            t.addressTextRotation = (walletprint.TextRotation)ddlAddressTextRotation.SelectedItem;

            if (rbFullAddress.Checked)
                t.addressTextContentVariant = walletprint.AddressPrintVariant.FullAddress;
            else if (rbOneLineAddress.Checked)
                t.addressTextContentVariant = walletprint.AddressPrintVariant.OneLine;
            else if (rbTimestampAndSerialInstead.Checked)
                t.addressTextContentVariant = walletprint.AddressPrintVariant.TimestampInstead;

            // privkey
            IsOk = IsOk & ParseOrComplainDouble(txtPrivkeyQrLeft, ref t.privkeyQrLeftMM, "Private Key QR Left");
            IsOk = IsOk & ParseOrComplainDouble(txtPrivkeyQrTop, ref t.privkeyQrTopMM, "Private Key QR Top");
            IsOk = IsOk & ParseOrComplainDouble(txtPrivkeyQrSize, ref t.privkeyQrSizeMM, "Private Key QR Size");

            IsOk = IsOk & ParseOrComplainDouble(txtPrivkeyTextLeft, ref t.privkeyTextLeftMM, "Private Key Text Left");
            IsOk = IsOk & ParseOrComplainDouble(txtPrivkeyTextTop, ref t.privkeyTextTopMM, "Private Key Text Top");
            IsOk = IsOk & ParseOrComplainDouble(txtPrivkeyTextWidth, ref t.privkeyTextWidthMM, "Private Key Text Width");
            IsOk = IsOk & ParseOrComplainDouble(txtPrivkeyTextHeight, ref t.privkeyTextHeightMM, "Private Key Text Height");

            t.privkeyTextFontName = (string)ddlPrivkeyTextFont.SelectedItem;
            IsOk = IsOk & ParseOrComplainDouble(txtPrivkeyTextSize, ref t.privkeyTextFontSize, "Private Key Text Font Size");
            IsOk = IsOk & ParseOrComplainInt(txtPrivkeyTextLineLength, ref t.privkeyTextCharsPerLine, "Private Key Text Line Length");

            t.privkeyTextColour = (walletprint.LimitedColourPallete)ddlPrivkeyTextColour.SelectedItem;
            t.privkeyTextFontStyle = (PdfSharp.Drawing.XFontStyle)ddlPrivkeyTextStyle.SelectedItem;
            t.privkeyTextRotation = (walletprint.TextRotation)ddlPrivkeyTextRotation.SelectedItem;


            if (IsOk) {
                return t;
            }
            else
            {
                return null;
            }
        }

        #endregion


        #region Public: Load and Save from-and-to Bundle Files

        public void LoadBundle(string filepath)
        {
            this.bundle = new walletprint.WalletBundle(filepath);

            LoadTemplate(this.bundle.template);

            LoadArtworkPixelWidth();

            // because bundles won't be saved without valid Artwork, we can automatically unlock the artwork-based UI things when we load a saved bundle
            UpdateArtworkUI();
        }

        public void SaveBundle(string filepath)
        {
            // this.bundle.template = SaveTemplate(); // this is now done by the caller, as we want error dialogs to fire before the Save file prompt

            if (this.bundle.template != null)
            {
                this.bundle.SaveBundle(filepath);
            }

            // no need to throw up further error dialogs at this point - the SaveTemplate function did that already
        }

        #endregion


        #region handy internal methods

        void UpdateArtworkUI()
        {
            btnSetWalletHeightFromArtworkAspect.Enabled = true;
            btnWalletExportArtwork.Enabled = true;

            UpdateArtworkLabel();
            
        }

        void LoadArtworkPixelWidth()
        {
            Image artwork = this.bundle.getArtworkImage();

            if (artwork == null)
            {
                artworkPixelWidth = 0;
            }
            else
            {
                artworkPixelWidth = artwork.Width;
            }
        }

        void UpdateArtworkLabel()
        {
            // if Wallet Width is defined, get it
            double walletWidthMM;

            if (artworkPixelWidth == 0)
            {
                lblArtworkStatus.Text = "Artwork Not Imported";
            } else
            {

                if (double.TryParse(txtWalletWidth.Text, out walletWidthMM))
                {
                    // convert wallet width in MM to Inches
                    double walletWidthInch = walletWidthMM * 0.0393701;

                    double artworkDpi = (double)artworkPixelWidth / walletWidthInch;

                    string dpiEvaluation;

                    if (artworkDpi < 150)
                        dpiEvaluation = "Very Bad";
                    else if (artworkDpi >= 150 && artworkDpi < 200)
                        dpiEvaluation = "Poor";
                    else if (artworkDpi >= 200 && artworkDpi < 300)
                        dpiEvaluation = "Fair";
                    else if (artworkDpi >= 300 && artworkDpi < 450)
                        dpiEvaluation = "Good";
                    else if (artworkDpi >= 450 && artworkDpi < 650)
                        dpiEvaluation = "Great";
                    else if (artworkDpi >= 650 && artworkDpi < 800)
                        dpiEvaluation = "More Than Enough";
                    else if (artworkDpi >= 800 && artworkDpi < 1000)
                        dpiEvaluation = "Completely Excessive...";
                    else if (artworkDpi >= 1000)
                        dpiEvaluation = "PLEASE REDUCE Artwork pixel size!";
                    else
                        dpiEvaluation = "DPI evaluation failed";

                    lblArtworkStatus.Text = String.Format("Imported, {0:0.0} DPI - {1}", artworkDpi, dpiEvaluation);

                }
                else
                {
                    lblArtworkStatus.Text = "Loaded (DPI unknown, set Wallet Width)";
                }
            }

            
        }

        void AddGuestCoin(string coinName, byte addresstype, bool isWIFstupid)
        {
            walletprint.CoinDef newCoin = new walletprint.CoinDef(coinName, addresstype, isWIFstupid);
            coindefs.Add(newCoin);

            ddlCoinSelect.DataSource = null;

            ddlCoinSelect.DataSource = coindefs;

            ddlCoinSelect.SelectedItem = newCoin;
        }



        #endregion



        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            // use LoadBundle method defined above

            if (MessageBox.Show("Opening a Template Bundle file will overwrite the current Template AND Artwork. Are you SURE you want to Load?", "Confirm Load", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            DialogResult ofdResult = ofdLoadBundle.ShowDialog();

            if (ofdResult != System.Windows.Forms.DialogResult.OK)
            {
                // they bailed out...
                return;
            }

            string importPath = ofdLoadBundle.FileName;

            LoadBundle(importPath);

        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            // use SaveBundle method defined above to do the grunt work!
            if (this.bundle.artwork == null)
            {
                MessageBox.Show("You must Import Artwork before you can save this Template", "Can't Save Template - Artwork Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // attempting to save the template to the internal Bundle will cover all the validation
            this.bundle.template = SaveTemplate();

            if (this.bundle.template == null)
            {
                // we didn't save, due to failure of validation
                return;
            }

            DialogResult sfdResult = sfdSaveBundle.ShowDialog();

            if (sfdResult != System.Windows.Forms.DialogResult.OK)
            {
                // they bailed...
                return;
            }

            string outputPath = sfdSaveBundle.FileName;

            bundle.SaveBundle(outputPath);

        }

        private void btnSetWalletHeightFromArtworkAspect_Click(object sender, EventArgs e)
        {
            // this should only be clickable once an Artwork has been added, but let's check anyway...
            if (this.bundle.artwork == null)
            {
                MessageBox.Show("Can't do this, no Artwork is currently loaded for this template", "Artwork required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double WalletWidth = 0;

            if (!ParseOrComplainDouble(txtWalletWidth, ref WalletWidth, "Wallet Width") )
            {
                MessageBox.Show("Can't do this, Wallet Width is not specified yet", "Wallet Width required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Image imgArtwork = this.bundle.getArtworkImage();

            double ArtworkAspect = (double)imgArtwork.Height / (double)imgArtwork.Width;

            double niceHeight = ArtworkAspect * WalletWidth;

            txtWalletHeight.Text = niceHeight.ToString("###0.0");

        }

        private void btnWalletImportArtwork_Click(object sender, EventArgs e)
        {
            if (bundle.artwork != null)
            {
                // check that the user intends to overwrite the currently-stored Artwork
                if (MessageBox.Show("This will replace the currently-loaded Artwork, are you sure you want to do that?\n(You may wish to Export the old artwork first...)", "Replace Artwork?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                {
                    // if they said Cancel, then bail out. Otherwise, they said OK and we continue
                    return;
                }
            }

            // trigger the relevant Open File Dialog to get the user to pick their Artwork file
            DialogResult ofdResult = ofdImportArtwork.ShowDialog();
            if (ofdResult != System.Windows.Forms.DialogResult.OK)
            {
                // guess they cancelled out
                return;
            }

            string artworkFilePath = ofdImportArtwork.FileName;

            // check that the selected file is a legit Image of some sort...
            try
            {
                Image imgtest = Image.FromFile(artworkFilePath);
                int imgwidth = imgtest.Width;
                artworkPixelWidth = imgwidth;
                imgtest.Dispose();
            }
            catch   (Exception ex)
            {
                // woops!
                MessageBox.Show("Selected image file is invalid, open in another application, or some similar error\nException: " + ex.ToString(), "Can't load image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.bundle.importArtwork(artworkFilePath);

            UpdateArtworkUI();

        }

        private void btnWalletExportArtwork_Click(object sender, EventArgs e)
        {
            // this should only be clickable once an Artwork has been added, but let's check anyway...
            if (this.bundle.artwork == null)
            {
                MessageBox.Show("Can't do this, no Artwork is currently loaded for this template", "Artwork required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sfdExportArtwork.DefaultExt = this.bundle.artworkFileExten;

            sfdExportArtwork.Filter = string.Format("{0} files|*.{0}|All Files|*.*", this.bundle.artworkFileExten);

            DialogResult sfdResult = sfdExportArtwork.ShowDialog();

            if(sfdResult != System.Windows.Forms.DialogResult.OK)
            {
                // they must've cancelled out of the dialog
                return;
            }

            string exportPath = sfdExportArtwork.FileName;

            bundle.exportArtwork(exportPath);

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure you're ready to leave the Editor? I don't know if you have any unsaved changes...", "Confirm Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            else
            {
                closingMyself = true;
                this.Close();
            }
        }

        private void frmTemplateEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !closingMyself)
            {
                if (MessageBox.Show("Sure you're ready to leave the Editor? I don't know if you have any unsaved changes...", "Confirm Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void btnAddGuestCoin_Click(object sender, EventArgs e)
        {
            string CoinName = Microsoft.VisualBasic.Interaction.InputBox("What is the Name (just the name) of this new coin?", "NAME of coin?");

            if (CoinName == "")
            {
                return;
            }

            string sCoinType = Microsoft.VisualBasic.Interaction.InputBox("What is the Address Version Number, also called the Network Version?\n(See the Wiki for details)", "ADDRESS VERSION of coin?");

            if (sCoinType == "")
            {
                return;
            }

            byte bCoinType;
            bool isWIFstupid = false;

            if (sCoinType.Contains('!'))
                isWIFstupid = true;

            if (byte.TryParse(sCoinType.Replace("!", ""), out bCoinType))
            {
                AddGuestCoin(CoinName, bCoinType, isWIFstupid);
            } else
            {
                MessageBox.Show("Version Number must be a number from 0-127, in decimal", "Invalid Version", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ddlAddressTextFont_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ddlPrivkeyTextFont_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtWalletWidth_TextChanged(object sender, EventArgs e)
        {
            UpdateArtworkLabel();
        }
    }
}
