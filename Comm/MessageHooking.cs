using BaseUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IsidUsedCtrl.Controls
{
    public class MessageHooking
    {
        private IntPtr _hookGetMsgID = IntPtr.Zero;
        private LowLevelProc _GetMsgHookProc;
        private IntPtr _hookCallWndID = IntPtr.Zero;
        private LowLevelProc _CallWndProc;
        private WinEventDelegate _winEventDelegate;
        private IntPtr _hookEventHandle = IntPtr.Zero;
        private static HashSet<int> _hookedThreads = new HashSet<int>();

        private IntPtr SetCallWndHook(LowLevelProc proc, uint dwThreadId)
        {
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule module = process.MainModule)
            {
                return GcjWinApi.SetWindowsHookEx(GcjWinApi.WH_CALLWNDPROC, proc, GcjWinApi.GetModuleHandle(module.ModuleName), dwThreadId /*(uint)GcjWinApi.GetCurrentThreadId()*/);
            }
        }

        private IntPtr SetGetMsgHook(LowLevelProc proc, uint dwThreadId)
        {
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule module = process.MainModule)
            {
                return GcjWinApi.SetWindowsHookEx(GcjWinApi.WH_GETMESSAGE, proc, GcjWinApi.GetModuleHandle(module.ModuleName), dwThreadId);
            }
        }

        private IntPtr HookGetCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSG msg = (MSG)Marshal.PtrToStructure(lParam, typeof(MSG));
                //Debug.WriteLine($"Message: {msg.message}, From: {msg.hWnd}");
                WinMsgUtil.TraceMessageDetail("GetMsg Hook", msg.hWnd, (uint)msg.message, msg.wParam, msg.lParam);
            }
            return GcjWinApi.CallNextHookEx(_hookGetMsgID, nCode, wParam, lParam);
        }

        private IntPtr HookCallWndCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                CWPSTRUCT msg = (CWPSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPSTRUCT));
                //Debug.WriteLine($"Message: {msg.message}, From hwnd: {msg.hwnd}, wParam: {msg.wParam}, lParam: {msg.lParam}");
                WinMsgUtil.TraceMessageDetail("CallWnd Hook", msg.hwnd, (uint)msg.message, msg.wParam, msg.lParam);
            }
            return GcjWinApi.CallNextHookEx(_hookCallWndID, nCode, wParam, lParam);
        }

        private void WinEventCallback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // Filter events to get only top-level window creation events
            if (eventType == GcjWinApi.EVENT_OBJECT_CREATE)
            {
                if (idObject == GcjWinApi.OBJID_WINDOW)
                {
                    // Get the process name of the created window
                    uint processId;
                    int threadId = (int)GcjWinApi.GetWindowThreadProcessId(hwnd, out processId);
                    var process = Process.GetProcessById((int)processId);

                    if (!_hookedThreads.Contains(threadId) && processId == Process.GetCurrentProcess().Id)
                    {
                        // 新线程，添加钩子
                        Trace.WriteLine($"Event Monitor: Window created by process: {process.ProcessName}, hwnd: {hwnd}");
                        IntPtr hook = SetGetMsgHook(_GetMsgHookProc, (uint)threadId);
                        if (hook != IntPtr.Zero)
                        {
                            _hookedThreads.Add(threadId);
                            Trace.WriteLine($"WinEventCallback: GetMsg Hook added to new thread {threadId}");
                        }
                        hook = SetCallWndHook(_CallWndProc, (uint)threadId);
                        if (hook != IntPtr.Zero)
                        {
                            _hookedThreads.Add(threadId);
                            Trace.WriteLine($"WinEventCallback: CallWnd Hook added to new thread {threadId}");
                        }
                        // Display a message in the console
                    }
                }
            }
        }

        public void HookAllThreadInCurrentProcess()
        {
            // Initialize the delegate to prevent it from being garbage collected
            _GetMsgHookProc = HookGetCallback;
            _CallWndProc = HookCallWndCallback;
            _winEventDelegate = new WinEventDelegate(WinEventCallback);

            // Set the WinEventHook for window creation and destruction
            _hookEventHandle = GcjWinApi.SetWinEventHook(GcjWinApi.EVENT_OBJECT_CREATE, GcjWinApi.EVENT_OBJECT_DESTROY, IntPtr.Zero, _winEventDelegate, 0, 0, GcjWinApi.WINEVENT_OUTOFCONTEXT);

            // 设置钩子到当前线程
            //_hookGetMsgID = SetGetMsgHook(_GetMsgHookProc, (uint)GcjWinApi.GetCurrentThreadId());
            //_hookCallWndID = SetCallWndHook(_CallWndProc, (uint)GcjWinApi.GetCurrentThreadId()); // 设置 WH_CALLWNDPROC 钩子

            foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
            {
                int threadId = thread.Id;
                if (!_hookedThreads.Contains(threadId))
                {
                    // 新线程，添加钩子
                    IntPtr hook = SetGetMsgHook(_GetMsgHookProc, (uint)threadId);
                    if (hook != IntPtr.Zero)
                    {
                        _hookedThreads.Add(threadId);
                        Trace.WriteLine($"Enum: GetMsg Hook added to new thread {threadId}");
                    }
                    hook = SetCallWndHook(_CallWndProc, (uint)threadId);
                    if (hook != IntPtr.Zero)
                    {
                        _hookedThreads.Add(threadId);
                        Trace.WriteLine($"Enum: CallWnd Hook added to new thread {threadId}");
                    }
                }
            }

        }
    }

}
