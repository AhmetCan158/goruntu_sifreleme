using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ImageEncryption
{
    public partial class MainWindow : Window
    {
        private BitmapImage orijinalImage;
        private WriteableBitmap sifrelenmisImage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private Color GetPixelColor(WriteableBitmap bitmap, int x, int y)
        {
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            int stride = bitmap.PixelWidth * bytesPerPixel;

            byte[] pixelData = new byte[bytesPerPixel];
            bitmap.CopyPixels(new Int32Rect(x, y, 1, 1), pixelData, stride, 0);

            Color color = Color.FromArgb(pixelData[3], pixelData[2], pixelData[1], pixelData[0]);
            return color;
        }

        private void SetPixelColor(WriteableBitmap bitmap, int x, int y, Color color)
        {
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            int stride = bitmap.PixelWidth * bytesPerPixel;

            byte[] pixelData = new byte[bytesPerPixel];
            pixelData[0] = color.B;
            pixelData[1] = color.G;
            pixelData[2] = color.R;
            pixelData[3] = color.A;

            bitmap.WritePixels(new Int32Rect(x, y, 1, 1), pixelData, stride, 0);
        }

        private void LoadImageButton(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;)|*.png;*.jpg;*.jpeg;";

            if (openFileDialog.ShowDialog() == true)
            {
                orijinalImage = new BitmapImage(new Uri(openFileDialog.FileName));
                imageControl.Source = orijinalImage;
            }
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (orijinalImage != null)
            {
                sifrelenmisImage = new WriteableBitmap(orijinalImage);
                Random rastgele = new Random();

                int width = sifrelenmisImage.PixelWidth;
                int height = sifrelenmisImage.PixelHeight;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int newX = rastgele.Next(width);
                        int newY = rastgele.Next(height);

                        Color color = GetPixelColor(sifrelenmisImage, x, y);
                        Color newColor = GetPixelColor(sifrelenmisImage, newX, newY);

                        SetPixelColor(sifrelenmisImage, x, y, newColor);
                        SetPixelColor(sifrelenmisImage, newX, newY, color);
                    }
                }

                imageControl.Source = sifrelenmisImage;
            }
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (sifrelenmisImage != null)
            {
                imageControl.Source = orijinalImage;
            }
        }
    }
}
