using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedRoundEdit : System.Windows.Controls.TextBox, INotifyPropertyChanged
    {
        public CustomizedRoundEdit() : base()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizedRoundEdit),
            //   new FrameworkPropertyMetadata(typeof(CustomizedRoundEdit)));

            Loaded += CustomizedRoundEdit_Loaded;
            LostFocus += CustomizedRoundEdit_LostFocus;
            GotFocus += CustomizedRoundEdit_GotFocus;
            IsVisibleChanged += CustomizedRoundEdit_IsVisibleChanged;
            IsEnabledChanged += CustomizedRoundEdit_IsEnabledChanged;
            PreviewKeyDown += CustomizedRoundEdit_PreviewKeyDown;
            TextChanged += CustomizedRoundEdit_TextChanged;

            DragEnter += CustomizedRoundEdit_DragEnter;
            DragOver += CustomizedRoundEdit_DragOver;
            Drop += CustomizedRoundEdit_Drop;
        }

        #region EventHandler
        private void CustomizedRoundEdit_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextValue = Text;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnTextValuenPropertyChanged(string propertyName)
        {
        }

        private void CustomizedRoundEdit_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextValue?.Trim()))
                Foreground = PlaceHolderTextColor;
            else
                Foreground =TextColor ;
        }

        private void CustomizedRoundEdit_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextValue?.Trim()))
                Foreground = PlaceHolderTextColor;
            else
                Foreground = TextColor;
        }

        private void CustomizedRoundEdit_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CustomizedRoundEdit::CustomizedRoundEdit_Loaded(" + this.Name + ")");
        }

        private void CustomizedRoundEdit_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Debug.WriteLine($"CustomizedRoundEdit_PreviewKeyDown(sender: {sender?.GetType().Name}, BaseClass:{sender?.GetType().BaseType?.Name}): IntKey:{(int) e.Key}, Key:{e.Key}, Char:{(char)e.Key}, SysKey : IntKey:{(int)e.SystemKey}, Key:{e.SystemKey}, Char:{(char)e.SystemKey}, Ctrl:{e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control)}, Alt:{e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Alt)}, Shift:{e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift)}");
            //CustomizedRoundEdit_PreviewKeyDown: IntKey: 156, Key: System, Char:, SysKey: IntKey: 120, Key: LeftAlt, Char: x, Ctrl: False, Alt: True, Shift: False
            //CustomizedRoundEdit_PreviewKeyDown: IntKey: 156, Key: System, Char:, SysKey: IntKey: 93, Key: F4, Char:], Ctrl: False, Alt: True, Shift: False
            if (e.SystemKey == Key.F4 && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                Window? wnd = DepUtil.FindParent<Window>(this);
                wnd?.Close();
            }
        }

        // 失去焦点时启动滚动
        private void CustomizedRoundEdit_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("CustomizedRoundEdit::CustomizedRoundEdit_IsEnabledChanged(" + this.Name + ") " + e.OldValue + " => " + e.NewValue);
          
        }

        private void CustomizedRoundEdit_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("CustomizedRoundEdit::CustomizedRoundEdit_IsVisibleChanged(" + this.Name + ") " + e.OldValue + " => " + e.NewValue);
        }

        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    base.OnRender(drawingContext);

        //    if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(PlaceholderText))
        //    {
        //        var formattedText = new FormattedText(
        //            PlaceholderText,
        //            CultureInfo.CurrentCulture,
        //            FlowDirection.LeftToRight,
        //            new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
        //            FontSize,
        //            Foreground);

        //        // 计算文本位置
        //        var x = (ActualWidth - formattedText.Width) / 2;
        //        var y = (ActualHeight - formattedText.Height) / 2;

        //        drawingContext.DrawText(formattedText, new Point(x, y));
        //    }
        //}
        #endregion EventHandler

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedRoundEdit),
            new PropertyMetadata("Gcj RoundTextBox v1.0"));


        [Description("Gets or sets the CustomizedRoundEdit Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region CornerRadius         // 定义 CornerRadius 依赖属性
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof(System.Windows.CornerRadius),
            typeof(CustomizedRoundEdit),
            new PropertyMetadata(new CornerRadius(5.0)));

        [Description("Gets or sets the CustomizedRoundEdit CornerRadius")]
        [Category("GcjControl")]
        public System.Windows.CornerRadius CornerRadius
        {
            get { return (System.Windows.CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion CornerRadius

        #region PlaceholderText         // 定义 PlaceholderText 依赖属性
        public static readonly DependencyProperty _PlaceholderText = DependencyProperty.Register(
            "PlaceholderText",
            typeof(string),
            typeof(CustomizedRoundEdit),
            new PropertyMetadata("Input Search Text"));


        [Description("Gets or sets the CustomizedRoundEdit PlaceholderText")]
        [Category("GcjControl")]
        public string PlaceholderText
        {
            get { return (string)GetValue(_PlaceholderText); }
            set { SetValue(_PlaceholderText, value); }
        }
        #endregion PlaceholderText

        #region TextValue         // 定义 TextValue 依赖属性
        public static readonly DependencyProperty _TextValue = DependencyProperty.Register(
            "TextValue",
            typeof(string),
            typeof(CustomizedRoundEdit),
            new PropertyMetadata(""));


        [Description("Gets or sets the CustomizedRoundEdit TextValue")]
        [Category("GcjControl")]
        public string TextValue
        {
            get { return (string)GetValue(_TextValue); }
            set { 
                SetValue(_TextValue, value);
                if (!this.IsFocused)
                {
                    base.Text = TextValue;
                }

            }
        }
        #endregion TextValue

        //#region Text // redefine Text 依赖属性
       
        //[Description("Gets or sets the CustomizedRoundEdit Text(Overided)")]
        //[Category("GcjControl")]
        //public new string Text
        //{
        //    get {
        //        if (string.IsNullOrWhiteSpace(TextValue))
        //            return PlaceholderText;
        //        else
        //            return TextValue;
        //    }
        //    set
        //    {
        //        base.Text = value;
        //    }
        //}
        //#endregion Text

        #region PlaceHolderTextColor         // 定义 PlaceHolderTextColor 依赖属性
        public static readonly DependencyProperty _PlaceHolderTextColor = DependencyProperty.Register(
            "PlaceHolderTextColor",
            typeof(Brush),
            typeof(CustomizedRoundEdit),
            new PropertyMetadata(Brushes.Gray));


        [Description("Gets or sets the CustomizedRoundEdit PlaceHolderTextColor")]
        [Category("GcjControl")]
        public Brush PlaceHolderTextColor
        {
            get { return (Brush)GetValue(_PlaceHolderTextColor); }
            set { SetValue(_PlaceHolderTextColor, value); }
        }
        #endregion PlaceHolderTextColor

        #region TextColor         // 定义 TextColor 依赖属性
        public static readonly DependencyProperty _TextColor = DependencyProperty.Register(
            "TextColor",
            typeof(Brush),
            typeof(CustomizedRoundEdit),
            new PropertyMetadata(Brushes.Black));


        [Description("Gets or sets the CustomizedRoundEdit TextColor")]
        [Category("GcjControl")]
        public Brush TextColor
        {
            get { return (Brush)GetValue(_TextColor); }
            set { SetValue(_TextColor, value); }
        }
        #endregion TextColor

        #endregion Properties

        #region EventHandlers

        #region OnFileDropped Event
        private void CustomizedRoundEdit_DragOver(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void CustomizedRoundEdit_DragEnter(object sender, DragEventArgs e)
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

        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(CustomizedRoundEdit),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedRoundEdit Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath

        private void CustomizedRoundEdit_Drop(object sender, DragEventArgs e)
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
            typeof(CustomizedRoundEdit)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized Button Control Drop File Event Handler")]
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

        #endregion EventHandlers
    }
}
