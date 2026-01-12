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

        private void btnIsidTestControlWindow_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new IsidTestControlWindow();
            this.Hide();
            wnd.ShowDialog();
            this.Show();
            this.Activate();
        }

        private void IsidTestControlWindowBak1_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new IsidTestControlWindowBak1();
            this.Hide();
            wnd.ShowDialog();
            this.Show();
            this.Activate();
        }

        private void btnOrgTelTestWedgitWindow_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new OrgTelTestWedgitWindow();
            this.Hide();
            wnd.ShowDialog();
            this.Show();
            this.Activate();
        }

        private void btnTemp1OrgTelTestWedgitWindow_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new Temp1OrgTelTestWedgitWindow();
            this.Hide();
            wnd.ShowDialog();
            this.Show();
            this.Activate();
        }

        private void btnTelTestWedgitWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var wnd = new TelTestWedgitWindow();
            wnd.ShowDialog();
            this.Show();
            this.Activate();
        }

        private void btnTelTestSearchWedgit_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new TelTestSearchWedgit();
            this.Hide();
            wnd.ShowDialog();
            this.Show();
            this.Activate();
        }

        
        private void btnTelTestShadowWindow_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new IsidTestShadowWindow();
            this.Hide();
            wnd.ShowDialog();
            this.Show();
            this.Activate();
            
        }

    }
}
