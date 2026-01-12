using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using static BaseUtil.GcjWinApi;
using Color = System.Drawing.Color;

namespace BaseUtil
{
    public partial class CtrlUtil
    {
        public static readonly string REGISTRY_PATH = "SOFTWARE\\zdhe\\batchdownload\\1.0";
#if false
        public static Bitmap CropBitmap(Bitmap source, int left, int top, int width, int height)
        {
            Rectangle cropRect = new Rectangle(
                left,
                top,
                Math.Min(width, source.Width - left),
                Math.Min(height, source.Height - top)
            );
            if (cropRect.Width <= 0 || cropRect.Height <= 0)
            {
                throw new ArgumentException("Invalid crop dimensions.");
            }

            Bitmap croppedBitmap = new Bitmap(cropRect.Width, cropRect.Height, PixelFormat.Format32bppArgb/*source.PixelFormat*/);

            using (Graphics g = Graphics.FromImage(croppedBitmap))
            {
                //g.Clear(Color.Transparent);
                //g.FillRectangle(Brushes.Transparent, cropRect);
                g.DrawImage(source, new Rectangle(0, 0, croppedBitmap.Width, croppedBitmap.Height), cropRect, GraphicsUnit.Pixel);
            }

            return croppedBitmap;
        }

        public static Bitmap CropBitmap1(Bitmap source, int left, int top, int width, int height)
        {
            //// 定义要裁剪的矩形区域
            //Rectangle cropRect = new Rectangle(left, top, width, height);

            //// 使用 Clone 方法裁剪图像并创建新的 Bitmap
            //Bitmap croppedBitmap = source.Clone(cropRect, PixelFormat.Format24bppRgb /*PixelFormat.Format32bppArgb*/ /*source.PixelFormat*/);
            //return croppedBitmap;

            Rectangle cropRect = new Rectangle(
                left,
                top,
                Math.Min(width, source.Width - left),
                Math.Min(height, source.Height - top)
            );
            if (cropRect.Width <= 0 || cropRect.Height <= 0)
            {
                throw new ArgumentException("Invalid crop dimensions.");
            }

            Bitmap croppedBitmap = new Bitmap(cropRect.Width, cropRect.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(croppedBitmap))
            {
                g.FillRectangle(Brushes.Transparent, cropRect);
                g.DrawImage(source, new Rectangle(0, 0, croppedBitmap.Width, croppedBitmap.Height), cropRect, GraphicsUnit.Pixel);
            }

            return croppedBitmap;
        }

        public static Bitmap RemoveEdgeBackground(Bitmap originalImage, int tolerance = 10)
        {
            // 获取边缘颜色
            Color backgroundColor = GetMostFrequentEdgeColor(originalImage);

            // 创建新位图，用于存放去除背景后的图像
            Bitmap transparentImage = new Bitmap(originalImage.Width, originalImage.Height);

            // 遍历每个像素
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color pixelColor = originalImage.GetPixel(x, y);

                    // 检查当前像素是否接近背景颜色
                    if (IsColorMatch(pixelColor, backgroundColor, tolerance))
                    {
                        // 设置为透明
                        transparentImage.SetPixel(x, y, Color.Transparent);
                    }
                    else
                    {
                        // 保留原颜色
                        transparentImage.SetPixel(x, y, pixelColor);
                    }
                }
            }

            return transparentImage;
        }

        public static Bitmap RemoveEdgeBackground2(Bitmap originalImage, int tolerance = 10)
        {
            // 获取边缘颜色
            (Color backgroundColor, Color backgroundColor2) = GetMostFrequentEdgeColor2(originalImage);

            // 创建新位图，用于存放去除背景后的图像
            Bitmap transparentImage = new Bitmap(originalImage.Width, originalImage.Height);

            // 遍历每个像素
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color pixelColor = originalImage.GetPixel(x, y);

                    // 检查当前像素是否接近背景颜色
                    if (IsColorMatch(pixelColor, backgroundColor, tolerance)|| IsColorMatch(pixelColor, backgroundColor2, tolerance))
                    {
                        // 设置为透明
                        transparentImage.SetPixel(x, y, Color.Transparent);
                    }
                    else
                    {
                        // 保留原颜色
                        transparentImage.SetPixel(x, y, pixelColor);
                    }
                }
            }

            return transparentImage;
        }

        public static Bitmap RemoveEdgeBackgroundN(Bitmap originalImage, int nMaxFreqLen, int tolerance = 10)
        {
            // 获取边缘颜色
            Color[] backgroundColors = GetMostFrequentEdgeColorN(originalImage, nMaxFreqLen);

            // 创建新位图，用于存放去除背景后的图像
            Bitmap transparentImage = new Bitmap(originalImage.Width, originalImage.Height);

            // 遍历每个像素
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color pixelColor = originalImage.GetPixel(x, y);

                    bool bUseOrg = true;
                    // 检查当前像素是否接近背景颜色
                    foreach(Color bkColor in backgroundColors)
                    {
                        if (IsColorMatch(pixelColor, bkColor, tolerance))
                        {
                            // 设置为透明
                            transparentImage.SetPixel(x, y, Color.Transparent);
                            bUseOrg = false;
                            break;
                        }
                    }
                    if(bUseOrg /*保留原颜色*/)
                        transparentImage.SetPixel(x, y, pixelColor);
                }
            }
            return transparentImage;
        }

        public static Bitmap RemoveBackground(Bitmap originalImage, Color backgroundColor, int tolerance = 10)
        {
            // 创建一个与原图大小相同的位图
            Bitmap transparentImage = new Bitmap(originalImage.Width, originalImage.Height);

            // 遍历每个像素
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color pixelColor = originalImage.GetPixel(x, y);

                    // 判断当前像素是否接近背景色（使用容差）
                    if (IsColorMatch(pixelColor, backgroundColor, tolerance))
                    {
                        // 设置为透明色
                        transparentImage.SetPixel(x, y, Color.Transparent);
                    }
                    else
                    {
                        // 保留原来的颜色
                        transparentImage.SetPixel(x, y, pixelColor);
                    }
                }
            }

            return transparentImage;
        }

        private static Color GetMostFrequentEdgeColor(Bitmap image)
        {
            Dictionary<Color, int> colorFrequency = new Dictionary<Color, int>();

            // 采样左、右、上、下边缘的颜色
            for (int y = 0; y < image.Height; y++)
            {
                CountColorFrequency(image.GetPixel(0, y), colorFrequency); // 左边缘
                CountColorFrequency(image.GetPixel(image.Width - 1, y), colorFrequency); // 右边缘
            }

            for (int x = 0; x < image.Width; x++)
            {
                CountColorFrequency(image.GetPixel(x, 0), colorFrequency); // 上边缘
                CountColorFrequency(image.GetPixel(x, image.Height - 1), colorFrequency); // 下边缘
            }

            // 找出频率最高的颜色
            Color mostFrequentColor = Color.White;
            int maxFrequency = 0;
            foreach (var kvp in colorFrequency)
            {
                if (kvp.Value > maxFrequency)
                {
                    mostFrequentColor = kvp.Key;
                    maxFrequency = kvp.Value;
                }
            }

            return mostFrequentColor;
        }

        private static (Color,Color) GetMostFrequentEdgeColor2(Bitmap image)
        {
            Dictionary<Color, int> colorFrequency = new Dictionary<Color, int>();

            // 采样左、右、上、下边缘的颜色
            for (int y = 0; y < image.Height; y++)
            {
                CountColorFrequency(image.GetPixel(0, y), colorFrequency); // 左边缘
                CountColorFrequency(image.GetPixel(image.Width - 1, y), colorFrequency); // 右边缘
            }

            for (int x = 0; x < image.Width; x++)
            {
                CountColorFrequency(image.GetPixel(x, 0), colorFrequency); // 上边缘
                CountColorFrequency(image.GetPixel(x, image.Height - 1), colorFrequency); // 下边缘
            }

            var sortedColor = colorFrequency
            .OrderByDescending(pair => pair.Value).ToArray();

            Color mostFrequentColor = Color.White;
            Color mostFrequentColor2 = Color.White;

            if (sortedColor.Count() > 0)
                mostFrequentColor = sortedColor[0].Key;
            if (sortedColor.Count() > 1)
            {
                mostFrequentColor2 = sortedColor[1].Key;
                if (Math.Abs(sortedColor[0].Value - sortedColor[1].Value) * 1.0 / Math.Max(sortedColor[0].Value, sortedColor[1].Value) < 0.2)
                    mostFrequentColor2 = sortedColor[1].Key;
                else 
                    mostFrequentColor2 = sortedColor[0].Key;
            }
            else
                mostFrequentColor2 = sortedColor[0].Key;


            return (mostFrequentColor, mostFrequentColor2);
        }

        private static Color[] GetMostFrequentEdgeColorN(Bitmap image, int nMaxFreq)
        {
            Dictionary<Color, int> colorFrequency = new Dictionary<Color, int>();

            // 采样左、右、上、下边缘的颜色
            for (int y = 0; y < image.Height; y++)
            {
                CountColorFrequency(image.GetPixel(0, y), colorFrequency); // 左边缘
                CountColorFrequency(image.GetPixel(image.Width - 1, y), colorFrequency); // 右边缘
            }

            for (int x = 0; x < image.Width; x++)
            {
                CountColorFrequency(image.GetPixel(x, 0), colorFrequency); // 上边缘
                CountColorFrequency(image.GetPixel(x, image.Height - 1), colorFrequency); // 下边缘
            }

            var sortedColor = colorFrequency
            .OrderByDescending(pair => pair.Value).ToArray();
            int nMax = sortedColor[0].Value;
            int nLen = 1;
            for (int i = 1; i < Math.Min(sortedColor.Count(), nMaxFreq); i++)
            {
                if ((nMax - sortedColor[i].Value) * 1.0 / nMax < 0.7)
                    nLen++;
                else
                    break;
            }

            Color[] mostFrequentColor = new Color[nLen];

            for (int i = 0; i < mostFrequentColor.Length; i++)
                mostFrequentColor[i] = sortedColor[i].Key;

            return mostFrequentColor;
        }

        public static Bitmap ResizeBitmap(Bitmap original, int newWidth, int newHeight)
        {
            // 创建一个新的Bitmap，用于存储缩放后的图像
            Bitmap resizedBitmap = new Bitmap(newWidth, newHeight);

            // 使用Graphics绘制图像，并设置插值模式提高图像质量
            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                // 设置图像插值模式（可以选择不同的模式以获得不同质量和速度的平衡）
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // 将原始图像绘制到新的大小
                graphics.DrawImage(original, 0, 0, newWidth, newHeight);
            }
            return resizedBitmap;
        }

        public static bool IsPngTransparent(string filePath)
        {
            // 加载 PNG 图像
            using (var bitmap = new Bitmap(filePath))
            {
                return IsPngTransparent(bitmap);
            }
        }

        public static bool IsPngTransparent(Bitmap? bitmap)
        {
            if (bitmap == null)
                return false;
            // 检查图像是否具有 Alpha 通道
            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb
                || bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb
                || bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format16bppArgb1555
                || bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb
                || bitmap.PixelFormat == PixelFormat.Format64bppArgb
                || bitmap.PixelFormat == PixelFormat.Format64bppPArgb)
            {
                // 图像不含 Alpha 通道，无法有透明背景
                return false;
            }

            // 遍历像素，检查是否存在透明区域
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);

                    // Alpha 值小于 255 表示有透明像素
                    if (pixelColor.A < 255)
                    {
                        return true;
                    }
                }
            }

            // 所有像素的 Alpha 值都为 255，图像没有透明区域
            return false;
        }

        public static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                // 将 Bitmap 保存到内存流
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                // 将流位置重置为起始位置
                memoryStream.Position = 0;

                // 创建 BitmapImage 并将流加载到其中
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                // 确保不再使用该流
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static BitmapImage? LoadTransparentImageFromLocalFile(string sWaterMark, int nMaxFreqLen=2)
        {
            if (!string.IsNullOrEmpty(sWaterMark) && !string.IsNullOrWhiteSpace(sWaterMark) && File.Exists(sWaterMark))
            {
                Bitmap? overlay1 = LoadImageToBitmapFromLocalFile(sWaterMark);

                if (overlay1 != null)
                {
                    if(!IsPngTransparent(overlay1))
                        overlay1 = RemoveEdgeBackgroundN(overlay1, nMaxFreqLen);
                    
                    return ConvertBitmapToBitmapImage(overlay1);
                }
            }
            return null;
        }

        static Dictionary<string, Bitmap?> _dictCachedWaterMark = new Dictionary<string, Bitmap?>();
        private static BitmapImage? fncLoadImageFromLocalFileWithWaterMark(string imagePath, string? sWaterMark, double Opacity, int nMaxFreqLen = 2, int nOration=0)
        {
            if (File.Exists(imagePath))
            {
                Bitmap? overlay1 = null;
                if (!string.IsNullOrEmpty(sWaterMark) && !string.IsNullOrWhiteSpace(sWaterMark) && File.Exists(sWaterMark))
                {
                    try
                    {
                        if (_dictCachedWaterMark.ContainsKey((string)sWaterMark)) 
                            overlay1 = _dictCachedWaterMark[(string)imagePath];

                        if (overlay1 == null)
                        {
                            overlay1 = LoadImageToBitmapFromLocalFile(sWaterMark);
                            _dictCachedWaterMark[(string)imagePath]=overlay1;
                            //Debug.Assert(!IsPngTransparent(overlay1));
                            if (overlay1 != null && !IsPngTransparent(overlay1))
                                overlay1 = RemoveEdgeBackgroundN(overlay1, nMaxFreqLen/*64*/);
                        }
                    }
                    catch (Exception)
                    { 
                    }
                }

                //watermark7.webp, approved14.avif, svg, ico
                using (Bitmap background = new Bitmap(imagePath))
                using (Graphics graphics = Graphics.FromImage(background))
                {
                    if (overlay1 != null)
                    {
                        float scale = Math.Min((float)background.Width / overlay1.Width, (float)background.Height / overlay1.Height);

                        // 计算缩放后的 overlay 尺寸
                        int newWidth = (int)(overlay1.Width * scale);
                        int newHeight = (int)(overlay1.Height * scale);
                        

                        Bitmap overlay= ResizeBitmap(overlay1, newWidth, newHeight);
                        // 设置透明度矩阵 (50% 透明度)
                        var colorMatrix = new System.Drawing.Imaging.ColorMatrix
                        {
                            Matrix33 = (float)Opacity // 透明度设置为 50%
                        };

                        // 创建图像属性并设置颜色矩阵
                        var imageAttributes = new System.Drawing.Imaging.ImageAttributes();
                        imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);
                        if (nOration % 360 != 0)
                        {
                            imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

                            // 将 graphics 的旋转中心设置为 overlay 的中心
                            graphics.TranslateTransform(newWidth /*overlay.Width*/ / 2, /*overlay.Height*/newHeight / 2);
                            graphics.RotateTransform(nOration); // 旋转 30 度
                            graphics.TranslateTransform(-newWidth/*overlay.Width*/ / 2, -/*overlay.Height*/newHeight / 2);
                        }

                        // 将透明背景图片绘制到背景图片上
                        //graphics.DrawImage(
                        //    overlay,
                        //    new Rectangle(background.Width / 6, background.Height / 6, background.Width * 2 / 3, background.Height * 2 / 3), // 调整大小和位置
                        //    0, 0, overlay.Width, overlay.Height,
                        //    GraphicsUnit.Pixel,
                        //    imageAttributes);
                        graphics.DrawImage(
                            overlay,
                            new Rectangle(background.Width / 2 - newWidth / 3, background.Height / 2 - newHeight / 3, newWidth * 2 / 3, newHeight * 2 / 3), // 调整大小和位置
                            0, 0, /*newWidth, newHeight,*/overlay.Width, overlay.Height,
                            GraphicsUnit.Pixel,
                            imageAttributes);

                        // 保存输出图片
                        //background.Save(outputImagePath);
                        using (var memoryStream = new MemoryStream())
                        {
                            // 将 Bitmap 保存到内存流中，以 PNG 格式保存
                            background.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                            // 重置流位置到起始
                            memoryStream.Position = 0;

                            // 创建 BitmapImage 并将流加载到其中
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = memoryStream;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();

                            // 确保 BitmapImage 可跨线程使用
                            bitmapImage.Freeze();

                            return bitmapImage;
                        }
                        Console.WriteLine("图片叠加完成！");
                    }
                    else
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                        bitmap.EndInit();
                        return bitmap;
                    }
                }
            }
            else
            {
                Trace.WriteLine("Image File Not found！");
                return null;
            }
        }

        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            // 创建内存流并将 BitmapImage 的数据写入流中
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);

                // 重置流的位置
                memoryStream.Position = 0;

                // 从流中创建 GDI+ Bitmap
                return new Bitmap(memoryStream);
            }
        }

        public static Bitmap? LoadImageToBitmapFromLocalFile(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();
                return BitmapImageToBitmap(bitmap);
            }
            else
            {
                Trace.WriteLine("PNG 文件未找到！");
                return null;
            }
        }

        public static Icon GetFileThumbnail(string filePath, bool largeIcon = true)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            uint flags = SHGFI_ICON | (largeIcon ? SHGFI_LARGEICON : SHGFI_SMALLICON);

            GcjWinApi.SHGetFileInfo(filePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);
            return Icon.FromHandle(shinfo.hIcon);
        }

        public static void GetFileThumbnailAndSaveToPng(string filePath, string filesave, bool largeIcon = true)
        {
            Icon ico = GetFileThumbnail(filePath, largeIcon);
            if (ico != null) SaveIconAsPng(ico, filesave);
        }

        public static void SaveIconAsPng(Icon icon, string filePath)
        {
            using (Bitmap bitmap = icon.ToBitmap())
            {
                bitmap.Save(filePath, ImageFormat.Png);
            }
        }

        public static BitmapImage? LoadImageFromLocalFileWaterMarkOption(string imagePath, string? sWaterMark=null/*"resources\\watermark6.png"*/, double Opacity =0.8, int nOration = 30)
        {
            bool bAddWaterMark = (!string.IsNullOrEmpty(sWaterMark) && !string.IsNullOrWhiteSpace(sWaterMark));
            Microsoft.Win32.RegistryKey? registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, false);
            if (bAddWaterMark == true)
            {
                if (registryKey != null)
                {
                    object? regValue = registryKey?.GetValue("WithWaterMark", 0);
                    if (regValue is string && (string.IsNullOrEmpty((string)regValue) || string.IsNullOrWhiteSpace((string)regValue) || "0" == ((string)regValue)?.Trim()))
                        bAddWaterMark = false;
                    else if (regValue is uint && (uint)regValue == 0)
                        bAddWaterMark = false;
                    else if (regValue is int && (int)regValue == 0)
                        bAddWaterMark = false;
                    else if (regValue is long && (long)regValue == 0)
                        bAddWaterMark = false;
                    else if (regValue is ulong && (ulong)regValue == 0)
                        bAddWaterMark = false;
                    registryKey?.Close();
                }
                else
                {
                    registryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(REGISTRY_PATH);
                    registryKey?.SetValue("WithWaterMark", 0, Microsoft.Win32.RegistryValueKind.DWord);
                    registryKey?.Close();
                }
            }

            if (!bAddWaterMark)
                return fncLoadImageFromLocalFile(imagePath);
            else
                return fncLoadImageFromLocalFileWithWaterMark(imagePath, sWaterMark, Opacity);
        }



#endif

        // 统计颜色频率
        private static void CountColorFrequency(Color color, Dictionary<Color, int> frequency)
        {
            if (frequency.ContainsKey(color))
            {
                frequency[color]++;
            }
            else
            {
                frequency[color] = 1;
            }
        }

        // 比较颜色，使用一定的容差
        private static bool IsColorMatch(Color color1, Color color2, int tolerance)
        {
            return Math.Abs(color1.R - color2.R) <= tolerance &&
                   Math.Abs(color1.G - color2.G) <= tolerance &&
                   Math.Abs(color1.B - color2.B) <= tolerance;
        }

        public static void SaveBitmapSourceAsPng(BitmapSource bitmap, string filePath)
        {
            // 确保目标目录存在
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // 创建一个 PngBitmapEncoder 并将 BitmapSource 添加到其中
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            // 将图像编码并保存到指定路径
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static BitmapImage? LoadImageFromLocalFile(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();
                return bitmap;
            }
            else
            {
                Trace.WriteLine("PNG 文件未找到！");
                return null;
            }
        }

        private static BitmapImage? fncLoadImageFromLocalFile(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();
                return bitmap;
            }
            else
            {
                Trace.WriteLine("PNG 文件未找到！");
                return null;
            }
        }

    }
}
