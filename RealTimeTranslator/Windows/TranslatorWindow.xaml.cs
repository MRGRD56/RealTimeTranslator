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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Effects;
using Path = System.IO.Path;
using System.Threading;
using RealTimeTranslator.Utils;
using Rectangle = System.Drawing.Rectangle;

namespace RealTimeTranslator
{
    /// <summary>
    /// Логика взаимодействия для TranslatorWindow.xaml
    /// </summary>
    public partial class TranslatorWindow : Window
    {
        private MainWindow MW { get; }
        private int UpMargin { get; set; } = 20;
        private string ImgFolder { get; } = Path.Combine(Default.RttDataDirectory, Default.TempFolder);
        private string ImgPath => Path.Combine(ImgFolder, Default.ImgName);
        private string TempFolderPath { get; } = Default.RttDataDirectory + Default.TempFolder;
        private GlobalKeyboardHook gkhTranslate { get; set; } = new GlobalKeyboardHook();
        private GlobalKeyboardHook gkhRecognizeOnly { get; set; } = new GlobalKeyboardHook();

        private readonly ValueRef<bool> _isAutoMode = new ValueRef<bool>(false);

        private bool IsAutoMode
        {
            get => _isAutoMode.Value;
            set
            {
                _isAutoMode.Value = value;
                lock (_isAutoMode)
                {
                    Monitor.PulseAll(_isAutoMode);
                }
            }
        }

        public TranslatorWindow(MainWindow mw)
        {
            InitializeComponent();
            MW = mw;
            MW.WindowState = WindowState.Minimized;

            WorkAutoMode();

            GlobalKeyHooksRegister();
        }

        private void GlobalKeyHooksRegister()
        {
            gkhTranslate.HookedKeys.Add(System.Windows.Forms.Keys.Oemtilde);
            gkhTranslate.HookedKeys.Add(System.Windows.Forms.Keys.F2);
            gkhTranslate.KeyDown += new System.Windows.Forms.KeyEventHandler(GkhTranslate_KeyDown); //new System.Windows.Forms.KeyEventHandler
            gkhRecognizeOnly.HookedKeys.Add(System.Windows.Forms.Keys.F3);
            gkhRecognizeOnly.KeyDown += new System.Windows.Forms.KeyEventHandler(GkhRecognizeOnly_KeyDown);
        }

        private void GkhTranslate_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            TranslateAsync();
            e.Handled = true;
        }

        private void GkhRecognizeOnly_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            RecognizeOnly();
            e.Handled = true;
        }

        private void TranslateButton_Click(object sender, RoutedEventArgs e) => TranslateAsync();

        private async void TranslateAsync()
        {
            var text = GetTextFromScreen().Replace(Environment.NewLine, " ");
            var translated = await Translator.TranslateTextViaGoogleAsync(text, Translator.ConvertLang3To2(Default.Lang3In), Default.Lang2Out);
            AddTextToTTWindow(text, translated);
        }

        private Bitmap GetTextScreenshot(bool doHide = true)
        {
            if (!Directory.Exists(ImgFolder))
            {
                Directory.CreateDirectory(ImgFolder);
            }

            Vector? position = null;
            Vector? size = null;

            Dispatcher.Invoke(() =>
            {
                var source = PresentationSource.FromVisual(this);
                var transformToDevice = source.CompositionTarget.TransformToDevice;
                position = transformToDevice.Transform(new Vector(Left, Top));
                size = transformToDevice.Transform(new Vector(Width, Height));
            });

            var img = GetScreenshot(position.Value.X, position.Value.Y, size.Value.X, size.Value.Y, doHide);
            img.Save(ImgPath, ImageFormat.Bmp);

            return img;
        }

        private string GetTextFromScreen(bool doHide = true)
        {
            var screenshot = GetTextScreenshot(doHide);

            string result = null;

            Dispatcher.Invoke(() => 
            {
                result = GetTextFromImage(ImgPath);
            });

            return result;
        }

        private Bitmap GetScreenshot(double posX, double posY, double width, double height, bool doHide = true)
        {
            int posXInt = Convert.ToInt32(posX);
            int posYInt = Convert.ToInt32(posY) + UpMargin;
            int widthInt = Convert.ToInt32(width);
            int heightInt = Convert.ToInt32(height) - UpMargin;
            if (doHide)
            {
                this.Hide();
            }
            // 100, 100 размер копируемой области
            Bitmap screen = new Bitmap(widthInt, heightInt);
            using (Graphics g = Graphics.FromImage(screen))
            {
                // 5, 5 - координаты левого верхнего угла копируемой области
                g.CopyFromScreen(posXInt, posYInt, 00, 0, screen.Size);
                //pictureBox1.Image = screen;
            }
            if (doHide)
            {
                this.Show();
            }
            return screen;
        }

        private string GetTextFromImage(string path)
        {
            Tesseract tesseract = new Tesseract(Default.RttDataDirectory + Default.TrainedDataFolder, Default.Lang3In, OcrEngineMode.TesseractLstmCombined);
            //var img = new Image<Rgb, byte>(path);
            Mat mat = new Mat(path);

            if (Default.IsAsIs)
            {
                tesseract.SetImage(mat);
                tesseract.Recognize();
                return tesseract.GetUTF8Text().Replace("|", "I");
            }

            Mat dstMat = mat;
            //VectorOfMat matChannels = new VectorOfMat();
            ////Mat mat1 = mat;
            ////Mat mat2 = mat;
            ////var mat = new Mat(path);
            ////mat.Save(App.AppSettings.TempFolder + "mat.bmp");

            ////преобразование цветового пространства (например, из цветного в градации серого)
            //CvInvoke.CvtColor(mat, dstMat, ColorConversion.Rgb2Gray);
            //dstMat.Save(TempFolderPath + "cvtcolor.bmp");

            ////разделение изображения на отдельные цветовые каналы
            //CvInvoke.Split(mat, matChannels);

            ////сборка многоцветного изображения из отдельных каналов
            //CvInvoke.Merge(matChannels, mat);
            //mat.Save(TempFolderPath + "merge.bmp");

            ////разница между двумя изображениями
            //CvInvoke.AbsDiff(mat, dstMat, dstMat);
            //dstMat.Save(App.AppSettings.TempFolder + "diff.bmp");

            CvInvoke.CvtColor(mat, mat, ColorConversion.Rgb2Gray);
            mat.Save(TempFolderPath + "gray.bmp");

            //приведение точек, которые темнее/светлее определенного уровня(50) к черному(0) или белому цвету(255)
            CvInvoke.Threshold(mat, dstMat, MW.ThresholdSlider.Value, 255, ThresholdType.Binary);
            dstMat.Save(TempFolderPath + "threshold.bmp");

            //CvInvoke.AdaptiveThreshold(dstMat, dstMat, 140, AdaptiveThresholdType.MeanC, ThresholdType.Binary, 3, -30);
            //dstMat.Save(App.AppSettings.TempFolder + "threshold_ad.bmp");

            tesseract.SetImage(dstMat);
            tesseract.Recognize();
            //return tesseract.GetUTF8Text().Replace("\n\n", "\n");
            //КОСТЫЛЬ
            var text = tesseract.GetUTF8Text().Replace("|", "I");
            return text;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F)
            {
                TranslateAsync();
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

        private void MenuBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                TranslateAsync();
            else
                DragMove();
        }

        private void AddTextToTTWindow(string origText, string translatedText)
        {
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

        private void RecognizeOnlyButton_Click(object sender, RoutedEventArgs e) => RecognizeOnly();

        private void RecognizeOnly()
        {
            var text = GetTextFromScreen();
            AddTextToTTWindow("[recongition only]", text);
        }

        private void LoadImgButton_Click(object sender, RoutedEventArgs e)
        {
            var text = GetTextFromImage(ImgPath);
            AddTextToTTWindow("[recongition only]", text);
        }

        private void AutoModeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            IsAutoMode = !IsAutoMode;
            if (IsAutoMode)
            {
                button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0xf4, 0x43, 0x36));
                button.FontWeight = FontWeights.Bold;
            }
            else
            {
                button.ClearValue(Control.BackgroundProperty);
                button.ClearValue(Control.FontWeightProperty);
            }
        }

        private async void WorkAutoMode()
        {
            Bitmap lastScreenshot = null;
            string lastText = null;

            await Task.Run(async () =>
            {
                while (true)
                {
                    if (!IsAutoMode)
                    {
                        lock (_isAutoMode)
                        {
                            Monitor.Wait(_isAutoMode);
                        }
                        continue;
                    }

                    var screenshot = GetTextScreenshot(false);
                    var previousScreenshot = lastScreenshot;
                    lastScreenshot = screenshot;

                    if (screenshot != null) 
                    {
                        string text = null;

                        Dispatcher.Invoke(() =>
                        {
                            text = GetTextFromImage(ImgPath);
                        });

                        var previousText = lastText;
                        lastText = text;

                        if (!string.IsNullOrEmpty(text) && text != previousText)
                        {
                            var translated = await Translator.TranslateTextViaGoogleAsync(text, Translator.ConvertLang3To2(Default.Lang3In), Default.Lang2Out);
                            Dispatcher.Invoke(() => AddTextToTTWindow(text, translated));
                        }
                    }

                    await Task.Delay(800);
                }
            });
        }

        //[DllImport("msvcrt.dll")]
        //private static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        public static bool CompareBitmaps(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null)
            {
                return bmp2 == null;
            }
            else if (bmp2 == null)
            {
                return false;
            }

            Rectangle rect = new Rectangle(0, 0, bmp1.Width, bmp1.Height);
            BitmapData bmpData1 = bmp1.LockBits(rect, ImageLockMode.ReadOnly, bmp1.PixelFormat);
            BitmapData bmpData2 = bmp2.LockBits(rect, ImageLockMode.ReadOnly, bmp2.PixelFormat);
            unsafe
            {
                byte* ptr1 = (byte*)bmpData1.Scan0.ToPointer();
                byte* ptr2 = (byte*)bmpData2.Scan0.ToPointer();
                int width = rect.Width * 3; // for 24bpp pixel data
                for (int y = 0; y < rect.Height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (*ptr1 != *ptr2)
                        {
                            return false;
                        }
                        ptr1++;
                        ptr2++;
                    }
                    ptr1 += bmpData1.Stride - width;
                    ptr2 += bmpData2.Stride - width;
                }
            }
            bmp1.UnlockBits(bmpData1);
            bmp2.UnlockBits(bmpData2);

            return true;
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