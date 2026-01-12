using GcjUiCtrl.Control;
using GcjUtil;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace IsidTestApp
{
    /// <summary>
    /// TestControlWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TelTestWedgitWindow : GcjBaseWindow
    {
        void HandleRobotInputMode(string sEvtName, string tagName, string name, string value, string key)
        {
            txtFlowTitle.Visibility = Visibility.Collapsed;
            txtFlowDetail.Visibility = Visibility.Collapsed;
            scRobotTitle.Visibility = Visibility.Collapsed;
            txtFlowCategoryTitle.Visibility = Visibility.Collapsed;
            imgRobotReturn.Visibility = Visibility.Visible;
            scRight.UpdateLayout();
        }

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

    }
}
