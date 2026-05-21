namespace SpaceFlight_News_App.Models
{
    public class FetchServerUnavailableEventArgs : EventArgs
    {
        public string ErrorMessage { get; }

        public static string FetchErrorMessage { get; set; } = string.Empty;
        public static string StatusCode { get; set; } = string.Empty;
        public static DateTime Date { get; set; }

        public FetchServerUnavailableEventArgs(string ErrorMessage, string StatusCode)
        {
            this.ErrorMessage = ErrorMessage;
            FetchServerUnavailableEventArgs.FetchErrorMessage = ErrorMessage;
            FetchServerUnavailableEventArgs.StatusCode = StatusCode;
            Date = DateTime.Today;
        }
    }
}
