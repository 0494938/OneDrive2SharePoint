using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedLaunchImage : System.Windows.Controls.Image
    {
        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedLaunchImage),
            new PropertyMetadata("Gcj Image Button v1.0"));


        [Description("Gets or sets the CustomizedLaunchImage Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性

        public CustomizedLaunchImage() : base()
        {
            //DefaultStyleKey = typeof(CustomizedImageButton);
            Loaded += CustomizedLaunchImage_Loaded;
            MouseLeftButtonDown += CustomizedLaunchImage_MouseLeftButtonDown; ;
        }

        static readonly string[] randUrls = new string[] { 
            "https://www.tel.co.jp/news/topics/2024/20240930_001.html", 
            "https://www.tel.co.jp/rd/base/index.html", 
            "https://www.tel.co.jp/product/all/index.html",
            "https://www.tel.co.jp/product/service/index.html",
            "https://www.tel.co.jp/ir/personal/index.html",
            "https://www.tel.co.jp/corporatesummary/index.html",
        };

        static Random random = new Random();

        private void CustomizedLaunchImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int randomNumber = random.Next(0, randUrls.Length);
            Process.Start(new ProcessStartInfo
            {
                FileName = randUrls[randomNumber],
                UseShellExecute = true // 必须设置为 true，才能使用默认浏览器
            });
        }

        private void CustomizedLaunchImage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("$$$$ CustomizedLaunchImage_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
        }

    }
}
