// ==============================================================================
// Filename: Error.cshtml.cs
//
// Author: Robert Howell
// Date: 6/25/2024
// Edited: 6/22/2026
// Version: 1.3
//
// Description: This is the code-behind for the site's error page.
//
// ==============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace SpaceFlight_News_App.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }
        public ActivityStatusCode? ActivityStatusCode { get; set; } = System.Diagnostics.ActivityStatusCode.Unset;

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ActivityStatusCode = Activity.Current?.Status ?? System.Diagnostics.ActivityStatusCode.Unset;
        }
    }
}
