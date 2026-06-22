// ==============================================================================
// Filename: Hyperlink.cs
// 
// Author: Robert Howell
// Date: 6/28/2024
// Edited: 6/22/2026
// Version: 1.0
//
// Description: HyperLinks hold data for anchor tags used.
//
// ==============================================================================

namespace SpaceFlight_News_App.Models
{
    public class HyperLink(string href, string textContent)
    {
        public string Href { get; set; } = href;
        public string TextContent { get; set; } = textContent;

        public override string ToString()
        {
           return $"<a href=\"{this.Href}\">{this.TextContent}</a>";
        }
    };
}
