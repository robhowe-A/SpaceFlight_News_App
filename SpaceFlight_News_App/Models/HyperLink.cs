// ==============================================================================
// Filename: Hyperlink.cs
// 
// Author: Robert Howell
// Date: 6/28/2024
// Version: 1.0
//
// Description: HyperLinks hold data for anchor tags used.
//
// ==============================================================================


namespace SpaceFlight_News_App.Models
{
    public class HyperLink
    {
        public string Href { get; set; }
        public string TextContent { get; set; }

        public HyperLink(string Href, string TextContent)
        {
            this.Href = Href;
            this.TextContent = TextContent;
        }

        public override string ToString()
        {
           return $"<a href=\"{this.Href}\">{this.TextContent}</a>";
        }
    };
}
