using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedTabWebCtrl : System.Windows.Controls.ContentControl
    {
        System.Windows.Controls.TabControl? _tabControl = null;
        public CustomizedTabWebCtrl() : base()
        {
            //DefaultStyleKey = typeof(CustomizedTabWebCtrl);
            if (IsInDesignMode()) {
                AddTabItem("Google", "https://www.youtube.com/watch?v=-yG5cYyDAT0", -1);
                //AddTabItem(" + ", "");
            }
            else {
                Loaded += CustomizedTabWebCtrl_Loaded;
                if (Items.Count == 0)
                {
                    AddTabItem("Loading...", "https://www.youtube.com/watch?v=-yG5cYyDAT0");
                    AddTabItem("Loading...", "https://www.youtube.com");
                    //AddTabItem(" + ", "");
                }
            }
        }

        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(this);
        }

        private void CustomizedTabWebCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void CustomizedTabWebCtrl_Loaded Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
            _tabControl = DepUtil.GetFirstChildrenOfType<System.Windows.Controls.TabControl>(this);
            Debug.Assert(_tabControl!=null);

            if (!IsInDesignMode())
            {
                InitBrowser();
                _tabControl.SelectionChanged += _tabControl_SelectionChanged;
                _tabControl.SelectedIndex = 0;
            }

            //ControlUtil.UpdateAllControlsVisual(this, 2);
        }

        private void _tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
#if false
            if (_tabControl !=null && (_tabControl?.SelectedItem is WebVWTabItem)){
                if (_tabControl?.SelectedItem is WebVWTabItem curItem) {
                    object header = curItem.Header;
                    if(header is string sHeader)
                    {
                        if (sHeader.Trim() == "+")
                        {
                            int nCurIdx = _tabControl.SelectedIndex;

                            //AddTabItem("(Empty)", "", nCurIdx);
                            ////_tabControl.ItemsSource = null; _tabControl.ItemsSource = Items;
                            //_tabControl.SelectedIndex = nCurIdx;
                            //_tabControl.UpdateLayout();

                            WebVWTabItem tabItem = new WebVWTabItem
                            {
                                //Style= (Style)this.FindResource("xCustomizedChromeTabItemStyle"),
                                Header = header,
                                Name = $"Tab{Items.Count + 1}",
                                Content = (sHeader.Trim() == "+") ? null : (new CustomizedAddressedWebView2()
                                {
                                    Style = (Style)this.FindResource("xCustomizedAddressedWebView2Style"),
                                }) // 可以自定义内容
                            };

                            //Add Event Handlder Mapping
                            if (tabItem.Content is CustomizedAddressedWebView2 addrWW)
                            {
                                addrWW.OnTitleChanged += CustomizedTabWebCtrl_OnTitleChanged;
                            }

                            if (IsChromeStyled == true)
                            {
                                tabItem.Style = (Style)this.FindResource("xCustomizedChromeTabItemStyle");
                            }

                            Items.Insert(nCurIdx, tabItem);
                            _tabControl.ItemsSource = Items;
                            ControlUtil.UpdateAllControlsVisual(this);
                        }
                    }
                }
            }
#endif
        }

        public /*async*/ void InitBrowser()
        {
            string sFileSavePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string sFileTempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "temp\\";
            Microsoft.Win32.RegistryKey? registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\zdhe\\batchdownload\\1.0", false);
            if (registryKey != null)
            {
                sFileSavePath = (registryKey?.GetValue("FileSavePath") as string) ?? sFileSavePath;
                sFileTempPath = (registryKey?.GetValue("FileTempPath") as string) ?? sFileTempPath;
                registryKey?.Close();
            }
        }
      
        public void AddTabItem(string header, string strUrl, int nIdx=-1)
        {
            WebVWTabItem tabItem = new WebVWTabItem
            {
                //Style= (Style)this.FindResource("xCustomizedChromeTabItemStyle"),
                Header = header,
                Name = $"Tab{Items.Count+1}",
                Content = (header.Trim()=="+")?null:(new CustomizedAddressedWebView2() {
                    Style = (Style)this.FindResource("xCustomizedAddressedWebView2Style"),
                }) // 可以自定义内容
            };

            //Add Event Handlder Mapping
            if(tabItem.Content is CustomizedAddressedWebView2 addrWW && header.Trim() != "+")
            {
                addrWW.OnTitleChanged += CustomizedTabWebCtrl_OnTitleChanged;
            }
            
            if (IsChromeStyled == true ) {
                tabItem.Style = (Style)this.FindResource("xCustomizedChromeTabItemStyle");
            }

            //(tabItem.Content as CustomizedAddressedWebView2).Style = (Style)this.FindResource("xCustomizedAddressedWebView2Style");
            if(nIdx==-1)
                this.Items.Add(tabItem);
            else
                this.Items.Insert(nIdx, tabItem);
        }

        private void CustomizedTabWebCtrl_OnTitleChanged(object sender, GcjRoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void CustomizedTabWebCtrl_OnTitleChanged Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
            CustomizedAddressedWebView2? webViewTab = sender as CustomizedAddressedWebView2;
            Debug.Assert(webViewTab != null);
            if (webViewTab != null) {
                WebVWTabItem? itemTab = webViewTab?.Parent as WebVWTabItem;
                Debug.Assert(webViewTab != null && _tabControl != null);
                if (itemTab != null && _tabControl != null)
                {
                    string sTitle = webViewTab.WebViewCtrl.CoreWebView2.DocumentTitle.Trim();
                    itemTab.Header = sTitle.Length > 20? (sTitle.Substring(0, 20) + "..."): sTitle;
                }
            }
        }
    }

    public class WebVWTabItem : TabItem, INotifyPropertyChanged
    {
        public WebVWTabItem() : base()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
