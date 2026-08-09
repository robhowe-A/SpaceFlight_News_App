// ==============================================================================
// 
// Author: Robert Howell
// Date: 8/9/2026
//
// ==============================================================================

namespace SpaceFlight_News_App.Background
{
    internal static class BackgroundSettingsProvider
    {
        //Create a context for this backend request to use
        public static IConfiguration AppSettingsConfiguration { get; } = new ConfigurationBuilder()
                                                    .AddJsonFile("appsettings.json")
                                                    .Build();
    };
}
