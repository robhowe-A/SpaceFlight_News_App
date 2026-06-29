// ==============================================================================
// Filename: SpaceFlightServer.cs
// 
// Author: Robert Howell
// Date: 6/24/2024
// Edited: 6/22/2026
// Version: 2.1
//
// Description: This file contains the SpaceFlightServer class. This class is
//     responsible for starting the backend server and scheduling data fetches.
//
// -- Edited data bus calls
// -- V2 - Edited constructor to include IConfiguration
//
// ==============================================================================

using System.Timers;

namespace SpaceFlight_News_App.Models
{
    internal sealed class SpaceFlightServer
    {
        // Instance of SpaceFlightServer begins background processing tasks.
        // Return timespans, which print out in friendly hour-min-seconds format
        public TimeSpan ArticleFetchTimerMilliseconds
        => TimeSpan.FromMilliseconds(this._articlesOneHourTimer.Interval);

        public TimeSpan ApodFetchTimerMilliseconds
        => TimeSpan.FromMilliseconds(this._apodFiveHourTimer.Interval);

        // Timers set for 20 minutes and 30 minutes, used for data fetches
        private readonly System.Timers.Timer _articlesOneHourTimer = new System.Timers.Timer(3600000); //60 minutes
        private readonly System.Timers.Timer _apodFiveHourTimer = new System.Timers.Timer(18000000); //300 minutes

        //
        // Summary:
        //     Begin backend server operation.
        //
        //
        public void Start()
        {
            // Ensure database is seeded with data, first
            var spaceFlightDataBus = new SpaceFlightDataBus();
            //Seed database, if empty.
            spaceFlightDataBus.CheckForDatabaseData();
            Console.WriteLine("Completed database data check.");

            // Step 1: Create a new Thread
            var myThread = new Thread(ScheduleDataFetch)
                   {
                          Name = "Timed Data Fetch",
                          IsBackground = true,
                          Priority = ThreadPriority.Normal
                   };

            Console.WriteLine($"Fetch thread state is: {myThread.ThreadState.ToString()}");

            // Step 2: Start the Thread
            myThread.Start();
            Console.WriteLine($"Fetch thread state is: {myThread.ThreadState.ToString()}");


            // Step 3: Main thread continues here.
            OnTimedCreateArticlesContext(); //fetch once
            OnTimedCreateApodContext(); //fetch once

            Console.WriteLine("Main thread continues.");
        }

        // Data fetches are scheduled to add database data on a schedule
        private async void ScheduleDataFetch()
        {
            try
            {
                // Use your context here
                _articlesOneHourTimer.Elapsed += OnTimedCreateArticlesContext;
                _articlesOneHourTimer.AutoReset = true;
                _articlesOneHourTimer.Enabled = true;

                _apodFiveHourTimer.Elapsed += OnTimedCreateApodContext;
                _apodFiveHourTimer.AutoReset = true;
                _apodFiveHourTimer.Enabled = true;

                //Keep this thread alive to allow schedule to continue
                //await Task.Delay(TimeSpan.FromHours(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ScheduleDataFetch: {ex.Message}");
            }
        }

        // Function called for timed article fetch
        private async void OnTimedCreateArticlesContext()
        {
            OnTimedFetchArticles();
        }
        private async void OnTimedCreateArticlesContext(object? source, ElapsedEventArgs? e)
        {
            OnTimedFetchArticles();
        }

        private async void OnTimedFetchArticles()
        {
            SpaceFlightDataBus spaceFlightDataBus = new SpaceFlightDataBus();
            await spaceFlightDataBus.OnTimedEventFetchArticles();
        }

        // Function called for timed apod fetch
        private async void OnTimedCreateApodContext()
        {
            OnTimedFetchApod();
        }
        private async void OnTimedCreateApodContext(object? source, ElapsedEventArgs? e)
        {
            OnTimedFetchApod();
        }

        private static async void OnTimedFetchApod()
        {
            SpaceFlightDataBus spaceFlightDataBus = new SpaceFlightDataBus();
            await spaceFlightDataBus.OnTimedEventFetchApods();
        }
    };
}
