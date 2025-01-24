using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Spire.Barcode;

namespace BarcodeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(text_input.Text))
            {
                MessageBox.Show("Please enter text to generate a barcode/QR code.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var barcode = new Barcode
            {
                Text = text_input.Text
            };

            try
            {
                var isQrCode = rbQRCode.IsChecked == true;
                var imagePath = GenerateBarcode(barcode, isQrCode);
                PreviewBarcode(imagePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(ex.Message);
            }

        }

        #region Private Methods
        private static string StoreFilePath(Barcode barcode, bool isQrCode)
        {
            var folder = Directory.GetCurrentDirectory() + "/outputs";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileName = isQrCode ? $"{barcode.Text}_QRCode.png" : $"{barcode.Text}_Barcode.png";
            var filePath = $"{folder}/{fileName}.png";
            return filePath;
        }

        private string GenerateBarcode(Barcode barcode, bool isQrCode)
        {
            var barCodeSettings = new BarcodeSettings
            {
                Type = isQrCode ? BarCodeType.QRCode : BarCodeType.Code39,
                Data = barcode.Text,
                ShowText = !isQrCode
            };

            var barCodeGenerator = new BarCodeGenerator(barCodeSettings);

            var filePathWithFileName = StoreFilePath(barcode, isQrCode);
            if (File.Exists(filePathWithFileName))
                throw new Exception("A file with same name already exists!");
            barCodeGenerator.GenerateImage().Save(filePathWithFileName);
            return filePathWithFileName;
        }

        private void PreviewBarcode(string imagePath)
        {
            var fileUri = new Uri(Path.GetFullPath(imagePath));
            imgDynamic.Source = new BitmapImage(fileUri);
        }

        #endregion
    }
}
