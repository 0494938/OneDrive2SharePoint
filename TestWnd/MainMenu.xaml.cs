using GcjUiCtrl.Control;
using IsidTestApp;
using System.Windows;
using Application = System.Windows.Application;

namespace IsidUsedCtrl
{
    /// <summary>
    /// MainMenu.xaml の相互作用ロジック
    /// </summary>
    public partial class MainMenu : GcjBaseWindow
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        private void btnTestAnimationCtrl_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new TestAnimationCtrl();
            this.Hide();
            wnd.ShowDialog();
            this.Show();
            this.Activate();
        }

        private void btnMigrateOneDriveToSP_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new MitsubishiTMigrateDataToSP();
            this.Hide();
            wnd.ShowDialog();
            this.Show();
            this.Activate();
        }
    }
}
