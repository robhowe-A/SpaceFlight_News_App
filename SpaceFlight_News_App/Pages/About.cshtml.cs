// ==============================================================================
// Filename: About.cshtml.cs
//
// Author: Robert Howell
// Date: 6/27/2024
// Edited: 6/22/2026
// Version: 1.3
//
// Description: This is the code-behind for the site's about page.
//
//
// ==============================================================================

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
            var spaceFlightDataBus = new SpaceFlightDataBus();
            NewsSites = await spaceFlightDataBus.GetNewsSites();

            //Sort the list
            NewsSites.Sort();

            if (NewsSites.Count >= 0)
            {
                //Remove all empty strings from the list
                NewsSites.RemoveAll(s => s == "");
            }

            return Page();
        }
    }
}
