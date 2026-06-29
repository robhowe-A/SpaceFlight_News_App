// ==============================================================================
// Filename: APOD.cs
// 
// Author: Robert Howell
// Date: 6/24/2024
// Version: 1.0
//
// Description: This object is a DTO for Astronomy Picture of the Day (APOD) that
// are fetched from the API.
//
// ==============================================================================

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SpaceFlight_News_App.Models
{
    public class APOD
    {
        [JsonPropertyName("title")]
        public string apodtitle { get; set; } = string.Empty;
        public string date { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string hdurl { get; set; } = string.Empty;

        [JsonPropertyName("explanation")]
        public string description { get; set; } = string.Empty;

        [JsonPropertyName("media_type")]
        public string media { get; set; } = string.Empty;
        [Key]
        public int id { get; set; } = -1;
    };
}
