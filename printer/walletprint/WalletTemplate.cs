using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using PdfSharp.Drawing;

namespace DogeAddress.walletprint
{
    public enum TextRotation
    {
        Normal,
        Clockwise,
        CounterClockwise,
        UpsideDown
    }
    
    public enum LimitedColourPallete
    {
        Black,
        White,
        Red,
        Green,
        Blue,
        Grey,
        Yellow,
        Orange,
        Pink,
        Brown,
        LightGrey,
        DarkGrey
    }

    public enum AddressPrintVariant
    {
        FullAddress,
        OneLine,
        TimestampInstead
    }

    /// <summary>
    /// Configuration for a Print Wallet Template
    /// </summary>
    public class WalletTemplate
    {
        // core config members
        public bool LayoutDebugging = false;

        public string TemplateDescription = "default built-in template";

        public string CoinName = "Dogecoin";
        public byte CoinAddressType = 30;
        public bool CoinIsWIFstupid = false;

        // size of Printed Wallet item, millimetres
        public double widthMM = 188;
        public double heightMM = 40;

        // ---- Wallet Address / reference area (public)
        // top-left of Address QR, millimetres
        public double addressQrLeftMM = 35.15;
        public double addressQrTopMM = 6.95;

        // size of Address QR, millimetres - it's going to be a square
        public double addressQrSizeMM = 26.44;


        // top-left of Address Text area, millimetres
        public double addressTextLeftMM = 4.16;
        public double addressTextTopMM = 20.95;

        // size of Address Text area, millimetres - not compelled to be square
        public double addressTextWidthMM = 27.65;
        public double addressTextHeightMM = 12.36;

        public TextRotation addressTextRotation = TextRotation.Normal;
        public int addressTextCharsPerLine = 12;
        public string addressTextFontName = "Courier New";
        public double addressTextFontSize = 10;
        public XFontStyle addressTextFontStyle = XFontStyle.Regular;
        public LimitedColourPallete addressTextColour = LimitedColourPallete.Black;

        public AddressPrintVariant addressTextContentVariant = AddressPrintVariant.FullAddress;

        // ---- Private Key area
        // top-left of privkey QR, millimetres
        public double privkeyQrLeftMM = 73.0;
        public double privkeyQrTopMM = 3.75;

        // size of privkey QR, millimetres - it's going to be a square
        public double privkeyQrSizeMM = 32.85;


        // top-left of privkey Text area, millimetres
        public double privkeyTextLeftMM = 108.47;
        public double privkeyTextTopMM = 2.15;

        // size of privkey Text area, millimetres - not compelled to be square
        public double privkeyTextWidthMM = 18.95;
        public double privkeyTextHeightMM = 36.17;

        public TextRotation privkeyTextRotation = TextRotation.CounterClockwise;
        public int privkeyTextCharsPerLine = 17;
        public string privkeyTextFontName = "Courier New";
        public double privkeyTextFontSize = 9.5;
        public XFontStyle privkeyTextFontStyle = XFontStyle.Regular;
        public LimitedColourPallete privkeyTextColour = LimitedColourPallete.Grey;

        // printed-page setup
        public PdfSharp.PageSize pagePrintPaperSize = PdfSharp.PageSize.A4;

        public int pagePrintCols = 1; // number of columns Across (X-axis increment) to lay out on page
        public int pagePrintRows = 6; // number of rows Down (Y-axis increment) to lay out on page

        public double pagePrintLeftMarginMM = 10.0; // distance from LEFT of page to start of first print item
        public double pagePrintTopMarginMM = 12.0; // distance from TOP of page to start of first print item

        public double pagePrintColGap = 5.0; // gap between horizontal COLUMNS of print items 
        public double pagePrintRowGap = 6.0; // gap between vertical ROWS of print items


        #region Text Colour brush-getters

        private XBrush GetBrushFromMyPallete(LimitedColourPallete pallete)
        {
            switch(pallete){
                case LimitedColourPallete.Black:
                    return XBrushes.Black;
                case LimitedColourPallete.Blue:
                    return XBrushes.Blue;
                case LimitedColourPallete.Green:
                    return XBrushes.Green;
                case LimitedColourPallete.Grey:
                    return XBrushes.Gray;
                case LimitedColourPallete.Red:
                    return XBrushes.Red;
                case LimitedColourPallete.White:
                    return XBrushes.White;
                case LimitedColourPallete.Yellow:
                    return XBrushes.Yellow;
                case LimitedColourPallete.Orange:
                    return XBrushes.Orange;
                case LimitedColourPallete.Pink:
                    return XBrushes.Pink;
                case LimitedColourPallete.Brown:
                    return XBrushes.Brown;
                case LimitedColourPallete.LightGrey:
                    return XBrushes.LightGray;
                case LimitedColourPallete.DarkGrey:
                    return XBrushes.DarkGray;
                default:
                    return XBrushes.Black;
            }
        }

        public XBrush GetBrushAddress
        {
            get
            { return GetBrushFromMyPallete(addressTextColour); }
        }

        public XBrush GetBrushPrivkey
        {
            get { return GetBrushFromMyPallete(privkeyTextColour); }
        }

        #endregion


        #region save and load methods (greatly assisted by http://stackoverflow.com/questions/4123590/serialize-an-object-to-xml)

        public void SaveToStream(System.IO.Stream sOut)
        {
            var serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(sOut, this);
            sOut.Flush();
        }

        public void SaveToStreamWriter(System.IO.StreamWriter swOut)
        {
            var serializer = new XmlSerializer(this.GetType());
            serializer.Serialize(swOut, this);
            swOut.Flush();           
        }

        /// <summary>
        /// Saves to an xml file
        /// </summary>
        /// <param name="FileName">File path of the new xml file</param>
        public void SaveToFile(string FileName)
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                this.SaveToStreamWriter(writer);
            }
        }

        public static WalletTemplate LoadFromStream(System.IO.Stream streamIn)
        {
            var serializer = new XmlSerializer(typeof(WalletTemplate));
            return serializer.Deserialize(streamIn) as WalletTemplate;
        }

        /// <summary>
        /// Load an object from an xml file
        /// </summary>
        /// <param name="FileName">Xml file name</param>
        /// <returns>The object created from the xml file</returns>
        public static WalletTemplate LoadFromFile(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                return LoadFromStream(stream);
            }
        }

        

        #endregion
    }

}
