// ==============================================================================
// Filename: SpaceFlightServer.cs
// 
// Author: Robert Howell
// Date: 6/24/2024
// Edited: 8/9/2026
// Version: 2.1
//
// Description: This file contains the SpaceFlightServer class. This class is
//     responsible for starting the backend server and scheduling data fetches.
//
// -- Edited data bus calls
// -- V2 - Edited constructor to include IConfiguration
//
// ==============================================================================

namespace SpaceFlight_News_App.Background
{
    internal sealed class SpaceFlightServer : IProcessLogging
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

        private LocalProcessInfo _localProcess { get; } = new LocalProcessInfo();
        private BackgroundFetch _timedFetch { get; } = new BackgroundFetch();

        //
        // Summary:
        //     Begin backend server operation.
        //
        //
        public LocalProcessInfo Start()
        {

            // Ensure database is seeded with data, first
            var spaceFlightDataBus = new SpaceFlightDataBus();
            //Seed database, if empty.
            spaceFlightDataBus.CheckForDatabaseData();

            WriteConsoleMessage(_localProcess, "Completed database data check.");

            // Step 1: Create a new Thread
            var myThread = new Thread(ScheduleDataFetch)
                   {
                          Name = "Timed Data Fetch",
                          IsBackground = true,
                          Priority = ThreadPriority.Normal
                   };

            WriteConsoleMessage(_localProcess, $"Fetch thread state is: {myThread.ThreadState.ToString()}");

            // Step 2: Start the Thread
            myThread.Start();
            WriteConsoleMessage(_localProcess, $"Fetch thread state is: {myThread.ThreadState.ToString()}");

            // Step 3: Main thread continues here.
            _timedFetch.OnTimedCreateArticlesContext(); //fetch once
            _timedFetch.OnTimedCreateApodContext(); //fetch once

            WriteConsoleMessage(_localProcess, "Main thread continues.");

            return _localProcess;
        }

        /// <summary>
        /// Data fetches are scheduled to add database data on a schedule
        /// </summary>
        private async void ScheduleDataFetch()
        {
            WriteConsoleMessage($"Background thread started.");

            try
            {
                _articlesOneHourTimer.Elapsed += _timedFetch.OnTimedCreateArticlesContext;
                _articlesOneHourTimer.AutoReset = true;
                _articlesOneHourTimer.Enabled = true;

                _apodFiveHourTimer.Elapsed += _timedFetch.OnTimedCreateApodContext;
                _apodFiveHourTimer.AutoReset = true;
                _apodFiveHourTimer.Enabled = true;

            }
            catch (Exception ex)
            {
                WriteConsoleMessage($"Error in ScheduleDataFetch: {ex.Message}\n{ex.StackTrace}");
            }
        }
    };
}
