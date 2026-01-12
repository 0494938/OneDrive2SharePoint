using BaseUtil;
using GcjUiCtrl.Control;
using GcjUtil;
using Microsoft.Web.WebView2.Core;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace IsidTestApp
{
    /// <summary>
    /// TestControlWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TelTestSearchWedgit : GcjBaseWindow
    {
        #region Variables
        private Point _startPoint;
        private ObservableCollection<string> dataSource = new ObservableCollection<string>
        {
            "Apple", "Banana", "Cherry", "Date", "Fig", "Grape", "Kiwi",
            "Apple1", "Banana1", "Cherry1", "Date1", "Fig1", "Grape1", "Kiwi1",
            "Apple2", "Banana2", "Cherry2", "Date2", "Fig2", "Grape2", "Kiwi2",
        };

        public ObservableCollection<string> FilteredItems { get; set; } = new ObservableCollection<string>();

        #endregion Variables

        public TelTestSearchWedgit()
        {
            InitializeComponent();
            InitBrowser();

            //this.ShowInTaskbar = false;

            // 设置窗口不能调整大小
            //this.ResizeMode = ResizeMode.NoResize;
            //_widgets = new ObservableCollection<Widget>();

            // ... 其他初始化代码
            CreateNotifyIcon(1);
            this.Loaded += TelTestSearchWedgit_Loaded;
            SuggestionsList.ItemsSource = FilteredItems; // 绑定到ListBox
        }

        #region EventHandler
        private void imgRobotReturn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtFlowTitle.Visibility = Visibility.Visible;
            txtFlowDetail.Visibility = Visibility.Visible;
            scRobotTitle.Visibility = Visibility.Visible;
            txtFlowCategoryTitle.Visibility = Visibility.Visible;
            imgRobotReturn.Visibility = Visibility.Collapsed;
            scRight.UpdateLayout();
        }

        private void imgSystemMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("CustomizedRoundCornerImage_MouseLeftButtonDown Clicked");
            TestContext? datacontext = this.DataContext as TestContext;
            grdSystem.Visibility = Visibility.Collapsed;
            webBrowser.Visibility = Visibility.Visible;
            imgSystemMapReturn.Visibility = Visibility.Visible;
            if (datacontext != null)
            {
                datacontext.SystemMapBrowserMode = true;
            }
        }

        private void imgSystemMapReturn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("imgSystemMapReturn_MouseLeftButtonDown Clicked");
            TestContext? datacontext = this.DataContext as TestContext;
            grdSystem.Visibility = Visibility.Visible;
            webBrowser.Visibility = Visibility.Collapsed;
            imgSystemMapReturn.Visibility = Visibility.Collapsed;
            if (datacontext != null)
            {
                datacontext.SystemMapBrowserMode = false;
                datacontext.IsSystemFrameSelected = true;
                if (datacontext.SystemMapBrowserMode)
                {
                    webBrowser.Visibility = Visibility.Collapsed;
                    grdSystem.Visibility = Visibility.Visible;
                }
            }
            selectedFile.GetBindingExpression(System.Windows.Shapes.Rectangle.VisibilityProperty).UpdateTarget();
            selectedSystem.GetBindingExpression(System.Windows.Shapes.Rectangle.VisibilityProperty).UpdateTarget();
            grdFile.Visibility = Visibility.Collapsed;
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if ((datacontext != null))
            {
                datacontext.IsFileFrameSelected = true;
                selectedFile.GetBindingExpression(System.Windows.Shapes.Rectangle.VisibilityProperty).UpdateTarget();
                selectedSystem.GetBindingExpression(System.Windows.Shapes.Rectangle.VisibilityProperty).UpdateTarget();
                grdFile.Visibility = Visibility.Visible;
                grdSystem.Visibility = Visibility.Collapsed;
                webBrowser.Visibility = Visibility.Collapsed;
            }
        }

        private void btnSystem_Click(object sender, RoutedEventArgs e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if ((datacontext != null))
            {
                datacontext.IsSystemFrameSelected = true;
                selectedFile.GetBindingExpression(System.Windows.Shapes.Rectangle.VisibilityProperty).UpdateTarget();
                selectedSystem.GetBindingExpression(System.Windows.Shapes.Rectangle.VisibilityProperty).UpdateTarget();
                grdFile.Visibility = Visibility.Collapsed;
                if (datacontext.SystemMapBrowserMode)
                {
                    webBrowser.Visibility = Visibility.Visible;
                    grdSystem.Visibility = Visibility.Collapsed;
                }
                else
                {
                    webBrowser.Visibility = Visibility.Collapsed;
                    grdSystem.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnShowAllRecentFile_Click(object sender, RoutedEventArgs e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if ((datacontext != null))
            {
                datacontext.FileRecentShowAll = !datacontext.FileRecentShowAll;
                List<CustomizedFileInfoCtrl> fileInfos = DepUtil.GetAllChildrenOfType<CustomizedFileInfoCtrl>(grdFile);
                foreach (CustomizedFileInfoCtrl fileInfo in fileInfos)
                {
                    if (datacontext.FileRecentShowAll && fileInfo.ItemIndex <= 1000)
                    {
                        fileInfo.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (fileInfo.ItemIndex > 4 && fileInfo.ItemIndex <= 1000)
                            fileInfo.Visibility = Visibility.Collapsed;
                    }
                }
                //UpdateAllControlsVisual(grdFile);
                grdFile.UpdateLayout();
            }
        }

        private void btnShowAllShareFile_Click(object sender, RoutedEventArgs e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if ((datacontext != null))
            {
                datacontext.FileSharedShowAll = !datacontext.FileSharedShowAll;
                //UpdateAllControlsVisual(grdFile);
                List<CustomizedFileInfoCtrl> fileInfos = DepUtil.GetAllChildrenOfType<CustomizedFileInfoCtrl>(grdFile);
                foreach (CustomizedFileInfoCtrl fileInfo in fileInfos)
                {
                    if (datacontext.FileSharedShowAll && fileInfo.ItemIndex > 1000)
                    {
                        fileInfo.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (fileInfo.ItemIndex > 1004)
                            fileInfo.Visibility = Visibility.Collapsed;
                    }
                }
            }
            //UpdateAllControlsVisual(grdFile);
            grdFile.UpdateLayout();
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

        private void TelTestSearchWedgit_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void btnDbg_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.TextValue = "b";
        }

        private void HandleSearchBox_TextChanged()
        {
            txtDebugMsg.Text = $"TextValue: {SearchBox.TextValue}";
            string query = SearchBox.Text.Trim();

            if (string.IsNullOrEmpty(query))
            {
                SuggestionPopup.IsOpen = false;
                return;
            }

            // 根据输入文本过滤数据源
            var results = dataSource
                .Where(item => item.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // 更新 ListBox 数据源
            FilteredItems.Clear();
            foreach (var item in results)
            {
                FilteredItems.Add(item);
            }

            // 若有匹配结果则显示 Popup  
            if (SuggestionPopup != null)
                SuggestionPopup.IsOpen = results.Any();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            HandleSearchBox_TextChanged();
        }

        // 用户选择项目时关闭 Popup 并处理选择
        private void SuggestionsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SuggestionsList.SelectedItem is string selectedItem)
            {
                SearchBox.TextValue = selectedItem;
                SuggestionPopup.IsOpen = false;

                // TODO: 执行检索或其他操作
                MessageBox.Show($"搜索: {selectedItem}");
            }
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
        
        private void WebBrowser_NavigationCompleted(string webBrowserName, object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if("Robot" == webBrowserName)
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        #endregion Methods

        #region JavaScripts
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
        #endregion JavaScripts

    }
}
