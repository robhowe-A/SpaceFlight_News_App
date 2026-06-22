// ==============================================================================
// Filename: Articles.cshtml.cs
//
// Author: Robert Howell
// Date: 6/25/2024
// Edited: 6/22/2026
// Version: 1.3
//
// Description: This is the code-behind for the site's Articles page.
//
// ==============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;
using System.Text.RegularExpressions;

namespace SpaceFlight_News_App.Pages
{
    public class ArticlesModel : PageModel
    {
        public Article[]? Articles;
        public string? ArticleDate { get; set; }
        public string? PreviousArticleDate { get; set; }
        public string? PreviousWeekArticleDate { get; set; }
        public string? OldestArticleDate { get; set; }
        public string? NextWeekArticleDate { get; set; }
        public string? NextArticleDate { get; set; }
        public string Message { get; set; } = "Loading...";
        public string? ViewQp { get; set; }
        public string? NewsSiteQp { get; set; }
        public string? SetDateQp { get; set; }
        private string? _viewQp { get; set; }
        private string? _newsSiteQp { get; set; }
        private string? _setDateQp { get; set; }
        public string TableActive { get; set; } = string.Empty;
        public string GridActive { get; set; } = string.Empty;
        public string GridSmActive { get; set; } = string.Empty;

        private readonly ILogger<ArticlesModel> _logger;

        public ArticlesModel(ILogger<ArticlesModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string? setDate, string? newsSite, string? view)
        {
            //Set query parameters from client's render
            SetDateQp = String.IsNullOrWhiteSpace(setDate) ? null : setDate.Trim();
            NewsSiteQp = String.IsNullOrWhiteSpace(newsSite) ? null : newsSite.Trim();
            ViewQp = String.IsNullOrWhiteSpace(view) ? null : view.Trim();

            //Verify QPs and/or set to null
            //SetDate
            DateTime trySetDateParse;
            if(DateTime.TryParse(SetDateQp, out trySetDateParse))
            {
                _setDateQp = trySetDateParse.ToShortDateString();
            }
            else
            {
                _setDateQp = null;
            }

            //NewsSite
            _newsSiteQp = NewsSiteQp == null ? null : 
                Regex.Match(NewsSiteQp, "^[A-Za-z0-9 .]{0,48}").Value;

            //View
            string[] views = ["grid", "table", "grid-sm"];
            if(ViewQp != null)
                if(view != null & views.ToList().Contains(ViewQp))
                {
                    _viewQp = ViewQp;
                }
                else
                    _viewQp = ViewQp ?? null;

            //SPECIAL: 30 DAYS SEARCH ROLLING DATA
            if (_setDateQp != null) // keep 30 days of articles by limiting SetDate query
                if ((DateTime.Parse(_setDateQp!) <= DateTime.Today.AddDays(-30)))
                {
                    _setDateQp = DateTime.Today.AddDays(-30).ToShortDateString();
                }

            await FetchArticlesData(_setDateQp, _newsSiteQp, _viewQp);

            //Log activity
            string pageLog = PageLogger.WritePageLog(this);
            _logger.Log(LogLevel.Information, pageLog);
            //End log

            if (CheckArticlesNullorEmpty())
            {
                Message = "No articles were found matching the search string.";
                Console.WriteLine("There is no data returned from the query!");

                if (_setDateQp != null && DateTime.Parse(_setDateQp) < new DateTime(2024, 06, 18)){
                    Message = "No articles earlier than 2024-06-18 are unavailable.";
                }
                _newsSiteQp = null;

                return Page();
            }
            else
            {
                // Set the date for page view
                ArticleDate = Articles![0].GetArticleDate().ToShortDateString();

                return Page();
            }
        }

        private async Task FetchArticlesData(string? setDateQp, string? newsSiteQp, string? viewQp)
        {
            var spaceFlightDataBus = new SpaceFlightDataBus();

            // Check query parameters in request
            if (CheckForQpData(setDateQp, newsSiteQp, viewQp))
            {
                if (!String.IsNullOrEmpty(newsSiteQp))
                {
                    Articles = await spaceFlightDataBus.QueryArticleSites(newsSiteQp);
                    Console.WriteLine($"There's a data fetch for articles by news site: {newsSiteQp}");
                }
                else
                {
                    if (String.IsNullOrEmpty(setDateQp) || !DateTime.TryParse(setDateQp, out DateTime parsedDate))
                    {
                        Articles = await spaceFlightDataBus.GetArticles();
                        Console.WriteLine($"Invalid date parameter.");
                    }
                    else
                    {
                        Articles = await spaceFlightDataBus.GetArticles(parsedDate);
                        Console.WriteLine($"There's a data fetch for articles by date: {setDateQp}");
                    }
                }
            }
            else
            {
                Articles = await spaceFlightDataBus.GetArticles();
                Console.WriteLine($"There's a general data fetch for articles.");
            }
        }

        private bool CheckForQpData(string? setDateQp = null, string? newsSiteQp = null, string? viewQp = null)
        {
            if (viewQp == "grid")
            {
                TableActive = string.Empty;
                GridActive = "active";
                GridSmActive = string.Empty;
            }
            else if (viewQp == "table")
            {
                TableActive = "active";
                GridActive = string.Empty;
                GridSmActive = string.Empty;
            }
            else if (viewQp == "grid-sm")
            {
                TableActive = string.Empty;
                GridActive = string.Empty;
                GridSmActive = "active";
            }
            else
            {
                TableActive = string.Empty;
                GridActive = "active"; // default view is grid
                GridSmActive = string.Empty;
            }

            //Check for query parameters
            if (!String.IsNullOrEmpty(viewQp) || 
                !String.IsNullOrEmpty(setDateQp) ||
                !String.IsNullOrEmpty(newsSiteQp))
            { return true; } else { return false; }
        }

        private bool CheckArticlesNullorEmpty()
        {
            return Articles == null || Articles.Length <= 0;
        }

        private void CheckArticleDateIsEmpty()
        {
            if (!String.IsNullOrWhiteSpace(ArticleDate)) return;

            if (CheckArticlesNullorEmpty())
            {
                ArticleDate = DateTime.Now.ToShortDateString();
            }
            else
            {
                ArticleDate = Articles![0].date.Date.ToShortDateString();
            }
        }

        public string GetPreviousArticleDate()
        {
            CheckArticleDateIsEmpty();

            var previousArticleDate = DateTime.Parse(ArticleDate!).AddDays(-1);
            PreviousArticleDate = previousArticleDate.ToShortDateString();

            // keep 30 days of articles by limiting SetDate query
            if ((DateTime.Parse(PreviousArticleDate) <= DateTime.Today.AddDays(-30)))
            {
                PreviousArticleDate = DateTime.Today.AddDays(-30).ToShortDateString();
            }

            return previousArticleDate.ToShortDateString();
        }

        public string GetPreviousWeekArticleDate()
        {
            CheckArticleDateIsEmpty();

            var previousArticleDate = DateTime.Parse(ArticleDate!).AddDays(-7);
            PreviousWeekArticleDate = previousArticleDate.ToShortDateString();

            // keep 30 days of articles by limiting SetDate query
            if ((DateTime.Parse(PreviousWeekArticleDate) <= DateTime.Today.AddDays(-30)))
            {
                PreviousWeekArticleDate = DateTime.Today.AddDays(-30).ToShortDateString();
            }

            return previousArticleDate.ToShortDateString();
        }

        public string GetNextArticleDate()
        {
            CheckArticleDateIsEmpty();

            var nextArticleDate = DateTime.Parse(ArticleDate!).AddDays(1);
            NextArticleDate = nextArticleDate.ToShortDateString();

            return nextArticleDate.ToShortDateString();
        }

        public string GetNextWeekArticleDate()
        {
            CheckArticleDateIsEmpty();

            var nextArticleDate = DateTime.Parse(ArticleDate!).AddDays(7);
            NextWeekArticleDate = nextArticleDate.ToShortDateString();

            return nextArticleDate.ToShortDateString();
        }
        public async Task<string> GetOldestArticleDate()
        {
            CheckArticleDateIsEmpty();
            //check database for oldest date
            var spaceFlightDataBus = new SpaceFlightDataBus();
            var oldestArticleDate = await spaceFlightDataBus.GetOldestArticleDate();
            OldestArticleDate = oldestArticleDate.ToShortDateString();

            // keep 30 days of articles by limiting SetDate query
            if ((DateTime.Parse(OldestArticleDate) <= DateTime.Today.AddDays(-30)))
            {
                OldestArticleDate = DateTime.Today.AddDays(-30).ToShortDateString();
            }

            return OldestArticleDate;
        }
    }
}
