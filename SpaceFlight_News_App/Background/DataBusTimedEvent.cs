// ==============================================================================
// 
// Author: Robert Howell
// Date: 8/9/2026
// Version: 1.2
//
// Description: This file is an addition to the data bus design for timed events.
//
// ==============================================================================

using System.Net;
using System.Text.Json;

namespace SpaceFlight_News_App.Background
{
    internal sealed class DatabusTimedEvent : SpaceFlightDataBus
    {
        public async Task OnTimedEventFetchArticles()
        {
            var sfnApiKey = configuration.GetConnectionString("SFN_API_KEY") ?? throw new NullReferenceException("Missing article environment variable.");

            try
            {
                var jsonString = await new ExternalApiHttpService().GetStringAsync(sfnApiKey);
                var results = JsonSerializer.Deserialize<Result>(jsonString);

                if (results == null)
                {
                    Console.Write("No results from fetch.");
                }
                else
                {
                    if (results.articles == null || !results.articles.Any())
                    {
                        //Articles not in results array
                        Console.WriteLine($"Successful fetch and there are no articles.");
                    }
                    else
                    {
                        var articles = results.articles;
                        var articlesSorted = articles.OrderBy(a => a.id).ToList();

                        await _spaceflightDatabase.SetArticles(articlesSorted);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in onTimedEventFetchArticles: {ex.Message}");
            }
            finally
            {
                _spaceflightDatabase.Dispose();
                WriteConsoleMessage($"Timed article fetch completed.");
            }
        }

        public async Task OnTimedEventFetchApods()
        {
            var apodApiKey = configuration.GetConnectionString("APOD_API_KEY") ?? throw new NullReferenceException("Missing apod environment variable.");

            try
            {
                var apod = new APOD();
                var jsonString = await new ExternalApiHttpService().GetStringAsync(apodApiKey);

                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    Console.WriteLine($"Fetch resulted in an empty set;");
                }
                else
                {
                    apod = JsonSerializer.Deserialize<APOD>(jsonString) ?? new APOD();
                    apod.id = 0;
                }

                if (apod.id == 0)
                {
                    await _spaceflightDatabase.SetApods(apod);
                }
                else
                {
                    Console.WriteLine($"Successful fetch, however there are no articles.");
                }
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.ServiceUnavailable || e.StatusCode == HttpStatusCode.GatewayTimeout)
                {
                    OnFetchServerUnavailable(new FetchServerUnavailableEventArgs($"{e.Message}", $"{e.StatusCode.ToString()}"));
                    Console.WriteLine($"Error occurred fetching APOD.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in onTimedEventFetchApods: {ex.Message}");
            }
            finally
            {
                _spaceflightDatabase.Dispose();
                WriteConsoleMessage($"Timed apod fetch completed.");
            }
        }
    };
}
