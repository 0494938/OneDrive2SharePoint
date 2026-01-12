using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace GcjUtil
{
    public partial class ControlUtil
    {
        public static string? GetControlTemplateAsXaml(System.Windows.Controls.Control control)
        {
            // 获取控件的 ControlTemplate
            ControlTemplate template = control.Template;

            if (template != null)
            {
                // 使用 StringWriter 创建 XmlWriter
                StringWriter stringWriter = new StringWriter();
                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true });

                // 使用 XamlWriter.Save 将模板序列化为 XAML
                XamlWriter.Save(template, xmlWriter);
                xmlWriter.Close();

                // 返回 XAML 字符串
                return stringWriter.ToString();
            }
            return null; // 如果没有模板，返回null
        }

        public static string? GetControlTemplateAsPrettyXaml(System.Windows.Controls.Control control)
        {
            // 获取控件的 ControlTemplate
            ControlTemplate template = control.Template;

            if (template != null)
            {
                StringWriter stringWriter = new StringWriter();
                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true , NewLineOnAttributes =true,});

                // 使用 XamlWriter.Save 将模板序列化为 XAML
                XamlWriter.Save(template, xmlWriter);
                xmlWriter.Close();
                return stringWriter.ToString() ;
            }
            return null; // 如果没有模板，返回null
        }

        public static string? GetDataTemplateAsXaml(FrameworkElement element, string sDateTemplateKey = "YourDataTemplateKey")
        {
            DataTemplate? dataTemplate = element.Resources[sDateTemplateKey] as DataTemplate;

            if (dataTemplate != null)
            {
                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
                XamlWriter.Save(dataTemplate, xmlWriter);
                return stringWriter.ToString();
            }
            return null;
        }

        public static string? GetDataTemplateAsPrettyXaml(FrameworkElement element, string sDateTemplateKey = "YourDataTemplateKey")
        {
            DataTemplate? dataTemplate = element.Resources[sDateTemplateKey] as DataTemplate;

            if (dataTemplate != null)
            {
                StringWriter stringWriter = new StringWriter();
                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true, NewLineOnAttributes = true, });
                XamlWriter.Save(dataTemplate, xmlWriter);
                return stringWriter.ToString();
            }
            return null;
        }

        public static DataTemplate? GetDataTemplate(System.Windows.Controls.Control control)
        {
            // 检查控件是否有 ContentTemplate
            if (control != null && control is System.Windows.Controls.ContentControl)
            {
                // 返回绑定的 DataTemplate
                return (control as System.Windows.Controls.ContentControl)?.ContentTemplate;
            }
            else if(control != null && control is System.Windows.Controls.ItemsControl)
            {
                // 返回绑定的 DataTemplate
                return (control as System.Windows.Controls.ItemsControl)?.ItemTemplate;
            }
            return null; // 如果没有绑定 DataTemplate，返回 null
        }

        public static string? GetDataTemplateAsPrettyXaml(System.Windows.Controls.Control control)
        {
            DataTemplate? dataTemplate = null;
            // 检查控件是否有 ContentTemplate
            if (control != null && control is System.Windows.Controls.ContentControl)
            {
                // 返回绑定的 DataTemplate
                dataTemplate =(control as System.Windows.Controls.ContentControl)?.ContentTemplate;
            }
            else if (control != null && control is System.Windows.Controls.ItemsControl)
            {
                // 返回绑定的 DataTemplate
                dataTemplate = (control as System.Windows.Controls.ItemsControl)?.ItemTemplate;
            }
            if(dataTemplate != null)
            {
                StringWriter stringWriter = new StringWriter();
                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true, NewLineOnAttributes = true, });
                XamlWriter.Save(dataTemplate, xmlWriter);
                return stringWriter.ToString();
            }
            return null; // 如果没有绑定 DataTemplate，返回 null
        }

        public static Brush GetRandomBrush()
        {
            Random rand = new Random();
            return new SolidColorBrush(Color.FromRgb(
                (byte)rand.Next(0, 256),
                (byte)rand.Next(0, 256),
                (byte)rand.Next(0, 256)));
        }
#if false
        private static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // 将 BitmapImage 转换为 MemoryStream
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);

                // 创建 Bitmap
                using (Bitmap bitmap = new Bitmap(memoryStream))
                {
                    return new Bitmap(bitmap);  // 返回新的 Bitmap 对象
                }
            }
        }

        public static Icon? CreateIconFromBitmap(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                // PNG 图片的像素格式需要是 32bpp，因为 ICO 格式支持 32bpp 带透明通道
                if (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                {
                    throw new ArgumentException("PNG 文件必须包含透明背景 (32bpp ARGB)。");
                }

                using (MemoryStream iconStream = new MemoryStream())
                {
                    bitmap.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png);  // 保存为 PNG 格式
                    return new Icon(iconStream);  // 从内存流创建 Icon 对象
                }
            }
            else return null;
        }
#endif
    }
}
