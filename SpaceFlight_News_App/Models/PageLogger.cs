// ==============================================================================
// Filename: PageLogger.cs
// 
// Author: Robert Howell
// Date: 1/22/2025
// Edited: 6/22/2026
// Version: 1.2
//
// Description: This file creates a helper object that writes log entries.
//
// ==============================================================================

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SpaceFlight_News_App.Models
{
    public static class PageLogger
    {
        public static string WritePageLog(PageModel pageModel)
        {
            var pageLog =    $"(M/D/YYYY H:M:S){DateTime.Now.ToShortDateString()} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}" +
                                $" - {pageModel.Request.HttpContext.Connection.RemoteIpAddress} - {pageModel.Request.Method} {pageModel.Request.Scheme}://{pageModel.Request.Host}{pageModel.Request.Path} {pageModel.Request.Protocol};" +
                                $"StatusCode:{pageModel.Response.StatusCode};\n";
            //C:\inetpub\logs\LogFiles\Logger
            using var privacyPageLog = new CreateLogFile(@"C:\inetpub\logs\LogFiles\Logger\", @$"C:\inetpub\logs\LogFiles\Logger\{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-pageusage.txt", true);

            privacyPageLog.WriteLog(pageLog);

            return pageLog;
        }
    };
}
