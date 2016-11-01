using System.Drawing;
using System.IO;
using System.Windows;

namespace UserSearch.Utils
{
    public class Utils
    {
        public static int PHOTO_SIZE = 100;

        public static byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static void showInfoDialog(string message)
        {
            MessageBox.Show(message, "User Search", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void showErrorDialog(string message)
        {
            MessageBox.Show(message, "User Search", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public static MessageBoxResult showYNDialog(string message)
        {
            return MessageBox.Show(message, "User Search", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
