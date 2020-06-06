using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RealTimeTranslator
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static AppSettings AppSettings { get; set; }
		public App()
		{
			AppSettings = new AppSettings
			{
				TrainedDataFolderPath = @"C:\RTT_Data\TrainedData",
				ImgPath = @"C:\RTT_Data\Temp\img.bmp",
				Lang3In = "eng",
				Lang2Out = "ru",
				TempFolder = @"C:\RTT_Data\Temp\",
				Threshold = 100,
				FontSize = 13,
				IsAsIts = false
			};
		}
	}
}
