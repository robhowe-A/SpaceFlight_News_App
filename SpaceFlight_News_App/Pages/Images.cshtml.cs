// ==============================================================================
// Filename: Images.cshtml.cs
//
// Author: Robert Howell
// Date: 7/28/2024
// Edited: 6/22/2026
// Version: 1.0
//
// Description: This is the code-behind for the site's images page.
//
// ==============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;

namespace SpaceFlight_News_App.Pages
{
    public class ImagesModel : PageModel
    {
#if DEBUG

        [BindProperty(SupportsGet = true)]
        public string Message { get; set; } = "Loading...";
        public Article[]? images;

        public async Task<IActionResult> OnGet()
        {

            var spaceFlightDataBus = new SpaceFlightDataBus();

            images = await spaceFlightDataBus.GetArticlesWithImages();

            if (images == null || !images.Any())
            {
                Message = "No articles were found.";

                return Page();
            }
            else
            {
                return Page();
            }
        }
#endif
    }
}
