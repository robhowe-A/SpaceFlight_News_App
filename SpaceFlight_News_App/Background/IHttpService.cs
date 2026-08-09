// ==============================================================================
// 
// Author: Robert Howell
// Created: 8/8/2024
//
// ==============================================================================

namespace SpaceFlight_News_App.Background
{
    internal interface IHttpService
    {
        public void GetAsync();

        public Task<string> GetStringAsync(string requestUri);

    };
}
