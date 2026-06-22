// ==============================================================================
// Filename: Index.cshtml.cs
//
// Author: Robert Howell
// Date: 1/17/2025
// Edited: 6/22/2026
// Version: 1.0
//
// Description: This is the code-behind for the site's index page.
//
//
// ==============================================================================

using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;

namespace SpaceFlight_News_App.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
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
