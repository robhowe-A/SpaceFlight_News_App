// ==============================================================================
// Filename: DatabaseConnection.cs
// 
// Author: Robert Howell
// Date: 6/24/2024
// Edited: 6/22/2026
// Version: 1.0
//
// Description: This file is inert. It is a placeholder for future development.
//
// ==============================================================================

using SpaceFlight_News_App.Data;

namespace SpaceFlight_News_App.Models
{
    public class DatabaseConnection
    {
        private readonly SpaceflightNewsMySqlContext _context;

        public DatabaseConnection(SpaceflightNewsMySqlContext context)
        {
            _context = context;
        }

        public void GetModel()
        {
            using var context = _context;

            Console.WriteLine(context.Model);
            // Now you can use the connection to execute your queries
        }
    };
}
