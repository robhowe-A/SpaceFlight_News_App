using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceFlight_News_App.Models;
using System.Text.RegularExpressions;

namespace SpaceFlight_News_App.Pages
{
    public class ArticlesModel : PageModel
    {
        public Article[]? articles;
        public string? ArticleDate { get; set; }
        public string? PreviousArticleDate { get; set; }
        public string? PreviousWeekArticleDate { get; set; }
        public string? OldestArticleDate { get; set; }
        public string? NextWeekArticleDate { get; set; }
        public string? NextArticleDate { get; set; }
        public string Message { get; set; } = "Loading...";
        public string? ViewQP { get; set; }
        public string? NewsSiteQP { get; set; }
        public string? SetDateQP { get; set; }
        private string? _viewQP { get; set; }
        private string? _newsSiteQP { get; set; }
        private string? _setDateQP { get; set; }
        public string TableActive { get; set; } = string.Empty;
        public string GridActive { get; set; } = string.Empty;
        public string GridSmActive { get; set; } = string.Empty;

        private readonly ILogger<ArticlesModel> _logger;

        public ArticlesModel(ILogger<ArticlesModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string? SetDate, string? NewsSite, string? View)
        {
            //Set query parameters from client's render
            SetDateQP = String.IsNullOrWhiteSpace(SetDate) ? null : SetDate.Trim();
            NewsSiteQP = String.IsNullOrWhiteSpace(NewsSite) ? null : NewsSite.Trim();
            ViewQP = String.IsNullOrWhiteSpace(View) ? null : View.Trim();

            //Verify QPs and/or set to null
            //SetDate
            DateTime trySetDateParse;
            if(DateTime.TryParse(SetDateQP, out trySetDateParse))
            {
                _setDateQP = trySetDateParse.ToShortDateString();
            }
            else
            {
                _setDateQP = null;
            }

            //NewsSite
            _newsSiteQP = NewsSiteQP == null ? null : 
                Regex.Match(NewsSiteQP, "^[A-Za-z0-9 .]{0,48}").Value;

            //View
            string[] views = ["grid", "table", "grid-sm"];
            if(ViewQP != null)
                if(View != null & views.ToList().Contains(ViewQP))
                {
                    _viewQP = ViewQP;
                }
                else
                    _viewQP = ViewQP ?? null;

            //SPECIAL: 30 DAYS SEARCH ROLLING DATA
            if (_setDateQP != null) // keep 30 days of articles by limiting SetDate query
                if ((DateTime.Parse(_setDateQP!) <= DateTime.Today.AddDays(-30)))
                {
                    _setDateQP = DateTime.Today.AddDays(-30).ToShortDateString();
                }

            await FetchArticlesData(_setDateQP, _newsSiteQP, _viewQP);

            //Log activity
            string pageLog = PageLogger.WritePageLog(this);
            _logger.Log(LogLevel.Information, pageLog);
            //End log

            if (CheckArticlesNullorEmpty())
            {
                Message = "No articles were found matching the search string.";
                Console.WriteLine("There is no data returned from the query!");

                if (_setDateQP != null && DateTime.Parse(_setDateQP) < new DateTime(2024, 06, 18)){
                    Message = "No articles earlier than 2024-06-18 are unavailable.";
                }
                _newsSiteQP = null;

                return Page();
            }
            else
            {
                // Set the date for page view
                ArticleDate = articles![0].GetArticleDate().ToShortDateString();

                return Page();
            }
        }

        private async Task FetchArticlesData(string? _setDateQP, string? _newsSiteQP, string? _viewQP)
        {
            var spaceFlightDataBus = new SpaceFlightDataBus();

            // Check query parameters in request
            if (CheckForQPData(_setDateQP, _newsSiteQP, _viewQP))
            {
                if (!String.IsNullOrEmpty(_newsSiteQP))
                {
                    articles = await spaceFlightDataBus.QueryArticleSites(_newsSiteQP);
                    Console.WriteLine($"There's a data fetch for articles by news site: {_newsSiteQP}");
                }
                else
                {
                    if (String.IsNullOrEmpty(_setDateQP) || !DateTime.TryParse(_setDateQP, out DateTime parsedDate))
                    {
                        articles = await spaceFlightDataBus.GetArticles();
                        Console.WriteLine($"Invalid date parameter.");
                    }
                    else
                    {
                        articles = await spaceFlightDataBus.GetArticles(parsedDate);
                        Console.WriteLine($"There's a data fetch for articles by date: {_setDateQP}");
                    }
                }
            }
            else
            {
                articles = await spaceFlightDataBus.GetArticles();
                Console.WriteLine($"There's a general data fetch for articles.");
            }
        }

        private bool CheckForQPData(string? setDateQP = null, string? newsSiteQP = null, string? viewQP = null)
        {
            if (viewQP == "grid")
            {
                TableActive = string.Empty;
                GridActive = "active";
                GridSmActive = string.Empty;
            }
            else if (viewQP == "table")
            {
                TableActive = "active";
                GridActive = string.Empty;
                GridSmActive = string.Empty;
            }
            else if (viewQP == "grid-sm")
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
            if (!String.IsNullOrEmpty(viewQP) || 
                !String.IsNullOrEmpty(setDateQP) ||
                !String.IsNullOrEmpty(newsSiteQP))
            { return true; } else { return false; }
        }

        private bool CheckArticlesNullorEmpty()
        {
            return articles == null || articles.Length <= 0;
        }

        private void CheckArticleDateIsEmpty()
        {
            if (String.IsNullOrWhiteSpace(ArticleDate))
            {
                if (CheckArticlesNullorEmpty())
                {
                    ArticleDate = DateTime.Now.ToShortDateString();
                }
                else
                {
                    ArticleDate = articles![0].date.Date.ToShortDateString();
                }
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
