using System.ComponentModel;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public class GcjBaseWedgtWindow : GcjBaseWindow, INotifyPropertyChanged
    {
        public GcjBaseWedgtWindow() : base()
        {
            //DefaultStyleKey = typeof(GcjBaseWindow);
        
        }

        private void GcjBaseWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //UpdateAllControlsVisual(this,3);
        }

        private void GcjBaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        #region Properties
        #endregion Properties
    }
}
