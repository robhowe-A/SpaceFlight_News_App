using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;

namespace SpaceFlight_News_App.Pages
{
    public class AboutModel : PageModel
    {
        public List<string> NewsSites = [];
        public HyperLink SpaceFlightApiLink = new HyperLink("https://spaceflightnewsapi.net/", "https://spaceflightnewsapi.net/");
        public HyperLink ApodLink = new HyperLink("https://apod.nasa.gov/apod/astropix.html", "Astronomy Picture of the Day");

        public async Task<IActionResult> OnGet()
        {
            //Call the database to fetch the news sites available
            SpaceFlightDataBus spaceFlightDataBus = new SpaceFlightDataBus();
            NewsSites = await spaceFlightDataBus.GetNewsSites() ?? ["No news sites returned."];

            //Sort the list
            NewsSites.Sort();

            if (NewsSites.Count >= 0 && NewsSites != null)
            {
                //Remove all empty strings from the list
                NewsSites.RemoveAll(s => s == "" || s == null);

                return Page();
            }
            else
            {
                return Page();
            }
        }
    }
}
