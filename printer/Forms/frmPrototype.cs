using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using Casascius.Bitcoin;

using DogeAddress.walletprint;
using System.IO;

namespace DogeAddress.Forms
{
    public partial class frmPrototype : Form
    {

        #region members

        walletprint.WalletBundle bundle;

        frmTemplateEditor editor;

        #endregion

        public frmPrototype()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string aboutText = string.Format("Multi-Coin Wallet Printer by Buxton The Red.\nVersion: {0}\nSee reddit.com/r/walletprint for further help and support.", Application.ProductVersion);

            MessageBox.Show(aboutText, "About Wallet Printer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editor != null && !editor.IsDisposed)
                editor.Close();

            this.Close();
        }

        private void btnFirstPrototype_Click(object sender, EventArgs e)
        {
            int pagecount;

            if (!int.TryParse(txtNumberPages.Text, out pagecount))
            {
                MessageBox.Show("Number of Pages is required", "Number of Pages is required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (pagecount < 1 || pagecount > 99)
            {
                MessageBox.Show("Number of Pages must be between 1-99", "Invalid Number of Pages", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // todo: take this out of the UI thread, put it in a class and run on a worker thread
            // and also rename this button - it's been a long time since this was my original prototype

            string previousCaption = this.Text;

            btnFirstPrototype.Enabled = false;
            this.Text = "GENERATING...";

            this.UseWaitCursor = true;

            // todo: move all the grunt work in to a nice proper Threaded situation
            // meanwhile, DoEvents is called here to allow the wait cursor, form caption and other "I'm busy..." indicators to be shown to the user before we get into the hard stuff that takes a few moments
            Application.DoEvents();

            string outputFile = txtOutputFile.Text;
            string logFilePath = string.Format("{0} - LOG {1}.txt", outputFile, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

            StreamWriter logFile = new StreamWriter(logFilePath);

            CoinDef thiscoin = new CoinDef(bundle.template.CoinName, bundle.template.CoinAddressType, bundle.template.CoinIsWIFstupid);

            logFile.WriteLine(string.Format("Paper Wallet Printer version {0}", Application.ProductVersion));
            logFile.WriteLine(string.Format("Generating wallets for coin {0}", thiscoin.ToString()));
            logFile.WriteLine("Using template: " + bundle.template.TemplateDescription.Replace("\n", " ")); // removes any line breaks from template description as it writes it to the log file

            bundle.template.LayoutDebugging = layoutDebuggingToolStripMenuItem.Checked;

            if (bundle.template.LayoutDebugging)
            {
                logFile.WriteLine("Layout Debugging is enabled - printed wallets are not intended for live use");
            }

            // make a pdfSharp document
            PdfDocument doc = new PdfDocument();
            int batchNumber = 1;
            bool crashingOut = false;

            XImage imgArtwork = XImage.FromGdiPlusImage(bundle.getArtworkImage());

            // loop for each page
            for (int pageNumber = 1; pageNumber <= pagecount; pageNumber++)
            {
                // and put a page on it
                PdfPage page = doc.AddPage();

                // note: if you change anything that gets written to logFile from this point until the end-marker is written, this will probably cause
                // compatibility issues with the Loader tool, which expects to be able to parse the log between these points
                logFile.WriteLine(string.Format("Generating Page {0} of {1}", pageNumber, pagecount));
                logFile.WriteLine("Generated Addresses:");
                logFile.WriteLine();

                page.Size = bundle.template.pagePrintPaperSize;
                
                using (XGraphics gfx = XGraphics.FromPdfPage(page, XPageDirection.Downwards))
                {
                    // LOOP Down
                    for (int y = 0; y < bundle.template.pagePrintRows; y++)
                    {
                        // LOOP Across
                        for (int x = 0; x < bundle.template.pagePrintCols; x++)
                        {
                            try
                            {
                                // get PrivKey and Address for this new Wallet
                                KeyPair kp = KeyPair.CreateX(ExtraEntropy.GetEntropy(), false, bundle.template.CoinAddressType, bundle.template.CoinIsWIFstupid);

                                KeyCollectionItem item = new KeyCollectionItem(kp);

                                string address = item.GetAddressBase58();

                                logFile.WriteLine(address);

                                // get an XForm of our Wallet
                                using (XForm walletForm = getSingleWallet(doc, bundle, imgArtwork, address, item.PrivateKey, batchNumber, bundle.template.LayoutDebugging))
                                {
                                    // decide where on the page to draw this wallet XForm object
                                    XUnit walletLeft = XUnit.FromMillimeter(bundle.template.pagePrintLeftMarginMM + ((bundle.template.widthMM + bundle.template.pagePrintColGap) * x));
                                    XUnit walletTop = XUnit.FromMillimeter(bundle.template.pagePrintTopMarginMM + ((bundle.template.heightMM + bundle.template.pagePrintRowGap) * y));

                                    XPoint whereToPutTheForm = new XPoint(walletLeft.Point, walletTop.Point);
                                    gfx.DrawImage(walletForm, whereToPutTheForm);
                                }

                                batchNumber++;
                            }
                            catch (Exception ex)
                            {
                                // well, damn...
                                logFile.WriteLine("--- CRASH! ---");
                                logFile.WriteLine("Encountered Unhandled Exception inside primary Y-X Print Loop");
                                logFile.WriteLine("Exception:");
                                logFile.WriteLine(ex.Message);
                                logFile.WriteLine(ex.Source);
                                logFile.WriteLine(ex.StackTrace);

                                crashingOut = true;

                                break;
                            }



                        }

                        if (crashingOut)
                            break;
                    }

                }

                if (crashingOut)
                    break;

                logFile.WriteLine();
                logFile.WriteLine("end");
                logFile.WriteLine();
            }

            if (!crashingOut)
            {
                logFile.WriteLine("Internal PDF Generation completed OK");

                if (File.Exists(outputFile))
                    File.Delete(outputFile);

                try
                {
                    doc.Save(outputFile);
                }
                catch (Exception ex)
                {
                    logFile.WriteLine("--- CRASH! ---");
                    logFile.WriteLine("Encountered Unhandled Exception when Saving PDF from Object to File");
                    logFile.WriteLine("Exception:");
                    logFile.WriteLine(ex.Message);
                    logFile.WriteLine(ex.Source);
                    logFile.WriteLine(ex.StackTrace);

                    crashingOut = true;
                }

                logFile.WriteLine("PDF saved to File OK - all done");
            }


            logFile.Close();

            // if we have failed in generating the PDF in to memory, or failed when saving it to file - show an error to the user, directing them to to the log file
            if (crashingOut)
            {
                this.Text = previousCaption;
                btnFirstPrototype.Enabled = true;
                this.UseWaitCursor = false;

                string errmsg = string.Format("Sorry, Wallet Printing has failed. See the log file for details.\n\nLog file:\n{0}", logFilePath);

                MessageBox.Show(errmsg, "Printing Failed, Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            

            if (chkOpenAfterGenerating.Checked)
            {
                // note the current datetime, for comparison if we're trying to wait for PDF Viewer
                DateTime timeOfStartingViewer = DateTime.Now;

                System.Diagnostics.Process procViewer = System.Diagnostics.Process.Start(outputFile);

                if (chkWipeOutputAfterViewing.Checked)
                {
                    btnFirstPrototype.Enabled = false;

                    this.Text = "WAITING FOR PDF VIEWER TO CLOSE";

                    procViewer.WaitForExit();

                    // ok, we *think* that maybe their PDF viewer has closed. But that might not be true.

                    // check to see if we were delayed by a Suspiciously Short length of time
                    TimeSpan timeWaitedForViewer = DateTime.Now - timeOfStartingViewer;

                    if (timeWaitedForViewer.TotalSeconds < 15)
                    {
                        MessageBox.Show("I think your PDF viewer closed really quickly - or I cannot detect if you are done with the generated PDF.\n\nPlease click OK when you are ready for me to delete the generated PDF.", "Ready to delete PDF file?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }

                    // overwrite contents of PDF with junk, then delete. This won't be secure against serious forensic attack in all situations, but it's better than just-delete
                    FileInfo info = new FileInfo(outputFile);

                    byte[] junkbytes = new byte[info.Length];
                    MemSet(junkbytes, 255);
                    File.WriteAllBytes(outputFile, junkbytes);

                    File.Delete(outputFile);
                }
            }

            this.Text = previousCaption;
            btnFirstPrototype.Enabled = true;
            this.UseWaitCursor = false;

        }

        // credit to http://stackoverflow.com/a/2518800 for this function
        public static void MemSet(byte[] array, byte value)
        {
            
            const int blockSize = 4096;
            int index = 0;
            int length = Math.Min(blockSize, array.Length);
            while (index < length)
            {
                array[index++] = value;
            }
            length = array.Length;
            while (index < length)
            {
                Buffer.BlockCopy(array, 0, array, index, Math.Min(blockSize, length - index));
                index += blockSize;
            }
        }


        private float mmToPoint(float millimeters)
        {
            return millimeters * 2.834645669291f; // default unit underneath is Points which is 72 points per inch. 25.4mm per inch. 72 / 25.4 = that number.
        }

        private string addressOrReferencePrep(string address, int maxCharsPerLine, AddressPrintVariant variant, int NumberWithinBatch)
        {
            switch(variant)
            {
                case AddressPrintVariant.FullAddress:
                    return lineSplitter(address, maxCharsPerLine);
                case AddressPrintVariant.OneLine:
                    return address.Substring(0, maxCharsPerLine);
                case AddressPrintVariant.TimestampInstead:
                    return string.Format("{0}.{1}.{2}:{3}",DateTime.Now.Year, DateTime.Now.DayOfYear.ToString("000"), DateTime.Now.ToString("HHmmss"), NumberWithinBatch.ToString("00"));
                default:
                    return lineSplitter(address, maxCharsPerLine);
            }
        }

        private string lineSplitter(string input, int maxCharsPerLine)
        {
            if (input.Length <= maxCharsPerLine)
                return input;


            StringBuilder sbOut = new StringBuilder();

            string remainingInput = input;

            do{
                string newPart = remainingInput.Substring(0, maxCharsPerLine);
                remainingInput = remainingInput.Substring(maxCharsPerLine);

                sbOut.AppendLine(newPart);
            } while(remainingInput.Length > maxCharsPerLine);

            if (remainingInput.Length > 0)
                sbOut.Append(remainingInput);


            return sbOut.ToString();


        }

        // draws a single Paper Wallet in to a PdfSharp XForm
        private PdfSharp.Drawing.XForm getSingleWallet(PdfDocument doc, WalletBundle b, XImage imgArtwork, string address, string privkey, int numberWithinBatch, bool layoutDebugging)
        {
            WalletTemplate t = b.template;

            double width = t.widthMM;
            double height = t.heightMM;

            XUnit walletSizeWide = XUnit.FromMillimeter(width);
            XUnit walletSizeHigh = XUnit.FromMillimeter(height);

            PdfSharp.Drawing.XForm form = new PdfSharp.Drawing.XForm(doc, walletSizeWide, walletSizeHigh);

            
            using (XGraphics formGfx = XGraphics.FromForm(form))
            {
                XGraphicsState state = formGfx.Save();

                bool interpolateArtwork = true;
                bool interpolateQRcodes = false;

                // XImage imgArtwork is now provided by caller, so this process only has to be done ONCE - because the artwork does not change between Wallets in a run!
                //XImage imgArtwork = XImage.FromGdiPlusImage(b.getArtworkImage());
                imgArtwork.Interpolate = interpolateArtwork;

                formGfx.DrawImage(imgArtwork, new RectangleF(0f, 0f, (float)walletSizeWide.Point, (float)walletSizeHigh.Point));

                // draw the QR codes and legible-text things

                // Address
                // QR
                Bitmap bmpAddress = BtcAddress.QR.EncodeQRCode(address);
                XImage imgAddress = XImage.FromGdiPlusImage(bmpAddress);
                imgAddress.Interpolate = interpolateQRcodes;

                XUnit addressQrLeft = XUnit.FromMillimeter(t.addressQrLeftMM);
                XUnit addressQrTop = XUnit.FromMillimeter(t.addressQrTopMM);
                XUnit addressQrSize = XUnit.FromMillimeter(t.addressQrSizeMM);

                // only print Address QR if called for
                if (t.addressQrSizeMM > 0.1)
                {
                    XRect addressQrRect = new XRect(addressQrLeft.Point, addressQrTop.Point, addressQrSize.Point, addressQrSize.Point);
                    formGfx.DrawImage(imgAddress, addressQrRect);
                }

                // text address
                string addressSplitForLines = addressOrReferencePrep(address, t.addressTextCharsPerLine, t.addressTextContentVariant, numberWithinBatch);
                XFont fontAddress = new XFont(t.addressTextFontName, t.addressTextFontSize, t.addressTextFontStyle);
                XTextFormatter tf = new XTextFormatter(formGfx);

                XUnit addressTxtLeft = XUnit.FromMillimeter(t.addressTextLeftMM);
                XUnit addressTxtTop = XUnit.FromMillimeter(t.addressTextTopMM);
                XUnit addressTxtWidth = XUnit.FromMillimeter(t.addressTextWidthMM);
                XUnit addressTxtHeight = XUnit.FromMillimeter(t.addressTextHeightMM);

                XRect addressRect = new XRect(addressTxtLeft.Point, addressTxtTop.Point, addressTxtWidth.Point, addressTxtHeight.Point);
                tf.Alignment = XParagraphAlignment.Center;

                TextRotation addressTxtRotation = t.addressTextRotation;
                double addressTxtRotationDegrees = RotationMarkerToDegrees(addressTxtRotation);

                if (layoutDebugging)
                {
                    formGfx.DrawRectangle(XBrushes.PowderBlue, addressRect);
                }

                XPoint rotateCentre = new XPoint(addressTxtLeft + (addressTxtWidth / 2), addressTxtTop + (addressTxtHeight / 2));
                XPoint matrixRotatePoint = new XPoint(addressRect.X + (addressRect.Width / 2), addressRect.Y + (addressRect.Height / 2));

                XMatrix rotateMatrix = new XMatrix();
                rotateMatrix.RotateAtAppend(addressTxtRotationDegrees, rotateCentre);
                addressRect.Transform(rotateMatrix);

                if (layoutDebugging)
                {
                    // draw a little tracer dot for where the centre of rotation is going to be
                    double rotateDotSize = 2.0;
                    formGfx.DrawEllipse(XBrushes.Red, rotateCentre.X - (rotateDotSize / 2), rotateCentre.Y - (rotateDotSize / 2), rotateDotSize, rotateDotSize);
                }

                // maybe even do some rotation of the lovely text!
                formGfx.Save();

                formGfx.RotateAtTransform(addressTxtRotationDegrees, rotateCentre);
                if (layoutDebugging)
                {
                    formGfx.DrawRectangle(XPens.OrangeRed, addressRect);
                }

                if (t.addressTextWidthMM > 0.1)
                {
                    tf.DrawString(addressSplitForLines, fontAddress, t.GetBrushAddress, addressRect);
                }
                formGfx.Restore();

                // Privkey
                // QR
                Bitmap bmpPrivkey = BtcAddress.QR.EncodeQRCode(privkey);
                XImage imgPrivkey = XImage.FromGdiPlusImage(bmpPrivkey);
                imgPrivkey.Interpolate = interpolateQRcodes;

                XUnit privkeyQrLeft = XUnit.FromMillimeter(t.privkeyQrLeftMM);
                XUnit privkeyQrTop = XUnit.FromMillimeter(t.privkeyQrTopMM);
                XUnit privkeyQrSize = XUnit.FromMillimeter(t.privkeyQrSizeMM);

                XRect privkeyQrRect = new XRect(privkeyQrLeft.Point, privkeyQrTop.Point, privkeyQrSize.Point, privkeyQrSize.Point);

                // only print privkey QR if specified - but you'd have to be an UTTER IDIOT to want to exclude this. Still, user input comes first!
                if (t.privkeyQrSizeMM > 0.1)
                {
                    formGfx.DrawImage(imgPrivkey, privkeyQrRect);
                }

                // legible
                string privkeySplitForLines = lineSplitter(privkey, t.privkeyTextCharsPerLine);

                XFont fontPrivkey = new XFont(t.privkeyTextFontName, t.privkeyTextFontSize, t.privkeyTextFontStyle);

                XUnit privkeyTxtLeft = XUnit.FromMillimeter(t.privkeyTextLeftMM);
                XUnit privkeyTxtTop = XUnit.FromMillimeter(t.privkeyTextTopMM);
                XUnit privkeyTxtWidth = XUnit.FromMillimeter(t.privkeyTextWidthMM);
                XUnit privkeyTxtHeight = XUnit.FromMillimeter(t.privkeyTextHeightMM);

                TextRotation privkeyTxtRotation = t.privkeyTextRotation;
                double privkeyTxtRotationDegrees = RotationMarkerToDegrees(privkeyTxtRotation);

                XRect privkeyRect = new XRect(privkeyTxtLeft.Point, privkeyTxtTop.Point, privkeyTxtWidth.Point, privkeyTxtHeight.Point);

                if (layoutDebugging)
                {
                    // draw a tracer rectangle for the original un-rotated text rectangle
                    formGfx.DrawRectangle(XBrushes.PowderBlue, privkeyRect);
                }

                // rotate that lovely text around its middle when drawing!
                rotateCentre = new XPoint(privkeyTxtLeft + (privkeyTxtWidth / 2), privkeyTxtTop + (privkeyTxtHeight / 2));

                matrixRotatePoint = new XPoint(privkeyRect.X + (privkeyRect.Width / 2), privkeyRect.Y + (privkeyRect.Height / 2));

                rotateMatrix = new XMatrix();
                rotateMatrix.RotateAtAppend(privkeyTxtRotationDegrees, rotateCentre);
                privkeyRect.Transform(rotateMatrix);

                if (layoutDebugging)
                {
                    // draw a little tracer dot for where the centre of rotation is going to be
                    double rotateDotSize = 2.0;
                    formGfx.DrawEllipse(XBrushes.Red, rotateCentre.X - (rotateDotSize / 2), rotateCentre.Y - (rotateDotSize / 2), rotateDotSize, rotateDotSize);
                }

                formGfx.Save();

                formGfx.RotateAtTransform(privkeyTxtRotationDegrees, rotateCentre);

                if (layoutDebugging)
                {
                    formGfx.DrawRectangle(XPens.OrangeRed, privkeyRect);
                }

                // only print privkey text if specified.
                if (t.privkeyTextWidthMM > 0.1)
                {
                    tf.DrawString(privkeySplitForLines, fontPrivkey, t.GetBrushPrivkey, privkeyRect);
                }

                formGfx.Restore();
            }



            return form;
        }


        private XRect MakeXRectForRotation(double x, double y, double width, double height, TextRotation rotation)
        {
            double _x = x;
            double _y = y;

            double _width = width;
            double _height = height;
            
            if (rotation == TextRotation.Clockwise || rotation == TextRotation.CounterClockwise)
            {
                _width = height;
                _height = width;
            }


            XRect output = new XRect(_x, _y, _width, _height);

            return output;
        }

        private double RotationMarkerToDegrees(TextRotation rotation)
        {
            switch (rotation)
            {
                case TextRotation.CounterClockwise:
                    return -90;
                case TextRotation.Clockwise:
                    return 90;
                case TextRotation.UpsideDown:
                    return 180;
                default:
                    return 0;
            }
        }

        private void btnBrowseTemplate_Click(object sender, EventArgs e)
        {
            DialogResult result = ofdLoadBundle.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtTemplateFilePath.Text = ofdLoadBundle.FileName;

                FileInfo info = new FileInfo(txtTemplateFilePath.Text);
                // harvest some free bonus entropy from the size, filepath, and last access time of the selected template
                ExtraEntropy.AddExtraEntropy(info.Length.ToString() + txtTemplateFilePath.Text  + info.LastAccessTimeUtc.ToLongTimeString());
                // note: this is NOT the only entropy in use - it just pleases me to put some extra in there because we can

                try
                {
                    bundle = new WalletBundle(txtTemplateFilePath.Text);
                } catch (Exception ex)
                {
                    MessageBox.Show("Failed to load bundle file\n" + ex.ToString(), "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtTemplateDescription.Text = bundle.template.TemplateDescription;

                if (bundle.template.CoinIsWIFstupid)
                {
                    lblTemplateInfo.Text = string.Format("Loaded wallet for coin '{0}', which uses Address Version {1} with WIF-stupid mode", bundle.template.CoinName, bundle.template.CoinAddressType);
                }
                else
                {
                    lblTemplateInfo.Text = string.Format("Loaded wallet for coin '{0}', which uses Address Version {1}", bundle.template.CoinName, bundle.template.CoinAddressType);
                }
                
                btnBrowseOutput.Enabled = true;
            }
        }

        private void frmPrototype_Load(object sender, EventArgs e)
        {

        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            DialogResult result = sfdOutputFile.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtOutputFile.Text = sfdOutputFile.FileName;

                btnFirstPrototype.Enabled = true;

                ExtraEntropy.AddExtraEntropy(txtOutputFile.Text);
            }
        }

        private void chkOpenAfterGenerating_CheckedChanged(object sender, EventArgs e)
        {
            chkWipeOutputAfterViewing.Enabled = chkOpenAfterGenerating.Checked;
        }

        private void btnOpenEditor_Click(object sender, EventArgs e)
        {
            if (editor == null || editor.IsDisposed)
            {
                editor = new frmTemplateEditor();
            }

            if (bundle != null)
            {
                MessageBox.Show("If you make any changes to the currently-selected template file, you will need to Save them in the Editor and re-Browse to see them", "Tedious Information Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            editor.Show();
            
        }

        private void layoutDebuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            layoutDebuggingToolStripMenuItem.Checked = !layoutDebuggingToolStripMenuItem.Checked;

        }

        // "Help\Go To Wiki Page" implementation - invokes default web browser
        private void tsmiGoToWikiPage_Click(object sender, EventArgs e)
        {
            string url = "https://www.reddit.com/r/walletprint/wiki/printer";

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

        private void txtNumberPages_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            try
            {
                int x = int.Parse(txt.Text);
                if (x >= 1 && x <= 99)
                {
                    errorProvider1.SetError(txt, "");
                }
                else
                {
                    errorProvider1.SetError(txt, "Must be in range 1-99");
                }

                
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txt, "Not a valid whole (integer) number");
            }
        }
    }
}
