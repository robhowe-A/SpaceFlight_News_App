using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;

namespace SpaceFlight_News_App.Pages
{
    public class ApodModel : PageModel
    {
        public APOD[]? apods;
        public APOD? apod;

        [BindProperty(SupportsGet = true)]
        public string Message { get; set; } = "Loading...";
        public string UnavailableToday { get; set; } = "The APOD today is unavailable.";
        public string FetchServerUnavailableMessage { get; set; } = string.Empty;
        public string FetchServerStatusCode { get; set; } = string.Empty;
        public string FetchFailDate { get; set; }
        public bool AvailableToday { get; set; } = true;

        private readonly ILogger<ApodModel> _logger;

        public ApodModel(ILogger<ApodModel> logger)
        {
            _logger = logger;
        }

        private void OnFetchServerUnavailable(object sender, FetchServerUnavailableEventArgs e)
        {
            FetchServerUnavailableMessage = e.ErrorMessage;
        }

        private SpaceFlightDataBus _spaceFlightDataBus = new SpaceFlightDataBus();

        public async Task<IActionResult> OnGet()
        {
            //var spaceFlightDataBus = new SpaceFlightDataBus();
            apods = await _spaceFlightDataBus.GetApods();

            //Log activity
            string pageLog = PageLogger.WritePageLog(this);
            _logger.Log(LogLevel.Information, pageLog);
            //End log

            if (apods == null || !apods.Any())
            {
                Message = "No APODS were found.";

                return Page();
            }
            else
            {
                var displayAPOD = apods[0];
                if (string.IsNullOrEmpty(apods[0].apodtitle))
                    displayAPOD = apods[1];

                var shortdatestr = DateTime.Today.ToShortDateString();
                var todate = DateTime.Today.Date;

                FetchServerUnavailableMessage = FetchServerUnavailableEventArgs.FetchErrorMessage;
                FetchServerStatusCode = FetchServerUnavailableEventArgs.StatusCode;
                FetchFailDate = FetchServerUnavailableEventArgs.Date.ToShortDateString();

                _spaceFlightDataBus.FetchServerUnavailable += OnFetchServerUnavailable;

                if (DateTime.Parse(displayAPOD.date) < DateTime.Today)
                    AvailableToday = false;

                //Display the first apod in the array
                apod = displayAPOD;
                return Page();
            }
        }
    }
}
