// ==============================================================================
// Filename: Privacy.cshtml.cs
//
// Author: Robert Howell
// Date: 7/21/2024
// Edited: 6/22/2026
// Version: 1.0
//
// Description: This is the code-behind for the site's privacy page.
//
//
// ==============================================================================

using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;

namespace SpaceFlight_News_App.Pages
{
    public sealed class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string pageLog = PageLogger.WritePageLog(this);
            _logger.Log(LogLevel.Information, pageLog);
        }
    }

}
