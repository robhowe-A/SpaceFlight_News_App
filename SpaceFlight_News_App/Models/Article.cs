// ==============================================================================
// Filename: Article.cs
// 
// Author: Robert Howell
// Date: 6/24/2024
// Edited: 6/29/2024
// Version: 1.2
//
// Description: This file holds DTO objects for articles that are fetched from the API.
//
// -- Added Result class
//
// ==============================================================================

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SpaceFlight_News_App.Models
{
    public class Result
    {
        [JsonPropertyName("results")]
        public IList<Article>? articles { get; set; }
    };

    public class Article
    {
        public string title { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        [JsonPropertyName("image_url")]
        public string textURL { get; set; } = string.Empty;
        public string summary { get; set; } = string.Empty;
        [JsonPropertyName("published_at")]
        public DateTime date { get; set; }
        public Boolean featured { get; set; } = false;

        public IList<LaunchReference>? launches { get; set; }
        public int id { get; set; } = -1;
        [Key]
        public int articleNum { get; set; }
        [JsonPropertyName("news_site")]
        public string? newsSite { get; set; } = string.Empty;

        public DateTime GetArticleDate()
        {
            //Reference the current date for article retrieval
            DateTime targetDate = this.date.Date;
            return targetDate;
        }
    };

    public class LaunchReference
    {
        [Key]
        public int launchNum { get; set; }
        [JsonPropertyName("launch_id")]
        public string? id { get; set; }
        public string? provider { get; set; }
    };
}
