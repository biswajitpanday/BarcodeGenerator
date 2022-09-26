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

            var barcode = new Barcode
            {
                Text = text_input.Text
            };

            try
            {
                var imagePath = GenerateBarcode_Sprite_Barcode_Package(barcode);
                var expireDate = new DateTime(2022, 10, 01).Ticks;
                if(DateTime.UtcNow.Ticks < expireDate)
                    PreviewBarcode(imagePath);
                else
                {
                    MessageBox.Show(
                        "Product is expired. Please Contact with developer. Email: biswajitmailid@gmail.com");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(ex.Message);
            }
            
        }

        #region Private Methods
        private static string StoreFilePath(Barcode barcode)
        {
            var folder = Directory.GetCurrentDirectory() + "/outputs";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var filePath = $"{folder}/{barcode.Text}.png";
            return filePath;
        }

        private string GenerateBarcode_Sprite_Barcode_Package(Barcode barcode)
        {
            var bs = new BarcodeSettings
            {
                Type = BarCodeType.Code39,
                Data = barcode.Text
            };

            var bg = new BarCodeGenerator(bs);
            
            var filePathWithFileName = StoreFilePath(barcode);
            if (File.Exists(filePathWithFileName))
                throw new Exception("A file with same name already exists!");
            bg.GenerateImage().Save(filePathWithFileName);
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
