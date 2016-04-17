using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.IO.Compression;

namespace DogeAddress.walletprint
{
    public class WalletBundle
    {
        public WalletBundle()
        {
            // boring empty constructor
        }

        public WalletBundle(string filepath)
        {
            LoadBundle(filepath);
        }

        #region public members
        public WalletTemplate template;
        public System.IO.MemoryStream artwork;
        public string artworkFileExten;
        #endregion

        #region general methods
        public System.Drawing.Image getArtworkImage()
        {
            if (artwork != null)
            {
                return System.Drawing.Image.FromStream(artwork);
            }
            else
            {
                return null;
            }
        }
        
        // load Artwork in from a disk file
        public void importArtwork(string filepath)
        {
            artwork = new MemoryStream();
            

            using (FileStream fsInput = new FileStream(filepath, FileMode.Open))
            {
                fsInput.CopyTo(artwork);
            }

            artworkFileExten = Path.GetExtension(filepath).Replace(".", "");
        }

        // export Artwork out to a disk file
        public void exportArtwork(string filepath)
        {
            if (artwork == null)
            {
                throw new InvalidOperationException("Cannot Export artwork out of Bundle, no Artwork present");
            }

            using (FileStream fsOutput = new FileStream(filepath, FileMode.Create))
            {
                artwork.Seek(0, SeekOrigin.Begin);
                artwork.CopyTo(fsOutput);
            }
        }
        
        #endregion


        #region File methods
        // save bundle to zip (overwriting any existing file)
        public void SaveBundle(string filepath)
        {
            // Bundle must contain both Template and Artwork to be saved out to disk

            if (template == null)
            {
                throw new InvalidOperationException("Cannot Save Bundle to disk - no Template is present");
            }

            if (artwork == null)
            {
                throw new InvalidOperationException("Cannot Save Bundle to disk - no Artwork is present");
            }

            // if file already exists, just delete it. far easier.
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            using (ZipArchive outZip = ZipFile.Open(filepath, ZipArchiveMode.Create))
            {
                ZipArchiveEntry entryTemplate = outZip.CreateEntry("template.xml", CompressionLevel.Optimal);
                using (Stream sTemplate = entryTemplate.Open())
                {
                    template.SaveToStream(sTemplate);
                }

                string artworkFileName = "artwork." + artworkFileExten;
                ZipArchiveEntry entryArtwork = outZip.CreateEntry(artworkFileName, CompressionLevel.Fastest);
                using (Stream sArtwork = entryArtwork.Open())
                {
                    artwork.Seek(0, SeekOrigin.Begin);
                    artwork.CopyTo(sArtwork);
                }

            }
            
        }

        // load bundle from zip
        public void LoadBundle(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new InvalidOperationException("Bundle file does not exist!");
            }

            using(ZipArchive inZip = ZipFile.Open(filepath, ZipArchiveMode.Read))
            {
                ZipArchiveEntry entryTemplate = inZip.GetEntry("template.xml");

                if (entryTemplate == null)
                {
                    throw new InvalidDataException("Bundle file does not contain a Template file!");
                }

                using (Stream sTemplate = entryTemplate.Open())
                {
                    template = WalletTemplate.LoadFromStream(sTemplate);
                }

                ZipArchiveEntry entryArtwork = inZip.Entries.Where(e => e.Name.StartsWith("artwork.")).FirstOrDefault();

                if (entryArtwork == null)
                {
                    throw new InvalidDataException("Bundle file does not contain an Artwork file!");
                }

                artworkFileExten = Path.GetExtension(entryArtwork.Name).Replace(".", "");

                using (Stream sArtwork = entryArtwork.Open())
                {
                    artwork = new MemoryStream();
                    sArtwork.CopyTo(artwork);
                }

            }
        }
        
        #endregion



    }
}
