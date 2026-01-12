using BaseUtil;
using GcjUiCtrl.Control;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace IsidTestApp
{
    /// <summary>
    /// TestControlWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class Temp1OrgTelTestWedgitWindow : GcjBaseWindow
    {
        private void OnBackGroundColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if (datacontext != null && e.NewValue != null)
            {
                datacontext.CustomizedBgColor = e.NewValue ?? (Colors.BlueViolet);
                RgnBorder.GetBindingExpression(Border.BackgroundProperty)?.UpdateTarget();
            }
        }
        private Point _startPoint;

        public Temp1OrgTelTestWedgitWindow()
        {
            InitializeComponent();
            InitBrowser();

            this.ShowInTaskbar = false;

            // 设置窗口不能调整大小
            this.ResizeMode = ResizeMode.NoResize;
            //_widgets = new ObservableCollection<Widget>();

            // ... 其他初始化代码

        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Minimized)
            {
                // 隐藏窗口
                //this.Hide();
            }
        }

        public async void InitBrowser()
        {
            TestContext? datacontext = this.DataContext as TestContext;
            string sFileSavePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string sFileTempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "temp\\";
            Microsoft.Win32.RegistryKey? registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(CtrlUtil.REGISTRY_PATH, false);
            if (registryKey != null)
            {
                sFileSavePath = (registryKey?.GetValue("FileSavePath") as string) ?? sFileSavePath;
                sFileTempPath = (registryKey?.GetValue("FileTempPath") as string) ?? sFileTempPath;
                registryKey?.Close();
            }

            var env = await CoreWebView2Environment.CreateAsync(null, string.IsNullOrEmpty(sFileSavePath) ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) : sFileSavePath);
            await webBrowser.EnsureCoreWebView2Async(env);
            await webBrowserRobot.EnsureCoreWebView2Async(env);

            webBrowser.NavigationCompleted += (s, e) => WebBrowser_NavigationCompleted("SystemMap", s, e);
            webBrowser.Loaded += (s, e) => WebBrowser_Loaded("SystemMap", s, e);
            webBrowser.CoreWebView2.WebMessageReceived += (s, e)=>CoreWebView2_WebMessageReceived("SystemMap",s,e);
            webBrowser.CoreWebView2.NewWindowRequested += (s, e) => CoreWebView2_NewWindowRequested("SystemMap", s, e);

            webBrowserRobot.NavigationCompleted += (s, e) => WebBrowser_NavigationCompleted("Robot", s, e);
            webBrowserRobot.Loaded += (s, e) => WebBrowser_Loaded("Robot", s, e);
            webBrowserRobot.CoreWebView2.WebMessageReceived += (s, e) => CoreWebView2_WebMessageReceived("Robot", s, e);
            webBrowserRobot.CoreWebView2.NewWindowRequested += (s, e) => CoreWebView2_NewWindowRequested("Robot", s, e);

            webBrowser.CoreWebView2.Navigate("https://www.google.com/");
            webBrowserRobot.CoreWebView2.Navigate("https://chatgpt.com/");
        }

        private void WebBrowser_Loaded(string webBrowserName, object sender, RoutedEventArgs e)
        {
            if ("Robot" == webBrowserName)
            {
            };
            if ("SystemMap" == webBrowserName)
            {
            };
        }

        private void CoreWebView2_NewWindowRequested(string webBrowserName, object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:IsidTestControlWindow::CoreWebView2_NewWindowRequested Triggered");
            if ("Robot" == webBrowserName)
            {
                e.NewWindow = (sender as CoreWebView2);
            };
            if ("SystemMap" == webBrowserName)
            {
                e.NewWindow = (sender as CoreWebView2);
            };
        }

        private void CoreWebView2_WebMessageReceived(string webBrowserName, object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string sWebMessage = e.TryGetWebMessageAsString();

            if(sWebMessage.StartsWith("http://") || sWebMessage.StartsWith("https://")) { 
            }else if(sWebMessage.StartsWith("{\""))
            {
                try
                {
                    dynamic? message = Newtonsoft.Json.JsonConvert.DeserializeObject(sWebMessage);
                    string? sEvtName = message?.evt;

                    if (sEvtName == "focus" || "keydown" == sEvtName)
                    {
                        string? tagName = message?.tag;
                        string? name = message?.name;

                        if (tagName == "DIV" && "prompt-textarea" == name)
                        {
                            string? value = message?.value;
                            string? key = message?.key;
                            Debug.WriteLine($"{webBrowserName} WebView2 -- Prompt Area Event;{sEvtName}(tag:{tagName}, tagName:{name}, value:{value}, key:{key})");
                            if (sEvtName == "focus" || sEvtName == "keydown" && key == "Enter")
                                HandleRobotInputMode(sEvtName, tagName, name, value, key);
                        }
                        else
                            Debug.WriteLine($"{webBrowserName} WebView2 -- Event;{sEvtName}(tag:{tagName}, tagName:{name})");
                    }
                    else if (sEvtName == "scroll") {
                        string? tagName = message?.tag;
                        string? name = message?.name;
                        double? scrollTop = message?.scrollTop;
                        double? scrollLeft = message?.scrollLeft;
                        Debug.WriteLine($"{webBrowserName} WebView2 -- Event;{sEvtName}(tag:{tagName}, tagName:{name}, scrollTop:{scrollTop}, scrollLeft:{scrollLeft})");
                    }
                    else if ("wheel" == sEvtName)
                    {
                        string? tagName = message?.tag;
                        string? name = message?.name;
                        double? deltaX = message?.deltaX;
                        double? deltaY = message?.deltaY;
                        double? deltaZ = message?.deltaZ;
                        Debug.WriteLine($"{webBrowserName} WebView2 -- Event;{sEvtName}(tag:{tagName}, tagName:{name}, deltaX:{deltaX}, deltaY:{deltaY}), deltaZ:{deltaZ})");
                    }
                    else
                    {
                        Debug.WriteLine($"{webBrowserName} WebView2 -- Event Unknown;{sEvtName}");
                    }
                }
                catch (Exception ex) {
                    Debug.WriteLine($"{webBrowserName} WebView2 -- Parse Json Exception Happened;(Message:{sWebMessage})");
                }
            }
            else
            {
                Debug.WriteLine($"CoreWebView2_WebMessageReceived:{webBrowserName}:Unhandled:{sWebMessage}");
            }
        }

        static readonly string scrollMonitorScript = @"
            document.body.style.backgroundColor = 'transparent';
            document.documentElement.style.backgroundColor = 'transparent';
            document.addEventListener('scroll', function(event) {
                let targetElement = event.target;
                window.chrome.webview.postMessage(JSON.stringify({
                    evt: 'scroll',
                    tag: targetElement.tagName,
                    name: targetElement.name || targetElement.id || '',
                    scrollTop: window.pageYOffset,
                    scrollLeft: window.pageXOffset
                }));
            });
            document.addEventListener('wheel', function(event) {
                let targetElement = event.target;
                window.chrome.webview.postMessage(JSON.stringify({
                    evt: 'wheel',
                    tag: targetElement.tagName,
                    name: targetElement.name || targetElement.id || '',
                    deltaX: event.deltaX,
                    deltaY: event.deltaY,
                    deltaZ: event.deltaZ
                }));
            });         
        ";

        static readonly string focusMonitorScript = scrollMonitorScript + @"
            document.addEventListener('focus', function(event) {
                var target = event.target;
                var tagName = target.tagName;  // 获取标签名
                var name = target.name || target.id || '';  // 获取名称或 ID
                window.chrome.webview.postMessage(JSON.stringify({evt:'focus', tag: tagName, name: name}));
            }, true);
            document.addEventListener('keydown', function(event) {
                var target = event.target;
                var tagName = target.tagName;  // 获取标签名
                var name = target.name || target.id || '';  // 获取名称或 ID
                var inputValue = target.value || '';  // 获取当前的输入值
                var keyPressed = event.key;  // 获取按下的键

                // 发送数据到 WPF
                window.chrome.webview.postMessage(JSON.stringify({
                    evt: 'keydown',
                    tag: tagName,
                    name: name,
                    value: inputValue,
                    key: keyPressed
                }));
            }, true);
            "
                /* + @"
                   document.addEventListener('keyup', function(event) {
                       var target = event.target;
                       var tagName = target.tagName;  // 获取标签名
                       var name = target.name || target.id || '';  // 获取名称或 ID
                       var inputValue = target.value || '';  // 获取输入的值

                       // 发送数据到 WPF
                       window.chrome.webview.postMessage(JSON.stringify({
                           evt: 'keyup',
                           tag: tagName,
                           name: name,
                           value: inputValue
                       }));
                   }, true);
                   document.addEventListener('keypress', function(event) {
                       var target = event.target;
                       var tagName = target.tagName;  // 获取标签名
                       var name = target.name || target.id || '';  // 获取名称或 ID
                       var inputValue = target.value || '';  // 获取当前的输入值
                       var keyPressed = event.key;  // 获取按下的键

                       // 发送数据到 WPF
                       window.chrome.webview.postMessage(JSON.stringify({
                           evt: 'keypress',
                           tag: tagName,
                           name: name,
                           value: inputValue,
                           key: keyPressed
                       }));
                   }, true);
               "*/
                ;

        private void WebBrowser_NavigationCompleted(string webBrowserName, object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if("Robot" == webBrowserName)
            {
                webBrowserRobot.CoreWebView2.ExecuteScriptAsync(focusMonitorScript);
            };
            if ("SystemMap" == webBrowserName)
            {
                webBrowser.CoreWebView2.ExecuteScriptAsync(scrollMonitorScript);
            };
        }

        public void LoadUiUrl(WebView2 browser, TestContext? datacontext, string strURL)
        {
            if (datacontext != null)
            {
                if (!strURL.StartsWith("http://") && !strURL.StartsWith("https://"))
                {
                    strURL = "https://" + strURL;
                }
                browser.CoreWebView2.Navigate(strURL);
            }
        }
    }
}
