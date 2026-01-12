using GcjDiff.DiffBuilder.Model;
using GcjDiff.Wpf.Controls;
using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using UserControl = System.Windows.Controls.UserControl;

namespace GcjUiCtrl.Control
{
    /// <summary>
    /// CustomizedGcjDiffViewerUserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomizedGcjDiffViewerUserControl : UserControl
    {
        public CustomizedGcjDiffViewerUserControl()
        {
            InitializeComponent();
            diffViewer.DragOver += DiffCtrl_DragOver;
            diffViewer.DragEnter += DiffCtrl_DragEnter;
            diffViewer.Drop += DiffCtrl_DropFile;
            this.Loaded += CustomizedGcjDiffViewerUserControl_Loaded;
        }

        //DiffPlex.Wpf.Controls.InternalLinesViewer? leftView=null
        System.Windows.Controls.Control? leftViewer = null;
        System.Windows.Controls.Control? rightViewer = null;

        public void SetHeaderAsOldToNew()
        {
            diffViewer.SetHeaderAsOldToNew();
        }

        public void SetHeaderAsLeftToRight()
        {
            diffViewer.SetHeaderAsLeftToRight();
        }

        private void CustomizedGcjDiffViewerUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void CustomizedGcjDiffViewerUserControl_Loaded(object sender, RoutedEventArgs e)\r\n Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");

            //leftViewer = Template?.FindName("LeftContentPanel", this) as System.Windows.Controls.Control;
            //rightViewer = Template?.FindName("RightContentPanel", this) as System.Windows.Controls.Control;

            List <UserControl> userCtrls = DepUtil.GetAllChildrenOfType<UserControl>(this);
            leftViewer = userCtrls.Where(x => x.Name == "LeftContentPanel").FirstOrDefault();
            rightViewer = userCtrls.Where(x => x.Name == "RightContentPanel").FirstOrDefault();
        }

        private void DiffCtrl_DropFile(object sender, DragEventArgs e)
        {
            if (diffViewer.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the file paths from the dropped data
                RaiseOnFileDropped(e);
            }
        }

        private void DiffCtrl_DragEnter(object sender, DragEventArgs e)
        {
            if (diffViewer.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
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

        private void DiffCtrl_DragOver(object sender, DragEventArgs e)
        {
            if (diffViewer.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        #region OnFileDropped Event
        // 注册一个路由事件
        public static readonly RoutedEvent OnFileDroppedEvent = EventManager.RegisterRoutedEvent(
            "OnFileDropped", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(GcjDragEventHandler), // 事件处理程序的类型
            typeof(CustomizedButton)); // 注册事件的类

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

        #region Properties
        
        #region DiffModel

        public static readonly DependencyProperty DiffModelProperty =
            DependencyProperty.Register("DiffModel", typeof(SideBySideDiffModel), typeof(CustomizedGcjDiffViewerUserControl), new FrameworkPropertyMetadata(null, null));

        [Bindable(true)]
        [Category("Appearance")]
        public SideBySideDiffModel DiffModel
        {
            get
            {
                return (SideBySideDiffModel)GetValue(DiffModelProperty);
            }
            set
            {
                SetValue(DiffModelProperty, value);
            }
        }

        #endregion DiffModel

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedGcjDiffViewerUserControl),
            new PropertyMetadata("Gcj CheckBox v1.0"));


        [Description("Gets or sets the CustomizedGcjDiffViewerUserControl Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region OldText         // 定义 OldText 依赖属性
        public static readonly DependencyProperty _OldText = DependencyProperty.Register(
            "OldText",
            typeof(string),
            typeof(CustomizedGcjDiffViewerUserControl),
            new PropertyMetadata("Gcj CheckBox v1.0"));


        [Description("Gets or sets the CustomizedGcjDiffViewerUserControl OldText")]
        [Category("GcjControl")]
        public string OldText
        {
            get { return (string)GetValue(_OldText); }
            set { SetValue(_OldText, value); }
        }
        #endregion OldText

        #region NewText         // 定义 NewText 依赖属性
        public static readonly DependencyProperty _NewText = DependencyProperty.Register(
            "NewText",
            typeof(string),
            typeof(CustomizedGcjDiffViewerUserControl),
            new PropertyMetadata("Gcj CheckBox v1.0"));


        [Description("Gets or sets the CustomizedGcjDiffViewerUserControl NewText")]
        [Category("GcjControl")]
        public string NewText
        {
            get { return (string)GetValue(_NewText); }
            set { SetValue(_NewText, value); }
        }
        #endregion NewText
        
        #endregion Properties

        #region Methods
        public void SetText(string left, string right)
        {
            diffViewer.SetText(left, right);
        }

        public void SetOldText(string value, string header = null)
        {
            diffViewer.SetOldText(value, header);
        }

        public void SetNewText(string value, string header = null)
        {
            diffViewer.SetNewText(value, header);
        }

        public async Task SetFiles(FileInfo oldFile, FileInfo newFile)
        {
            diffViewer.SetFiles(oldFile, newFile);
        }
        public void ShowOpenFileContextMenu()
        {
            diffViewer.ShowOpenFileContextMenu();
        }

        public void OpenFileOnBoth()
        {
            diffViewer.OpenFileOnBoth();
        }

        public void OpenFileOnLeft()
        => diffViewer.OpenFileOnLeft();

        public void OpenFileOnRight()
            => diffViewer.OpenFileOnRight();

        public string OpenFileOnLeft(string header, out FileInfo file)
        {
            return diffViewer.OpenFileOnLeft(header, out file);
        }

        public string OpenFileOnRight(string header, out FileInfo file)
        {
            return diffViewer.OpenFileOnRight(header, out file);
        }


        public SideBySideDiffModel GetSideBySideDiffModel()
        {
            return diffViewer.GetSideBySideDiffModel();
        }

        /// <summary>
        /// Gets the inline diffs result.
        /// </summary>
        public DiffPaneModel GetInlineDiffModel()
        {
            return diffViewer.GetInlineDiffModel();
        }

        /// <summary>
        /// Refreshes.
        /// </summary>
        public void Refresh()
        {
            diffViewer.Refresh();
        }

        /// <summary>
        /// Switches to the view of side-by-side diff mode.
        /// </summary>
        public void ShowSideBySide()
        {
            diffViewer.ShowSideBySide();
        }

        /// <summary>
        /// Switches to the view of inline diff mode.
        /// </summary>
        public void ShowInline()
        {
            diffViewer.ShowInline();    
        }

        public bool GoTo(int lineIndex, bool isLeftLine = false)
        {
            return diffViewer.GoTo(lineIndex, isLeftLine);
        }

        public bool GoTo(DiffPiece line, bool isLeftLine = false)
        {
            return diffViewer.GoTo(line, isLeftLine);
        }

        public DiffPiece GetLine(int lineIndex, bool isLeftLine = false)
        {
           return diffViewer.GetLine(lineIndex, isLeftLine);
        }

        public IEnumerable<DiffPiece> GetLinesInViewport(bool isLeftLine = false, VisibilityLevels level = VisibilityLevels.Any)
        {
            return diffViewer.GetLinesInViewport(isLeftLine, level);
        }

        public IEnumerable<DiffPiece> GetLinesInViewport(VisibilityLevels level)
        {
            return diffViewer.GetLinesInViewport(level);
        }

        public IEnumerable<DiffPiece> GetLinesBeforeViewport(bool isLeftLine = false, VisibilityLevels level = VisibilityLevels.Any)
        {
            return diffViewer.GetLinesBeforeViewport(isLeftLine, level);
        }

        public IEnumerable<DiffPiece> GetLinesBeforeViewport(VisibilityLevels level)
        {
            return diffViewer.GetLinesBeforeViewport(level);
        }

        public IEnumerable<DiffPiece> GetLinesAfterViewport(bool isLeftLine = false, VisibilityLevels level = VisibilityLevels.Any)
        {
            return diffViewer.GetLinesAfterViewport(isLeftLine, level);
        }

        public IEnumerable<DiffPiece> GetLinesAfterViewport(VisibilityLevels level)
        {
            return diffViewer.GetLinesAfterViewport(level);
        }

        public void OpenViewModeContextMenu()
        {
            diffViewer.OpenViewModeContextMenu();
        }

        public void CollapseUnchangedSections(int? contextLineCount = null)
        {
            diffViewer.CollapseUnchangedSections(contextLineCount);
        }

        public void ExpandUnchangedSections()
        {
            diffViewer.ExpandUnchangedSections();
        }

        public void SetMenuButtonStyle(Style style)
        {
            diffViewer.SetMenuButtonStyle(style);
        }

        public void SetMenuButtonTemplate(ControlTemplate template)
        {
            diffViewer.SetMenuButtonTemplate(template);
        }

        public void SetMenuTextBoxStyle(Style style)
        {
            diffViewer.SetMenuTextBoxStyle(style);
        }

        public void SetMenuTextBoxTemlate(ControlTemplate template)
        {
            diffViewer.SetMenuTextBoxTemlate((ControlTemplate)template);
        }

        public DiffPiece PreviousDiff()
        {
            return diffViewer.PreviousDiff();
        }

        public DiffPiece NextDiff()
        {
           return  diffViewer.NextDiff();
        }

        #endregion Methods
    }
}
