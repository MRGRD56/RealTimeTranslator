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
using Emgu.CV.CvEnum;
using static RealTimeTranslator.Properties.Settings;
using System.IO;

namespace RealTimeTranslator
{
	/// <summary>
	/// Логика взаимодействия для TranslatorWindow.xaml
	/// </summary>
	public partial class TranslatorWindow : Window
	{
		private MainWindow MW { get; }
		private int UpMargin { get; set; } = 20;
		private string ImgPath { get; } = Default.RttDataDirectory + Default.TempFolder + Default.ImgName;
		private string TempFolderPath { get; } = Default.RttDataDirectory + Default.TempFolder;

		public TranslatorWindow(MainWindow mw)
		{
			InitializeComponent();
			//this.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 34, 34, 34));
			MW = mw;

			//TODO!!!!
			//MW.TTWindow.TranslatedTextTB.Background = new SolidColorBrush(Default.OutBackground);
		}

		private void TranslateButton_Click(object sender, RoutedEventArgs e) => Translate();

		private void Translate()
		{
			var text = GetTextFromScreen().Replace(Environment.NewLine, " ");
			//MessageBox.Show(text);
			var translated = Translator.TranslateTextViaGoogle(text, Translator.ConvertLang3To2(Default.Lang3In), Default.Lang2Out);
			AddTextToTTWindow(text, translated);
		}

		private string GetTextFromScreen()
		{

			var img = GetScreenShot(this.Left, this.Top, this.Width, this.Height);
			img.Save(ImgPath, ImageFormat.Bmp);
			return GetTextFromImage(ImgPath);
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
			Tesseract tesseract = new Tesseract(Default.RttDataDirectory + Default.TrainedDataFolder, Default.Lang3In, OcrEngineMode.TesseractLstmCombined);
			//var img = new Image<Rgb, byte>(path);
			Mat mat = new Mat(path);

			if (Default.IsAsIts)
			{
				tesseract.SetImage(mat);
				tesseract.Recognize();
				return tesseract.GetUTF8Text();
			}

			Mat dstMat = mat;
			VectorOfMat matChannels = new VectorOfMat();
			//Mat mat1 = mat;
			//Mat mat2 = mat;
			//var mat = new Mat(path);
			//mat.Save(App.AppSettings.TempFolder + "mat.bmp");

			//преобразование цветового пространства (например, из цветного в градации серого)
			CvInvoke.CvtColor(mat, dstMat, ColorConversion.Rgb2Gray);
			dstMat.Save(TempFolderPath + "cvtcolor.bmp");

			//разделение изображения на отдельные цветовые каналы
			CvInvoke.Split(mat, matChannels);

			//сборка многоцветного изображения из отдельных каналов
			CvInvoke.Merge(matChannels, mat);
			mat.Save(TempFolderPath + "merge.bmp");

			////разница между двумя изображениями
			//CvInvoke.AbsDiff(mat, dstMat, dstMat);
			//dstMat.Save(App.AppSettings.TempFolder + "diff.bmp");

			//приведение точек, которые темнее/светлее определенного уровня(50) к черному(0) или белому цвету(255)
			CvInvoke.Threshold(mat, dstMat, MW.ThresholdSlider.Value, 255, ThresholdType.Binary);
			dstMat.Save(TempFolderPath + "threshold.bmp");

			//CvInvoke.AdaptiveThreshold(dstMat, dstMat, 140, AdaptiveThresholdType.MeanC, ThresholdType.Binary, 3, -30);
			//dstMat.Save(App.AppSettings.TempFolder + "threshold_ad.bmp");

			tesseract.SetImage(dstMat);
			tesseract.Recognize();
			//return tesseract.GetUTF8Text().Replace("\n\n", "\n");
			//КОСТЫЛЬ
			return tesseract.GetUTF8Text().Replace('|', 'I');//.Replace('\n', ' ');
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
			MW.TTWindow.Close();
			MW.Close();
			Close();
		}

		private void HidePanelButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("В разработке");
		}

		private void TransparentBackgroundButton_Click(object sender, RoutedEventArgs e)
		{
			//TODO!!! - to delete!
			//TranslatedTextSV.Visibility = Visibility.Hidden;
		}

		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void MenuBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void AddTextToTTWindow(string origText, string translatedText)
		{
			//TODO!!!
			//TranslatedTextSV.Visibility = Visibility.Visible;
			//TranslatedTextTB.Text = text;
			//<Run Text = "The quick brown fox jumps over the lazy dog." Foreground = "#EAEAEA" />
			//<LineBreak />
			//<Run Text = "Шустрая бурая лисица прыгает через ленивого пса." FontFamily = "Segoe UI Semibold" Foreground = "#FFFFFF" />
			AddInline(new LineBreak());
			AddInline(new LineBreak());
			AddInline(new Run { Text = origText, Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xEA, 0xEA, 0xEA)) });
			AddInline(new LineBreak());
			AddInline(new Run
			{
				Text = translatedText,
				Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xFF, 0xFF, 0xFF)),
				FontFamily = new System.Windows.Media.FontFamily("Segoe UI Semibold")
			});
			MW.TTWindow.TranslatedTextSV.ScrollToEnd();
		}

		private void AddInline(Inline item) => MW.TTWindow.TranslatedTextTB.Inlines.Add(item);

		private void RecognizeOnlyButton_Click(object sender, RoutedEventArgs e)
		{
			var text = GetTextFromScreen();
			AddTextToTTWindow("= recongition only =", text);
		}

		private void LoadImgButton_Click(object sender, RoutedEventArgs e)
		{
			var text = GetTextFromImage(ImgPath);
			AddTextToTTWindow("= recongition only =", text);
		}
	}
}

//Image<Gray, byte> sobel = img
//	.Sobel(1, 0, 3)
//	.AbsDiff(new Gray(0d))
//	.Convert<Gray, byte>()
//	.ThresholdBinary(new Gray(100), new Gray(255));
//Mat SE = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, 
//	new System.Drawing.Size(10, 1), new System.Drawing.Point(-1, -1));
//sobel = sobel.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Dilate, SE, new System.Drawing.Point(-1, -1), 
//	1, Emgu.CV.CvEnum.BorderType.Reflect, new MCvScalar(255));