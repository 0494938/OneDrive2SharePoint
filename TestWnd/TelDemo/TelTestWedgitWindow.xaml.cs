using BaseUtil;
using GcjUiCtrl.Control;
using Microsoft.Web.WebView2.Core;
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
    public partial class TelTestWedgitWindow : GcjBaseWindow
    {
        private Point _startPoint;

        public TelTestWedgitWindow()
        {
            InitializeComponent();
            InitBrowser();

            this.ShowInTaskbar = false;

            // 设置窗口不能调整大小
            //this.ResizeMode = ResizeMode.NoResize;
            //_widgets = new ObservableCollection<Widget>();

            // ... 其他初始化代码
            CreateNotifyIcon(1);
            this.Loaded += TelTestWedgitWindow_Loaded;
        }

        #region EventHandler
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Minimized)
            {
                // 隐藏窗口
                //this.Hide();
            }
        }

        private void OnBackGroundColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if (datacontext != null && e.NewValue != null)
            {
                datacontext.CustomizedBgColor = e.NewValue ?? (Colors.BlueViolet);
                RgnBorder.GetBindingExpression(Border.BackgroundProperty)?.UpdateTarget();
            }
        }

        private void TelTestWedgitWindow_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WebBrowser_NavigationCompleted(string webBrowserName, object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if ("Robot" == webBrowserName)
            {
                webBrowserRobot.CoreWebView2.ExecuteScriptAsync(focusMonitorScript);
            };
            if ("SystemMap" == webBrowserName)
            {
                //webBrowser.CoreWebView2.ExecuteScriptAsync(scrollMonitorScript);
                DelayMonitorIFrame();
                Debug.WriteLine("after DelayMonitorIFrame...");
            };
        }

        #endregion EventHandler

        #region Methods
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
            webBrowser.CoreWebView2.WebMessageReceived += (s, e) => CoreWebView2_WebMessageReceived("SystemMap", s, e);
            webBrowser.CoreWebView2.NewWindowRequested += (s, e) => CoreWebView2_NewWindowRequested("SystemMap", s, e);

            webBrowserRobot.NavigationCompleted += (s, e) => WebBrowser_NavigationCompleted("Robot", s, e);
            webBrowserRobot.Loaded += (s, e) => WebBrowser_Loaded("Robot", s, e);
            webBrowserRobot.CoreWebView2.WebMessageReceived += (s, e) => CoreWebView2_WebMessageReceived("Robot", s, e);
            webBrowserRobot.CoreWebView2.NewWindowRequested += (s, e) => CoreWebView2_NewWindowRequested("Robot", s, e);

            //webBrowser.CoreWebView2.Navigate("https://www.google.com/");
            webBrowser.CoreWebView2.Navigate("https://www.yahoo.co.jp/");
            webBrowserRobot.CoreWebView2.Navigate("https://chatgpt.com/");
        }

        public void LoadUiUrl(Microsoft.Web.WebView2.Wpf.WebView2 browser, TestContext? datacontext, string strURL)
        {
            if (datacontext != null)
            {
                if (!strURL.StartsWith("http://") && !strURL.StartsWith("https://"))
                {
                    strURL = "https://" + strURL;
                }
                webBrowser.CoreWebView2.Navigate(strURL);
            }
        }

        private async void DelayMonitorIFrame()
        {
            Debug.WriteLine("before DelayMonitorIFrame::Task.Delay ...");
            await Task.Delay(5000);  // Wait for 10 seconds
                                     // After the delay, use Dispatcher to ensure the task runs on the UI thread
            Debug.WriteLine("before DelayMonitorIFrame::Dispatcher.InvokeAsync async script ...");
            await this.Dispatcher.InvokeAsync(() =>
            {
                // Code that should run after 10 seconds on the UI thread
                webBrowser.CoreWebView2.ExecuteScriptAsync(iframeFocusInject);
            });
        }
        #endregion Methods

        #region JavaScript for Hook
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
            console.log('focus inject begin');
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
            ";

        string iframeFocusInject = @"
            console.log('focus inject begin');
            document.addEventListener('focus', function(event) {
                var target = event.target;
                var tagName = target.tagName;  // 获取标签名
                var name = target.name || target.id || '';  // 获取名称或 ID
                window.chrome.webview.postMessage(JSON.stringify({evt:'focus', tag: tagName, name: name}));
            }, true);
            console.log('keydown inject begin');
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

            console.log('DOMContentLoaded event inject begin');
            //the following may never has chance to run as it will be launched in WebBrowser_NavigationCompleted event
            document.addEventListener('DOMContentLoaded', function() {
                let iframes = document.getElementsByTagName('iframe');
                console.log('begin iframe enum in DOMContentLoaded');
                for (let i = 0; i < iframes.length; i++) {
                    let iframe = iframes[i];

                    // 监听 iframe 的 load 事件
                    if(iframe){
                        iframe.addEventListener('load', function() {
                            console.log('in iframe load event');
                            try {
                                // 尝试访问 iframe 的 contentDocument
                                if (iframe.contentDocument) {
                                    let iframeDocument = iframe.contentDocument;
                                
                                    iframeDocument.addEventListener('focus', function(event) {
                                        var target = event.target;
                                        var tagName = target.tagName;  // 获取标签名
                                        var name = target.name || target.id || '';  // 获取名称或 ID
                                        window.chrome.webview.postMessage(JSON.stringify({evt:'focus', tag: tagName, name: name}));
                                    }, true);

                                }
                            } catch (error) {
                                console.log('Error accessing iframe content: ' + error);
                            }
                        });
                    }else{
                        console.log('iframe is null in DOMContentLoaded event' + (i+1));
                    }
                }
                console.log('after iframe enum in DOMContentLoaded');
            });
            console.log('DOMContentLoaded event inject end');

            let iframes = document.getElementsByTagName('iframe');
            console.log('iframe found : ' + iframes.length + ' begin enum');
            for (let i = 0; i < iframes.length; i++) {
                //this part to handle which has been loaded...
                console.log('begin add focus for ' + (i+1));
                try { 
                    let iframe = iframes[i];
                    if(iframe ){
                        let iframeDocument = iframe.contentDocument;
                                
                        if (iframeDocument){
                            iframeDocument.addEventListener('focus', function(event) {
                                var target = event.target;
                                var tagName = target.tagName;  // 获取标签名
                                var name = target.name || target.id || '';  // 获取名称或 ID
                                window.chrome.webview.postMessage(JSON.stringify({evt:'focus', tag: tagName, name: name}));
                            }, true);
                        }else{
                            console.log('iframeDocument is null in focus inject event' + (i+1));
                        }
                    }else{
                        console.log('iframe is null for focus event' + (i+1));
                    }
                } catch (error) {
                    console.log('Error accessing iframe : ' + error);
                }
                //this part to handle which has not been loaded.
                try { 
                    console.log('begin inject focus in load event of iframe ' + (i+1));
                    let iframe = iframes[i];
                    // 监听 iframe 的 load 事件
                    if(iframe){
                        iframe.addEventListener('load', function() {
                            console.log('in iframe load event to inject focus event');
                            try {
                                // 尝试访问 iframe 的 contentDocument
                                if (iframe.contentDocument) {
                                    let iframeDocument = iframe.contentDocument;

                                    iframeDocument.addEventListener('focus', function(event) {
                                        var target = event.target;
                                        var tagName = target.tagName;  // 获取标签名
                                        var name = target.name || target.id || '';  // 获取名称或 ID
                                        window.chrome.webview.postMessage(JSON.stringify({evt:'focus', tag: tagName, name: name}));
                                    }, true);
                                }else{
                                    console.log('iframeDocument is null in frame load event' + (i+1));
                                }
                            } catch (error) {
                                console.log('Error accessing iframe content: ' + error);
                            }
                        });
                    }else{
                        console.log('iframe is null in frame load event injection' + (i+1));
                    }
                    console.log('after inject focus in load event of iframe ' + (i+1));
                } catch (error) {
                    console.log('Error accessing iframe : ' + error);
                }
                console.log('after add focus for ' + (i+1));
            }
            console.log('after iframe enum');

            ";
        #endregion JavaScript for Hook
    }
}
