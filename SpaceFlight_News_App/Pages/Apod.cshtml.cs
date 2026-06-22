// ==============================================================================
// Filename: Apod.cshtml.cs
//
// Author: Robert Howell
// Date: 6/29/2024
// Edited: 6/22/2026
// Version: 1.0
//
// Description: The front-end page displays full details of an APOD.
//
//
// ==============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;

namespace SpaceFlight_News_App.Pages
{
    public class ApodModel(ILogger<ApodModel> logger) : PageModel
    {
        public APOD[]? Apods;
        public APOD? Apod;

        [BindProperty(SupportsGet = true)]
        public string Message { get; set; } = "Loading...";
        public string UnavailableToday { get; set; } = "The APOD today is unavailable.";
        public string FetchServerUnavailableMessage { get; set; } = string.Empty;
        public string FetchServerStatusCode { get; set; } = string.Empty;
        public string? FetchFailDate { get; set; }
        public bool AvailableToday { get; set; } = true;

        private void OnFetchServerUnavailable(object? sender, FetchServerUnavailableEventArgs e)
        {
            FetchServerUnavailableMessage = e.ErrorMessage;
        }

        private SpaceFlightDataBus _spaceFlightDataBus = new SpaceFlightDataBus();

        public async Task<IActionResult> OnGet()
        {
            //var spaceFlightDataBus = new SpaceFlightDataBus();
            Apods = await _spaceFlightDataBus.GetApods();

            //Log activity
            string pageLog = PageLogger.WritePageLog(this);
            logger.Log(LogLevel.Information, pageLog);
            //End log

            if (Apods == null || !Apods.Any())
            {
                Message = "No APODS were found.";

                return Page();
            }
        
            var displayApod = Apods[0];
            if (string.IsNullOrEmpty(Apods[0].apodtitle))
                displayApod = Apods[1];

            FetchServerUnavailableMessage = FetchServerUnavailableEventArgs.FetchErrorMessage;
            FetchServerStatusCode = FetchServerUnavailableEventArgs.StatusCode;
            FetchFailDate = FetchServerUnavailableEventArgs.Date.ToShortDateString();

            _spaceFlightDataBus.FetchServerUnavailable += OnFetchServerUnavailable;

            if (DateTime.Parse(displayApod.date) < DateTime.Today)
                AvailableToday = false;

            //Display the first apod in the array
            Apod = displayApod;
            return Page();
        }
    }
}
