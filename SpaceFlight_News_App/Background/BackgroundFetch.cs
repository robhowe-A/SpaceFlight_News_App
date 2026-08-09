// ==============================================================================
// 
// Author: Robert Howell
// Date: 8/9/2026
// Version: 1.0
//
// ==============================================================================

using System.Timers;

namespace SpaceFlight_News_App.Background
{
    internal class BackgroundFetch
    {
        public async void OnTimedCreateArticlesContext()
        {
            OnTimedFetchArticles();
        }

        /// <summary>
        /// Function called for timed article fetch.
        /// </summary>
        public async void OnTimedCreateArticlesContext(object? source, ElapsedEventArgs? e)
        {
            OnTimedFetchArticles();
        }

        public async void OnTimedCreateApodContext()
        {
            OnTimedFetchApod();
        }

        /// <summary>
        /// Function called for timed apod fetch.
        /// </summary>
        public async void OnTimedCreateApodContext(object? source, ElapsedEventArgs? e)
        {
            OnTimedFetchApod();
        }

        private async void OnTimedFetchArticles()
        {
            await new DatabusTimedEvent().OnTimedEventFetchArticles();
        }
        private static async void OnTimedFetchApod()
        {
            await new DatabusTimedEvent().OnTimedEventFetchApods();
        }
    };
}
