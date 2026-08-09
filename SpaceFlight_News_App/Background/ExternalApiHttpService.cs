// ==============================================================================
// 
// Author: Robert Howell
// Created: 8/8/2024
// Description: This service centralizes Http Request.
//
// ==============================================================================

namespace SpaceFlight_News_App.Background
{
    internal sealed class ExternalApiHttpService : IHttpService
    {
        private HttpClient _httpClient;

        public ExternalApiHttpService()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestVersion = new Version(2, 0);
            httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;

#if DEBUG
            httpClient.DefaultRequestHeaders.Add("User-Agent", @"spaceflight-web.rhdeveloping.com | Dev environment | Robert Howell");
#else
            httpClient.DefaultRequestHeaders.Add("User-Agent", @"spaceflight-web.rhdeveloping.com | Server environment | Developed by Robert Howell");
#endif
            _httpClient = httpClient;
        }

        public void GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetStringAsync(string requestUri)
        {
            return await _httpClient.GetStringAsync(requestUri);
        }
    };
}
