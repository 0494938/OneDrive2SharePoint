using BaseUtil;
using IsidUsedCtrl;
using IsidUsedCtrl.Controls;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using Application = System.Windows.Application;
using MSG = System.Windows.Interop.MSG;

namespace IsidTestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        void app_Startup(object sender, StartupEventArgs e)
        {
            //int hr = GcjWinApi.CoInitializeEx(IntPtr.Zero, CoInit.COINIT_APARTMENTTHREADED);
            int hr = GcjWinApi.CoInitialize(IntPtr.Zero);

            if (hr == 0) // S_OK
            {
                Trace.WriteLine("COM initialized successfully.");
                // 在此处使用 COM 对象
            }
            else if (hr == 1) // S_FALSE
            {
                Debug.Assert(true, "COM already initialized, Ignore Error");
            }
            else
            {
                Debug.Assert(false, "Failed to initialize COM.");
                // 错误处理
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);

#if false
            new MessageHooking().HookAllThreadInCurrentProcess();
#endif

#if false
            var mainWindow = new TelTestWebView2GrayBG();
            mainWindow.Show();
#else
#if true
#if true
            var mainWindow = new MainMenu();
            mainWindow.Show();
#else
            var mainWindow = new TestAnimationCtrl();
            mainWindow.Show();
            
            #endif
#else
            // 创建第一个窗口
            //var mainWindow = new DynamicXamlEditor3Test();
            //var mainWindow = new IsidTestControlWindow();
            //var mainWindow = new OrgTelTestWedgitWindow();
            //var mainWindow = new Temp1OrgTelTestWedgitWindow();
            var mainWindow = new TelTestWedgitWindow();
            mainWindow.Show();
#endif
#endif
        }


    }
}
