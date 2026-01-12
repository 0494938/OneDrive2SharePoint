using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GcjUiCtrl.Control
{
    /*******************************************************************
   * 
   * .Net implemteaiton  of TextBox , TextBoxBase https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/Primitives/TextBoxBase.cs,52a329d42544a8fc
   * 
   * 
   * 正浮点数用下面方法：
   * //正浮点数    

      private void tbTest_PreviewTextInput(object sender,TextCompositionEventArgs e)
      {
              //匹配只能输入一个小数点的浮点数
               Regex numbeRegex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
                  e.Handled =
                      !numbeRegex.IsMatch(
                          tbTest.Text.Insert(
                              tbTest.SelectionStart, e.Text));
                  tbTest.Text = tbTest.Text.Trim();
      }

   * 
   * 正整数用下面方法：
      //正整数  
      private void tbTest_PreviewTextInput(object sender,TextCompositionEventArgs e)
      {
                  Regex re = new Regex("[^0-9.-]+");
                  e.Handled = re.IsMatch(e.Text);
      }
   * 
   * 禁用文本框中输入法如下：
   * 
        <TextBox Name="tb_Test" InputMethod.IsInputMethodEnabled="False"/>


   */

    public partial class EncodingTextBox : System.Windows.Controls.TextBox
    {
        #region GcjVer
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(EncodingTextBox),
            new PropertyMetadata("Gcj Encoding TextBox v1.0"));


        [Description("Gets or sets the EncodingTextBox Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        public EncodingTextBox() : base()
        {
            //DefaultStyleKey = typeof(EncodingTextBox);

            this.Loaded += EncodingTextBox_Loaded;
            this.PreviewMouseRightButtonDown += OnMouseRightButtonDown;
            this.PreviewMouseLeftButtonDown += OnMouseRightButtonUp;
            this.ContextMenuOpening += EncodingTextBox_ContextMenuOpening;
            this.GotFocus += OnGetFocus;
            this.DragOver += EncodingTextBox_DragOver; ;
            this.DragEnter += EncodingTextBox_DragEnter; ;
            this.Drop += EncodingTextBox_Drop; ;
        }

        private void EncodingTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void EncodingTextBox_Loaded Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
        }

        private void EncodingTextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            EncodingTextBox? menuOwner = (sender as EncodingTextBox);
            ContextMenu? popUpMenu = menuOwner?.ContextMenu;
            if (popUpMenu != null)
                popUpMenu.Tag = this;
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("OnMouseRightButtonDown ");
        }
        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("OnMouseRightButtonUp ");
        }

        private void OnGetFocus(object sender, System.EventArgs e)
        {
            //_lastActiveTextBox = this;
            Debug.WriteLine("OnGetFocus ");
            //this.Background = null;
        }

        #region DropFileSupport
        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(EncodingTextBox),
            new PropertyMetadata(null));

        [Description("Gets or sets the EncodingTextBox Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath


        #region OnFileDropped Event
        private void EncodingTextBox_DragOver(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void EncodingTextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Allow copy action
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                // No valid data - disable drag effect
                e.Effects = DragDropEffects.None;
            }
        }

        private void EncodingTextBox_Drop(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the file paths from the dropped data
                string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                DropFilePath = droppedFiles.FirstOrDefault();

                RaiseOnFileDropped(e);
            }
        }

        // 注册一个路由事件
        public static readonly RoutedEvent OnFileDroppedEvent = EventManager.RegisterRoutedEvent(
            "OnFileDropped", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(GcjDragEventHandler), // 事件处理程序的类型
            typeof(EncodingTextBox)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized Encoding Edit Control Drop File Event Handler")]
        [Category("GcjControl")]
        public event GcjDragEventHandler OnFileDropped
        {
            add { AddHandler(OnFileDroppedEvent, value); }
            remove { RemoveHandler(OnFileDroppedEvent, value); }
        }
        protected void RaiseOnFileDropped(DragEventArgs e)
        {
            GcjDragEventArgs newArgs = new GcjDragEventArgs()
            {
                RoutedEvent = OnFileDroppedEvent,
                DropFiles = (string[])e.Data.GetData(DataFormats.FileDrop)

            };
            RaiseEvent(newArgs);
        }

        #endregion OnFileDropped Event
        #endregion DropFileSupport

    }

}
