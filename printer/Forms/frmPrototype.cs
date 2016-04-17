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
            MessageBox.Show("Multi-Coin Wallet Printer by Buxton The Red\n\nBased on Bitcoin Address Utility by Casascius\n(Licence: GNU GPL v3)\n\nhttp://reddit.com/r/walletprint for support and discussion", "About Multi-Coin Wallet Printer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editor != null && !editor.IsDisposed)
                editor.Close();

            this.Close();
        }

        private void btnFirstPrototype_Click(object sender, EventArgs e)
        {
            // todo: take this out of the UI thread, put it in a class and run on a worker thread

            string previousCaption = this.Text;

            btnFirstPrototype.Enabled = false;
            this.Text = "GENERATING...";

            this.UseWaitCursor = true;

            string outputFile = txtOutputFile.Text;
            string logFilePath = string.Format("{0} - LOG {1}.txt", outputFile, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

            StreamWriter logFile = new StreamWriter(logFilePath);

            CoinDef thiscoin = new CoinDef(bundle.template.CoinName, bundle.template.CoinAddressType, bundle.template.CoinIsWIFstupid);

            logFile.WriteLine(string.Format( "Generating wallets for coin {0}", thiscoin.ToString()));
            logFile.WriteLine("Using template: " + bundle.template.TemplateDescription);

            bundle.template.LayoutDebugging = layoutDebuggingToolStripMenuItem.Checked;

            if (bundle.template.LayoutDebugging)
            {
                logFile.WriteLine("Layout Debugging is enabled - printed wallets are not intended for live use");
            }

            // make a pdfSharp document
            PdfDocument doc = new PdfDocument();

            // and put a page on it
            PdfPage page = doc.AddPage();

            int batchNumber = 1;

            logFile.WriteLine("Generated Addresses:");
            logFile.WriteLine();

            page.Size = bundle.template.pagePrintPaperSize;

            using (XGraphics gfx = XGraphics.FromPdfPage(page, XPageDirection.Downwards))
            {

                // LOOP
                for (int y = 0; y < bundle.template.pagePrintRows; y++)
                {

                    for (int x = 0; x < bundle.template.pagePrintCols; x++)
                    {
                        // get PrivKey and Address for this new Wallet
                        KeyPair kp = KeyPair.CreateX(ExtraEntropy.GetEntropy(), false, bundle.template.CoinAddressType, bundle.template.CoinIsWIFstupid);
                        
                        KeyCollectionItem item = new KeyCollectionItem(kp);

                        string address = item.GetAddressBase58();

                        logFile.WriteLine(address);

                        // get an XForm of our Wallet
                        XForm walletForm = getSingleWallet(doc, bundle, address, item.PrivateKey, batchNumber, bundle.template.LayoutDebugging);
                        batchNumber++;

                        // make a wild-assed guess at where to draw the Form
                        XUnit walletLeft = XUnit.FromMillimeter(bundle.template.pagePrintLeftMarginMM + ((bundle.template.widthMM + bundle.template.pagePrintColGap) * x));
                        XUnit walletTop = XUnit.FromMillimeter(bundle.template.pagePrintTopMarginMM + ((bundle.template.heightMM + bundle.template.pagePrintRowGap) * y));

                        XPoint whereToPutTheForm = new XPoint(walletLeft.Point, walletTop.Point);
                        gfx.DrawImage(walletForm, whereToPutTheForm);

                        // ENDLOOP
                    }
                }

            }

            logFile.WriteLine();
            logFile.WriteLine("end");

            logFile.Close();
            
            if (File.Exists(outputFile))
                File.Delete(outputFile);

            doc.Save(outputFile);

            if (chkOpenAfterGenerating.Checked)
            {
                System.Diagnostics.Process procViewer = System.Diagnostics.Process.Start(outputFile);

                if (chkWipeOutputAfterViewing.Checked)
                {
                    btnFirstPrototype.Enabled = false;

                    this.Text = "WAITING FOR PDF VIEWER TO CLOSE";

                    procViewer.WaitForExit();

                    // do some kind of deleting and/or overwriting on the output file here
                    
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

        private PdfSharp.Drawing.XForm getSingleWallet(PdfDocument doc, WalletBundle b, string address, string privkey, int numberWithinBatch, bool layoutDebugging)
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

                formGfx.DrawImage(XImage.FromGdiPlusImage(b.getArtworkImage()), new RectangleF(0f, 0f, (float)walletSizeWide.Point, (float)walletSizeHigh.Point));

                // draw the QR codes and legible-text things

                // Address
                // QR
                Bitmap bmpAddress = BtcAddress.QR.EncodeQRCode(address);
                XImage imgAddress = XImage.FromGdiPlusImage(bmpAddress);
                imgAddress.Interpolate = false;

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
                imgPrivkey.Interpolate = false;

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



        //private PdfSharp.Drawing.XForm getPrototypeWallet(PdfDocument doc, string address, string privkey, bool layoutDebugging)
        //{
        //    int width = 188;
        //    int height = 40;

        //    XUnit walletSizeWide = XUnit.FromMillimeter(width);
        //    XUnit walletSizeHigh = XUnit.FromMillimeter(height);

        //    PdfSharp.Drawing.XForm form = new PdfSharp.Drawing.XForm(doc, walletSizeWide, walletSizeHigh);

        //    // put the background graphic on it
        //    string imagepath = @"D:\!!DROPBOX\Dropbox\Dogecoin\paper wallets\My Gift Card system\my Wallet Printer app\assets\blank-v2.png";

        //    using (XGraphics formGfx = XGraphics.FromForm(form))
        //    {
        //        XGraphicsState state = formGfx.Save();

        //        formGfx.DrawImage(XImage.FromFile(imagepath), new RectangleF(0f, 0f, (float)walletSizeWide.Point, (float)walletSizeHigh.Point));

        //        // draw the QR codes and legible-text things

        //        // Address
        //        // QR
        //        Bitmap bmpAddress = BtcAddress.QR.EncodeQRCode(address);
        //        XImage imgAddress = XImage.FromGdiPlusImage(bmpAddress);
        //        imgAddress.Interpolate = false;

        //        XUnit addressQrLeft = XUnit.FromMillimeter(35.15);
        //        XUnit addressQrTop = XUnit.FromMillimeter(6.95);
        //        XUnit addressQrSize = XUnit.FromMillimeter(26.44);

        //        XRect addressQrRect = new XRect(addressQrLeft.Point, addressQrTop.Point, addressQrSize.Point, addressQrSize.Point);

        //        formGfx.DrawImage(imgAddress, addressQrRect);

        //        // legible
        //        string addressSplitForLines = lineSplitter(address, 12);
        //        XFont fontAddress = new XFont("Courier New", 10, XFontStyle.Regular);
        //        XTextFormatter tf = new XTextFormatter(formGfx);

        //        XUnit addressTxtLeft = XUnit.FromMillimeter(4.16);
        //        XUnit addressTxtTop = XUnit.FromMillimeter(20.95);
        //        XUnit addressTxtWidth = XUnit.FromMillimeter(27.65);
        //        XUnit addressTxtHeight = XUnit.FromMillimeter(12.36);

        //        XRect addressRect = new XRect(addressTxtLeft.Point, addressTxtTop.Point, addressTxtWidth.Point, addressTxtHeight.Point);
        //        tf.Alignment = XParagraphAlignment.Center;

        //        TextRotation addressTxtRotation = TextRotation.Normal;
        //        double addressTxtRotationDegrees = RotationMarkerToDegrees(addressTxtRotation);

        //        if (layoutDebugging)
        //        {
        //            formGfx.DrawRectangle(XBrushes.PowderBlue, addressRect);
        //        }

        //        XPoint rotateCentre = new XPoint(addressTxtLeft + (addressTxtWidth / 2), addressTxtTop + (addressTxtHeight / 2));
        //        XPoint matrixRotatePoint = new XPoint(addressRect.X + (addressRect.Width / 2), addressRect.Y + (addressRect.Height / 2));

        //        XMatrix rotateMatrix = new XMatrix();
        //        rotateMatrix.RotateAtAppend(addressTxtRotationDegrees, rotateCentre);
        //        addressRect.Transform(rotateMatrix);

        //        if (layoutDebugging)
        //        {
        //            // draw a little tracer dot for where the centre of rotation is going to be
        //            double rotateDotSize = 2.0;
        //            formGfx.DrawEllipse(XBrushes.Red, rotateCentre.X - (rotateDotSize / 2), rotateCentre.Y - (rotateDotSize / 2), rotateDotSize, rotateDotSize);
        //        }

        //        // maybe even do some rotation of the lovely text!
        //        formGfx.Save();
                
        //        formGfx.RotateAtTransform(0, rotateCentre);
        //        if (layoutDebugging)
        //        {
        //            formGfx.DrawRectangle(XPens.OrangeRed, addressRect);
        //        }
        //        tf.DrawString(addressSplitForLines, fontAddress, XBrushes.Black, addressRect);
        //        formGfx.Restore();

        //        // Privkey
        //        // QR
        //        Bitmap bmpPrivkey = BtcAddress.QR.EncodeQRCode(privkey);
        //        XImage imgPrivkey = XImage.FromGdiPlusImage(bmpPrivkey);
        //        imgPrivkey.Interpolate = false;

        //        XUnit privkeyQrLeft = XUnit.FromMillimeter(73.0);
        //        XUnit privkeyQrTop = XUnit.FromMillimeter(3.75);
        //        XUnit privkeyQrSize = XUnit.FromMillimeter(32.85);

        //        XRect privkeyQrRect = new XRect(privkeyQrLeft.Point, privkeyQrTop.Point, privkeyQrSize.Point, privkeyQrSize.Point);


        //        formGfx.DrawImage(imgPrivkey, privkeyQrRect);

        //        // legible
        //        string privkeySplitForLines = lineSplitter(privkey, 17);

        //        XFont fontPrivkey = new XFont("Courier New", 9.5, XFontStyle.Regular);

        //        XUnit privkeyTxtLeft = XUnit.FromMillimeter(108.47);
        //        XUnit privkeyTxtTop = XUnit.FromMillimeter(2.15);
        //        XUnit privkeyTxtWidth = XUnit.FromMillimeter(18.95);
        //        XUnit privkeyTxtHeight = XUnit.FromMillimeter(36.17);



        //        TextRotation privkeyTxtRotation = TextRotation.CounterClockwise;
        //        double privkeyTxtRotationDegrees = RotationMarkerToDegrees(privkeyTxtRotation);

        //        XRect privkeyRect = new XRect(privkeyTxtLeft.Point, privkeyTxtTop.Point, privkeyTxtWidth.Point, privkeyTxtHeight.Point);

        //        if (layoutDebugging)
        //        {
        //            // draw a tracer rectangle for the original un-rotated text rectangle
        //            formGfx.DrawRectangle(XBrushes.PowderBlue, privkeyRect);
        //        }

        //        // rotate that lovely text around its middle when drawing!
        //        rotateCentre = new XPoint(privkeyTxtLeft + (privkeyTxtWidth / 2), privkeyTxtTop + (privkeyTxtHeight / 2));
                
        //        matrixRotatePoint = new XPoint(privkeyRect.X + (privkeyRect.Width / 2), privkeyRect.Y + (privkeyRect.Height / 2));

        //        rotateMatrix = new XMatrix();
        //        rotateMatrix.RotateAtAppend(privkeyTxtRotationDegrees, rotateCentre);
        //        privkeyRect.Transform(rotateMatrix);

        //        if (layoutDebugging)
        //        {
        //            // draw a little tracer dot for where the centre of rotation is going to be
        //            double rotateDotSize = 2.0;
        //            formGfx.DrawEllipse(XBrushes.Red, rotateCentre.X - (rotateDotSize / 2), rotateCentre.Y - (rotateDotSize / 2), rotateDotSize, rotateDotSize);
        //        }

        //        formGfx.Save();

        //        formGfx.RotateAtTransform(privkeyTxtRotationDegrees, rotateCentre);

        //        if (layoutDebugging)
        //        {
        //            formGfx.DrawRectangle(XPens.OrangeRed, privkeyRect);
        //        }

        //        //tf.DrawString(privkeySplitForLines, fontPrivkey, XBrushes.Gray, privkeyRect);
        //        tf.DrawString(privkeySplitForLines, fontPrivkey, XBrushes.Red, privkeyRect);

        //        formGfx.Restore();
        //    }



        //    return form;
        //}

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
    }
}
