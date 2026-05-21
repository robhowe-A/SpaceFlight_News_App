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
