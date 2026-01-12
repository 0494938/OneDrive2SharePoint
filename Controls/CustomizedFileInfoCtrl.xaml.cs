using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace GcjUiCtrl.Control
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomizedFileInfoCtrl : System.Windows.Controls.UserControl
    {
        public CustomizedFileInfoCtrl()
        {
            InitializeComponent();
        }

        #region HasValue // 定义 HasValue 依赖属性
        //public static readonly DependencyProperty HasValue =
        //DependencyProperty.Register("HasValue", typeof(bool), typeof(CustomizedFileInfoCtrl), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedFileInfoCtrl HasValue")]
        [Category("GcjControl")]
        public bool HasValue
        {
            get {
                /* FilePath: Local file name or File Url in sharepoint , box etc. assume you can get file source (box/sharepoint etc ) from file name. 
                 * FileNameDesc: file description include box/share point, directory, , description. get from FilePath or manual set
                 * ThumbnailImageSource: Thumbnail Image Path, you can get from file/network or manual set
                 */
                return !string.IsNullOrEmpty(FilePath) || !string.IsNullOrEmpty(FileNameDesc) || ThumbnailImageSource != null;
            }
        }
        #endregion HasValue
     
        #region GcjVer  // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedFileInfoCtrl),
            new PropertyMetadata("Gcj File Info Control v1.0"));


        [Description("Gets or sets the CustomizedFileInfoCtrl Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region ImageSource // 定义 ImageSource 依赖属性
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(CustomizedFileInfoCtrl), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedFileInfoCtrl ImageSource")]
        [Category("GcjControl")]
        public ImageSource ThumbnailImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        #endregion ImageSource

        #region FilePath // 定义 FilePath 依赖属性
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(CustomizedFileInfoCtrl), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedFileInfoCtrl FilePath")]
        [Category("GcjControl")]
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }
        #endregion FilePath

        #region UserImageSource // 定义 UserImageSource 依赖属性
        public static readonly DependencyProperty UserImageSourceProperty =
            DependencyProperty.Register("UserImageSource", typeof(ImageSource), typeof(CustomizedFileInfoCtrl), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedFileInfoCtrl UserImageSource")]
        [Category("GcjControl")]
        public ImageSource UserImageSource
        {
            get { return (ImageSource)GetValue(UserImageSourceProperty); }
            set { SetValue(UserImageSourceProperty, value); }
        }
        #endregion UserImageSource

        #region FileNameDesc // 定义 FileNameDesc 依赖属性
        public static readonly DependencyProperty FileNameDescProperty =
            DependencyProperty.Register("FileNameDesc", typeof(string), typeof(CustomizedFileInfoCtrl), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedFileInfoCtrl FileNameDesc")]
        [Category("GcjControl")]
        public string FileNameDesc
        {
            get { return (string)GetValue(FileNameDescProperty); }
            set { SetValue(FileNameDescProperty, value); }
        }
        #endregion FileNameDesc    

        #region UserInfo  // 定义 UserInfo 依赖属性
        public static readonly DependencyProperty UserInfoProperty =
            DependencyProperty.Register("UserInfo", typeof(string), typeof(CustomizedFileInfoCtrl), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedFileInfoCtrl UserInfo")]
        [Category("GcjControl")]
        public string UserInfo
        {
            get { return (string)GetValue(UserInfoProperty); }
            set { SetValue(UserInfoProperty, value); }
        }
        #endregion UserInfo
        
        #region ItemIndex  // 定义 ItemIndex 依赖属性
        public static readonly DependencyProperty ItemIndexProperty =
            DependencyProperty.Register("ItemIndex", typeof(int), typeof(CustomizedFileInfoCtrl), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedFileInfoCtrl ItemIndex")]
        [Category("GcjControl")]
        public int ItemIndex
        {
            get { return (int)GetValue(ItemIndexProperty); }
            set { SetValue(ItemIndexProperty, value); }
        }
        #endregion ItemIndex
    }
}
