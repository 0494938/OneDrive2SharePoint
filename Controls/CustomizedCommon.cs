using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GcjUiCtrl.Control
{
    //
    // 概要:
    //     Contains arguments relevant to all drag-and-drop events (System.Windows.DragDrop.DragEnter,
    //     System.Windows.DragDrop.DragLeave, System.Windows.DragDrop.DragOver, and System.Windows.DragDrop.Drop).
    public sealed class GcjDragEventArgs : RoutedEventArgs
    {
        public string[] DropFiles { get; set; }
    }

    public delegate void GcjDragEventHandler(object sender, GcjDragEventArgs e);


    public sealed class GcjRoutedEventArgs : RoutedEventArgs
    {
        public object  RntEvtArg { get; set; }
    }

    public delegate void GcjWebView2EventHandler(object sender, GcjRoutedEventArgs e);

    public static class GcjConst
    {
        public static int nDbgSequence = 0;
    }
}
