using RealTimeTranslator.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Emgu;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.Util;
using RealTimeTranslator.Windows;

namespace RealTimeTranslator
{
	/// <summary>
	/// Логика взаимодействия для TranslatorWindow.xaml
	/// </summary>
	public partial class TranslatorWindow : Window
	{
		private MainWindow MW { get; }
		public string Lang { get; set; } = "eng";
		private const string ImgPath = @"C:\RTT_Data\Temp\img.jpg";
		public const string TrainedDataFolderPath = @"C:\RTT_Data\TrainedData";
		private int UpMargin { get; set; } = 20;

		public TranslatorWindow(MainWindow mw)
		{
			InitializeComponent();
			//this.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 34, 34, 34));
			MW = mw;
		}

		private void TranslateButton_Click(object sender, RoutedEventArgs e) => Translate();

		private void Translate()
		{
			var img = GetScreenShot(this.Left, this.Top, this.Width, this.Height);
			//DEBUG
			img.Save(ImgPath, ImageFormat.Jpeg);
			//MW.MainImage.Source = BitmapToImageConverter.GetBitmapImage(img);
			var text = GetTextFromImage(ImgPath);
			//MessageBox.Show(text);
			ShowText(text);
		}

		private Bitmap GetScreenShot(double posX, double posY, double width, double height)
		{
			int posXInt = Convert.ToInt32(posX);
			int posYInt = Convert.ToInt32(posY) + UpMargin;
			int widthInt = Convert.ToInt32(width);
			int heightInt = Convert.ToInt32(height) - UpMargin;
			this.Hide();
			// 100, 100 размер копируемой области
			Bitmap screen = new Bitmap(widthInt, heightInt);
			using (Graphics g = Graphics.FromImage(screen))
			{
				// 5, 5 - координаты левого верхнего угла копируемой области
				g.CopyFromScreen(posXInt, posYInt, 00, 0, screen.Size);
				//pictureBox1.Image = screen;
			}
			this.Show();
			return screen;
		}

		private string GetTextFromImage(string path)
		{
			Tesseract tesseract = new Tesseract(TrainedDataFolderPath, Lang, OcrEngineMode.TesseractLstmCombined);
			tesseract.SetImage(new Image<Bgr, byte>(path));
			tesseract.Recognize();
			return tesseract.GetUTF8Text().Replace("\n\n", "\n");
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.F)
			{
				Translate();
			}
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void HidePanelButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("В разработке");
		}

		private void TransparentBackgroundButton_Click(object sender, RoutedEventArgs e)
		{
			TranslatedTextSV.Visibility = Visibility.Hidden;
		}

		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void MenuBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void ShowText(string text)
		{
			TranslatedTextSV.Visibility = Visibility.Visible;
			TranslatedTextTB.Text = text;
		}
	}
}
