// ==============================================================================
// 
// Author: Robert Howell
// Date: 8/9/2026
// Version: 1.2
//
// ==============================================================================

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SpaceFlight_News_App.Background
{
    public class LocalProcessInfo
    {
        public Process proc { get; } = Process.GetCurrentProcess();
        public Thread thread { get; } = Thread.CurrentThread;
        public uint? osThreadId { get; } = GetCurrentThreadId();
        public static Process SystemProcess() => Process.GetCurrentProcess();
        public static Thread GetCurrentThread() => Thread.CurrentThread;

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();
    };
}
