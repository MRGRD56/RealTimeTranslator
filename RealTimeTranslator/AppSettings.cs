using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeTranslator
{
	public class AppSettings
	{
		public string Lang3In { get; set; }
		public string Lang2Out { get; set; }
		public string ImgPath { get; set; }
		public string TempFolder { get; set; }
		public string TrainedDataFolderPath { get; set; }
		public double Threshold { get; set; }
		public double FontSize { get; set; }
		public bool IsAsIts { get; set; }
	}
}
