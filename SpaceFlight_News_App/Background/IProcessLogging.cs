// ==============================================================================
// 
// Author: Robert Howell
// Date: 8/9/2026
// Version: 1.0
//
// ==============================================================================

namespace SpaceFlight_News_App.Background
{
    internal class IProcessLogging
    {
        protected static void WriteConsoleMessage(string message)
        {
            var localProcess = new LocalProcessInfo();
            Console.WriteLine($"ThreadId:{localProcess.thread.Name}-{localProcess.osThreadId}|{message}");
        }

        protected static void WriteConsoleMessage(LocalProcessInfo localProcess, string message)
        {
            var threadName = localProcess.thread.Name is null ?
                localProcess.proc.ProcessName : localProcess.thread.Name;
            Console.WriteLine($"ThreadId:{threadName}-{localProcess.osThreadId}|{message}");
        }
    };
}
