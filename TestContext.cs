using BaseUtil;
using GcjUiCtrl.Control;
using GcjUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IsidTestApp
{

    public partial class TestContext : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region CustomizedBgColor
        //System.Windows.Media.Color __customizedBgColor = Colors.BlueViolet;
        public static readonly DependencyProperty __customizedBgColor = DependencyProperty.Register(
            "CustomizedBgColor",
            typeof(System.Windows.Media.Color),
            typeof(TestContext),
            new PropertyMetadata(Colors.BlueViolet));

        public System.Windows.Media.Color CustomizedBgColor
        {
            get { return (System.Windows.Media.Color)GetValue(__customizedBgColor); }
            set
            {
                SetValue(__customizedBgColor, value);
                SetValue(__customizedBgBrush, new SolidColorBrush(value));
                SetValue(__customizedFgColor, CtrlUtil.GetWhiteBlackForeGroundColor(value));
                SetValue(__customizedFgBrush, new SolidColorBrush((System.Windows.Media.Color)GetValue(__customizedFgColor)));
            }
        }
        #endregion CustomizedBgColor

        #region CustomizedFgColor
        //System.Windows.Media.Color __customizedFgColor = Colors.White;
        public static readonly DependencyProperty __customizedFgColor = DependencyProperty.Register(
            "CustomizedFgColor",
            typeof(System.Windows.Media.Color),
            typeof(TestContext),
            new PropertyMetadata(Colors.White));
        public System.Windows.Media.Color CustomizedFgColor
        {
            get
            {
#if true
                // 根据加权的RGB分量计算感知亮度（YIQ公式）
                return CtrlUtil.GetWhiteBlackForeGroundColor((System.Windows.Media.Color)GetValue(__customizedFgColor));
#else
                return ColorHelper.FindBestContrastColor4((System.Windows.Media.Color)GetValue(__customizedBgColor));
                
#endif
            }
            set
            {
                SetValue(__customizedFgColor, value);
                SetValue(__customizedFgBrush, new SolidColorBrush(value));
            }
        }
        #endregion CustomizedBgColor

        #region CustomizedBgBrush
        //System.Windows.Media.Brush? __customizedBgBrush = null;// new SolidColorBrush(__customizedBgColor);
        public static readonly DependencyProperty __customizedBgBrush = DependencyProperty.Register(
            "CustomizedBgBrush",
            typeof(System.Windows.Media.Brush),
            typeof(TestContext),
            new PropertyMetadata(GcjBaseWindow.DefaultBgBrush));

        public System.Windows.Media.Brush CustomizedBgBrush
        {
            get
            {
                return (System.Windows.Media.Brush)GetValue(__customizedBgBrush);
            }
            set
            {
                //__customizedBgBrush = value;
                SetValue(__customizedBgBrush, value);
            }
        }
        #endregion CustomizedBgBrush


        #region CustomizedRoundEditBrush
        public static readonly DependencyProperty __customizedRoundEditBrush = DependencyProperty.Register(
            "CustomizedRoundEditBrush",
            typeof(System.Windows.Media.Brush),
            typeof(TestContext),
            new PropertyMetadata(GcjBaseWindow.DefaultRoundEditBgBrush));

        public System.Windows.Media.Brush CustomizedRoundEditBrush
        {
            get
            {
                return (System.Windows.Media.Brush)GetValue(__customizedRoundEditBrush);
            }
            set
            {
                SetValue(__customizedRoundEditBrush, value);
            }
        }
        #endregion CustomizedRoundEditBrush

        #region CustomizedFgBrush
        //System.Windows.Media.Brush? __customizedFgBrush = null;// new SolidColorBrush(__customizedBgColor);
        public static readonly DependencyProperty __customizedFgBrush = DependencyProperty.Register(
         "CustomizedFgBrush",
         typeof(System.Windows.Media.Brush),
         typeof(TestContext),
         new PropertyMetadata(GcjBaseWindow.DefaultFgBrush));
        public System.Windows.Media.Brush CustomizedFgBrush
        {
            get
            {
                //if (__customizedFgBrush == null)
                //    __customizedFgBrush = new SolidColorBrush(CustomizedFgColor);
                //return __customizedFgBrush;
                return (System.Windows.Media.Brush)GetValue(__customizedFgBrush);
            }
            set
            {
                //__customizedBgBrush = value;
                SetValue(__customizedFgBrush, value);
            }
        }
        #endregion CustomizedFgBrush

        bool __bFileFrameSelected =
#if DEBUG
            true;
#else
            true;
#endif
        public bool IsFileFrameSelected { get { return __bFileFrameSelected; } set { __bFileFrameSelected = value; } }
        public bool IsSystemFrameSelected { get { return !__bFileFrameSelected; } set { __bFileFrameSelected = !value; } }

        bool __bFileRecentShowAll = false;
        public bool FileRecentShowAll { get { return __bFileRecentShowAll; } set { __bFileRecentShowAll = value; } }
        bool __bFileSharedShowAll = false;
        public bool FileSharedShowAll { get { return __bFileSharedShowAll; } set { __bFileSharedShowAll = value; } }

        #region ReleaseMode // 定义 ReleaseMode 依赖属性
        public bool ReleaseMode
        {
            get
            {
                //return (bool)GetValue(ReleaseModeProperty);
#if DEBUG
                return false;
#else
                return true;
#endif
            }
        }
        #endregion ReleaseMode

        #region SystemMapBrowserMode // 定义 ReleaseMode 依赖属性
        bool __SystemMapBrowserMode = false;
        public bool SystemMapBrowserMode
        {
            get { return __SystemMapBrowserMode; }
            set { __SystemMapBrowserMode = value; }
        }
        #endregion SystemMapBrowserMode

        #region Opacity
        [Description("Gets or sets the Opacity")]
        [Category("GcjControl")]
        public double Opacity
        {
#if DEBUG
            get { return 1;}
#else
            get { return 1;}
#endif
        }
        #endregion Opacity  
    }

}
