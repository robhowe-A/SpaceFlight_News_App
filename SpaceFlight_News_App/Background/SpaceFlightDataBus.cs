// ==============================================================================
// Filename: SpaceFlightDataBus.cs
// 
// Author: Robert Howell
// Date: 6/24/2024
// Edited: 8/9/2026
// Version: 1.2
//
// Description: This file is a data bus design database additions/changes.
//
// ==============================================================================

namespace SpaceFlight_News_App.Background
{
    // <summary>
    //     SpaceFlightDataBus class is responsible for fetching/retrieving data from the database.
    //     Each object created creates a new database context.
    // </summary>
    internal class SpaceFlightDataBus
    {
        protected SpaceFlightDatabase _spaceflightDatabase;

        protected static IConfiguration configuration = BackgroundSettingsProvider.AppSettingsConfiguration;

        public SpaceFlightDataBus()
        {
            _spaceflightDatabase = new SpaceFlightDatabase();
        }

        public async Task<APOD[]> GetNewestApod()
        {
            try
            {
                return await RetrieveNewestApod();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetApods: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                _spaceflightDatabase.Dispose();
            }
            return [];
        }

        public async Task<Article[]> GetArticles(DateTime? date)
        {
            try
            {
                return await RetrieveArticles(date);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetArticles: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                _spaceflightDatabase.Dispose();
            }
            return [];
        }

        public async Task<Article[]> GetArticlesWithImages()
        {
            try
            {
                //Fetch database articles
                return await RetrieveArticlesWithImages();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetArticles: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                _spaceflightDatabase.Dispose();
            }
            return [];
        }

        public async Task<DateTime> GetOldestArticleDate()
        {
            try
            {
                return await RetieveArticleDate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetArticles: {ex.Message}");
            }
            finally
            {
                _spaceflightDatabase.Dispose();
            }
            return DateTime.Now;
        }

        public async Task<Article[]> GetArticleSites(string newsSiteInputValue)
        {
            try
            {
                return await RetrieveArticleSites(newsSiteInputValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in QueryArticleSites: {ex.Message}");
            }
            finally {
                _spaceflightDatabase.Dispose();
            }
            return [];
        }
        
        public async Task<List<string>> GetNewsSites()
        {
            try
            {
                return await RetrieveNewsSites();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetArticles: {ex.Message}");
            }
            finally
            {
                _spaceflightDatabase.Dispose();
            }
            return [];
        }
        
        public event EventHandler<FetchServerUnavailableEventArgs>? FetchServerUnavailable;

        protected void OnFetchServerUnavailable(FetchServerUnavailableEventArgs e) { 
            FetchServerUnavailable?.Invoke(this, e); // Null-check and invoke
        }

        public void CheckForDatabaseData()
        {
            try
            {
                _spaceflightDatabase.CheckForDataEmpty();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CheckForDatabaseData: {ex.Message}");

            }
            finally
            {
                _spaceflightDatabase.Dispose();
            }
        }

        private async Task<APOD[]> RetrieveNewestApod()
        {
            var apods = await _spaceflightDatabase.SelectNewestApod();
            return apods.Length > 0 ? apods : [];
        }
        
        private async Task<Article[]> RetrieveArticles(DateTime? date)
        {
            DateTime currentDate = date == null ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) : (DateTime)date;

            var articles = await _spaceflightDatabase.SelectArticles(currentDate);
            //Sort articles by date
            Array.Sort(articles, (article1, article2) => article2.date.CompareTo(article1.date));

            return articles.Length > 0 ? articles : [];
        }
        
        private async Task<Article[]> RetrieveArticlesWithImages()
        {
            var articles = new Article[] { };
            articles = await _spaceflightDatabase.SelectArticlesWithImages();
            return articles.Length > 0 ? articles : [];
        }

        private async Task<DateTime> RetieveArticleDate()
        {
            //Fetch database articles
            var oldestArticleDate = await _spaceflightDatabase.SelectOldestArticleDateTime();

            //Response data is not null
            //hard data limit possible. check non-consecutive dates and trim
            //ex: return oldestArticleDate[11];
            //12-8-25: confirmed db does not have non-consecutive dates
            //return index 0 is safe
            return oldestArticleDate.Length > 0 ? oldestArticleDate[0] : new DateTime();
        }

        private async Task<Article[]> RetrieveArticleSites(string newsSiteInputValue)
        {
            //Fetch database articles
            var articles = await _spaceflightDatabase.SelectArticleSites(newsSiteInputValue);
            return articles.Length > 0 ? articles : [];
        }
        
        private async Task<List<string>> RetrieveNewsSites()
        {
            //Fetch database articles
            var articles = await _spaceflightDatabase.SelectNewsSites();
            return articles.Count > 0 ? articles : [];
        }
    }
};
