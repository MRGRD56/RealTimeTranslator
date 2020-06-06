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

namespace RealTimeTranslator.Windows
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private TranslatorWindow TWindow { get; }
		public List<string> Langs { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			TWindow = new TranslatorWindow(this);
			TWindow.Show();
			DataContext = this;
			SetLangs();
			//this.Hide();
		}

		private void SetLangs()
		{
			var files = System.IO.Directory.GetFiles(App.AppSettings.TrainedDataFolderPath);
			Langs = files.ToList().Select(x =>
			{
				var name = new System.IO.FileInfo(x).Name;
				return name.Split(new char[] { '.' })[0];
			}).ToList();
		}

		private void LangsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			App.AppSettings.Lang3In = (string)LangsCB.SelectedItem;
		}

		private void ThresholdSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			App.AppSettings.Threshold = ThresholdSlider.Value;
		}

		private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			App.AppSettings.FontSize = e.NewValue;
			if (TWindow != null)
				TWindow.TranslatedTextTB.FontSize = e.NewValue;
		}

		private void IsAsItsCB_CheckedUnckecked(object sender, RoutedEventArgs e)
		{
			App.AppSettings.IsAsIts = IsAsItsCB.IsChecked == true ? true : false;
		}
	}
}
