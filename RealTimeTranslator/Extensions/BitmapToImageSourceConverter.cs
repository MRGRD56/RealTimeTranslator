using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RealTimeTranslator.Extensions
{
	
	public class BitmapToImageConverter
	{
		public static BitmapImage GetBitmapImage(Bitmap bitmap)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				bitmap.Save(ms, ImageFormat.Png);
				ms.Position = 0;
				BitmapImage bi = new BitmapImage();
				bi.BeginInit();
				bi.StreamSource = ms;
				bi.EndInit();

				return bi;
			}
		}
	}
}
