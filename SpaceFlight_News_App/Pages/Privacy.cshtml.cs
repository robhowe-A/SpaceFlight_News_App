using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;
using System;
using System.Text;

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
