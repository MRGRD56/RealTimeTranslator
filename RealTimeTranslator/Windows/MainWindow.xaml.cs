using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static RealTimeTranslator.Properties.Settings;

namespace RealTimeTranslator.Windows
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private TranslatorWindow TWindow { get; }
		public TranslatedTextWindow TTWindow { get; }
		public List<string> Langs { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			TWindow = new TranslatorWindow(this);
			TWindow.Show();
			TTWindow = new TranslatedTextWindow(this);
			TTWindow.Show();
			DataContext = this;
			SetLangs();
			//this.Hide();

			//Langs3InCB Langs2OutCB ThresholdSlider FontSizeSlider
			Langs3InCB.SelectedItem = Default.Lang3In;
			//Langs2OutCB.SelectedItem = Langs2OutCB.Items
			//	.Cast<ComboBoxItem>()
			//	.Where(x => x.Content.ToString() == Default.Lang2Out)
			//	.FirstOrDefault();
			Lang2OutTB.Text = Default.Lang2Out;
			ThresholdSlider.Value = Default.Threshold;
			IsAsItsCB.IsChecked = Default.IsAsIts;
			FontSizeSlider.Value = Default.OutFontSize;

			//UpdateLastImage();
		}

		private void SetLangs()
		{
			var files = System.IO.Directory.GetFiles(Default.RttDataDirectory + Default.TrainedDataFolder);
			Langs = files.ToList().Select(x =>
			{
				var name = new System.IO.FileInfo(x).Name;
				return name.Split(new char[] { '.' })[0];
			}).ToList();
		}

		private void Langs3InComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Default.Lang3In = (string)Langs3InCB.SelectedItem;
			Default.Save();
		}

		private void ThresholdSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Default.Threshold = ThresholdSlider.Value;
			Default.Save();
		}

		private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			var val = FontSizeSlider.Value;
			if (TWindow != null && this != null)
			{
				Default.OutFontSize = val;
				Default.Save();
				//TODO!!!
				TTWindow.TranslatedTextTB.FontSize = val;
			}
		}

		private void IsAsItsCB_CheckedUnckecked(object sender, RoutedEventArgs e)
		{
			Default.IsAsIts = IsAsItsCB.IsChecked == true ? true : false;
			Default.Save();
		}

		private void Lang2OutTB_TextChanged(object sender, TextChangedEventArgs e)
		{
			var lang = Lang2OutTB.Text;
			Default.Lang2Out = lang;
			Default.Save();
		}

		//public void UpdateLastImage()
		//{
		//	BitmapImage img = new BitmapImage();
		//	img.BeginInit();
		//	img.UriSource = new Uri(Default.RttDataDirectory + Default.TempFolder + "threshold.bmp");
		//	img.EndInit(); // Getting the exception here
		//	LastImage.Source = img;
		//}
	}
}
