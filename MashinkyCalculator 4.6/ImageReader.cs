using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace MashinkyCalculator
{
    class ImageReader
    {
        private Bitmap wagonsSet;
        private Bitmap vehicleSet;
        private BitmapImage blankImage;
        public ImageReader(BitmapImage blankImage)
        {
            this.blankImage = blankImage;
            ReadVanillaIcons();
        }
        public void ReadVanillaIcons()
        {
           // File.AppendAllText("trace.txt", "\nLoading " + Settings.GameFolderPath + "\\media\\map\\gui\\wagons_basic_set.png");
            wagonsSet = Image.FromFile(Settings.GameFolderPath + "\\media\\map\\gui\\wagons_basic_set.png") as Bitmap;
           // File.AppendAllText("trace.txt", "\nFile loaded!");
          //  File.AppendAllText("trace.txt", "\nLoading " + Settings.GameFolderPath + "\\media\\map\\gui\\cars_basic_set.png");
            vehicleSet = Image.FromFile(Settings.GameFolderPath + "\\media\\map\\gui\\cars_basic_set.png") as Bitmap;
          //  File.AppendAllText("trace.txt", "\nFile loaded!");
        }
        /// <summary>
        /// Crops image
        /// </summary>
        /// <param name="iconSource"></param>
        /// <param name="coords">X, Y, weight, height</param>
        /// <returns>Cloned Bitmap within specified coords</returns>
        public BitmapImage ReadIcon(string iconSource, int[] coords)
        {
            Bitmap source;
            if (iconSource.Contains("/map/gui/wagons_basic_set.png"))
                source = wagonsSet;
            else if (iconSource.Contains("/media/map/gui/cars_basic_set.png"))
                source = vehicleSet;
            else
                source = Image.FromFile(iconSource) as Bitmap;
            Rectangle cloneArea = new Rectangle(coords[0], coords[1], coords[2], coords[3]);
          //  source.Save("source.bmp");
            Bitmap bmp = source.Clone(cloneArea, PixelFormat.Format32bppArgb);
            // bmp.Save("test.bmp");
           // bmp.MakeTransparent(Color.Black);
           // bmp.MakeTransparent(Color.White);

            return ConvertBitmapToBitmapImage(bmp);

        }

        public BitmapImage ConvertBitmapToBitmapImage(Bitmap bmp)
        {
            BitmapImage bmpImage = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                bmpImage.BeginInit();
                bmpImage.StreamSource = ms;
                bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                bmpImage.EndInit();
            }

            return bmpImage;
        }
    }
}
