// ==============================================================================
// Filename: ApodShort.cshtml.cs
//
// Author: Robert Howell
// Date: 6/29/2024
// Edited: 6/22/2026
// Version: 1.0
//
// Description: This is the code-behind for the site's small details of an APOD.
//  
//
// ==============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;

namespace SpaceFlight_News_App.Pages
{
    public class ApodShortModel : PageModel
    {
        public APOD[]? Apods;
        public APOD? Apod;

        [BindProperty(SupportsGet = true)]
        public string Message { get; set; } = "Loading...";

#if DEBUG
        public async Task<IActionResult> OnGet()
        {
            var spaceFlightDataBus = new SpaceFlightDataBus();
            Apods = await spaceFlightDataBus.GetApods();

            if (Apods == null || !Apods.Any())
            {
                Message = "No APODS were found.";

            }
            else
            {
                Apod = Apods[0];
            }

            return Page();
        }
#endif
    }
}
