using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;

namespace SpaceFlight_News_App.Pages
{
    public class ApodShortModel : PageModel
    {

        public APOD[]? apods;
        public APOD? apod;

        [BindProperty(SupportsGet = true)]
        public string Message { get; set; } = "Loading...";

#if DEBUG
        public async Task<IActionResult> OnGet()
        {
            var spaceFlightDataBus = new SpaceFlightDataBus();
            apods = await spaceFlightDataBus.GetApods();

            if (apods == null || !apods.Any())
            {
                Message = "No APODS were found.";

                return Page();
            }
            else
            {
                apod = apods[0];
                return Page();
            }
        }
#endif
    }
}
