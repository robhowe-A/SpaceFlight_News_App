// ==============================================================================
// Filename: FetchServerUnavailableEventArgs.cs
// 
// Author: Robert Howell
// Date: 6/22/2026
// Version: 1.0
//
// Description: FetchServerUnavailableEventArgs hold data for server unavailable events.
//
// ==============================================================================

namespace SpaceFlight_News_App.Models
{
    public class FetchServerUnavailableEventArgs : EventArgs
    {
        public string ErrorMessage { get; }

        public static string FetchErrorMessage { get; private set; } = string.Empty;
        public static string StatusCode { get; private set; } = string.Empty;
        public static DateTime Date { get; private set; }

        public FetchServerUnavailableEventArgs(string errorMessage, string statusCode)
        {
            this.ErrorMessage = errorMessage;
            FetchServerUnavailableEventArgs.FetchErrorMessage = errorMessage;
            FetchServerUnavailableEventArgs.StatusCode = statusCode;
            Date = DateTime.Today;
        }
    }
}
