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
using System.Windows.Shapes;
using static RealTimeTranslator.Properties.Settings;

namespace RealTimeTranslator.Windows
{
	/// <summary>
	/// Логика взаимодействия для TranslatedTextWindow.xaml
	/// </summary>
	public partial class TranslatedTextWindow : Window
	{
		private MainWindow MW { get; }

		public TranslatedTextWindow(MainWindow mw)
		{
			InitializeComponent();
			MW = mw;

			TranslatedTextTB.FontSize = Default.OutFontSize;
			TranslatedTextSV.ScrollToEnd();
		}

		private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}
	}
}
