// ==============================================================================
// Filename: SpaceFlightServer.cs
// 
// Author: Robert Howell
// Date: 6/24/2024
// Edited: 6/29/2024
// Version: 2.1
//
// Description: This file contains the SpaceFlightServer class. This class is
//     responsible for starting the backend server and scheduling data fetches.
//
// -- Edited data bus calls
// -- V2 - Edited constructor to include IConfiguration
//
// ==============================================================================

using Microsoft.EntityFrameworkCore;
using SpaceFlight_News_App.Data;
using System.Timers;

namespace SpaceFlight_News_App.Models
{
    internal sealed class SpaceFlightServer
    {
        // Instance of SpaceFlightServer begins background processing tasks.
        public SpaceFlightServer(){ }

        // Return timespans, which print out in friendly hour-min-seconds format
        public TimeSpan articleFetchTimerMilliseconds
        {
            get { return TimeSpan.FromMilliseconds(this.ArticlesOneHourTimer.Interval); }
        }
        public TimeSpan apodFetchTimerMilliseconds
        {
            get { return TimeSpan.FromMilliseconds(this.ApodFiveHourTimer.Interval); }
        }

        // Timers set for 20 minutes and 30 minutes, used for data fetches
        private readonly System.Timers.Timer ArticlesOneHourTimer = new System.Timers.Timer(3600000); //60 minutes
        private readonly System.Timers.Timer ApodFiveHourTimer = new System.Timers.Timer(18000000); //300 minutes

        //
        // Summary:
        //     Begin backend server operation.
        //
        // 
        public void Start()
        {
            // Ensure database is seeded with data, first
            SpaceFlightDataBus spaceFlightDataBus = new SpaceFlightDataBus();
            //Seed database, if empty.
            spaceFlightDataBus.CheckForDatabaseData();
            Console.WriteLine("Completed database data check.");

            // Step 1: Create a new Thread
            Thread myThread = new Thread(new ThreadStart(ScheduleDataFetch));

            myThread.Name = "Timed Data Fetch";
            myThread.IsBackground = true;
            myThread.Priority = ThreadPriority.Normal;
            Console.WriteLine($"Fetch thread state is: {myThread.ThreadState.ToString()}");

            // Step 2: Start the Thread
            myThread.Start();
            Console.WriteLine($"Fetch thread state is: {myThread.ThreadState.ToString()}");


            // Step 3: Main thread continues here.
            onTimedCreateArticlesContext(); //fetch once
            onTimedCreateApodContext(); //fetch once
            
            Console.WriteLine("Main thread continues.");
        }

        // Data fetches are scheduled to add database data on a schedule
        private async void ScheduleDataFetch()
        {
            try
            {
                // Use your context here
                ArticlesOneHourTimer.Elapsed += onTimedCreateArticlesContext;
                ArticlesOneHourTimer.AutoReset = true;
                ArticlesOneHourTimer.Enabled = true;

                ApodFiveHourTimer.Elapsed += onTimedCreateApodContext;
                ApodFiveHourTimer.AutoReset = true;
                ApodFiveHourTimer.Enabled = true;

                //Keep this thread alive to allow schedule to continue
                //await Task.Delay(TimeSpan.FromHours(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ScheduleDataFetch: {ex.Message}");
            }
        }

        // Function called for timed article fetch
        private async void onTimedCreateArticlesContext()
        {
            onTimedFetchArticles();
        }
        private async void onTimedCreateArticlesContext(Object? source, ElapsedEventArgs? e)
        {
            onTimedFetchArticles();
        }

        private async void onTimedFetchArticles()
        {
            SpaceFlightDataBus spaceFlightDataBus = new SpaceFlightDataBus();
            await spaceFlightDataBus.onTimedEventFetchArticles();
        }

        // Function called for timed apod fetch
        private async void onTimedCreateApodContext()
        {
            onTimedFetchApod();
        }
        private async void onTimedCreateApodContext(Object? source, ElapsedEventArgs? e)
        {
            onTimedFetchApod();
        }

        private async void onTimedFetchApod()
        {
            SpaceFlightDataBus spaceFlightDataBus = new SpaceFlightDataBus();
            await spaceFlightDataBus.onTimedEventFetchApods();
        }
    };
}
