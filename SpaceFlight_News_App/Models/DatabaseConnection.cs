// ==============================================================================
// Filename: DatabaseConnection.cs
// 
// Author: Robert Howell
// Date: 6/24/2024
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
        private readonly Spaceflight_News_MySQLContext _context;


        public DatabaseConnection(Spaceflight_News_MySQLContext context)
        {
            _context = context;
        }

        public void getModel()
        {
            using (var context = _context)
            {
                Console.WriteLine(context.Model);
                // Now you can use the connection to execute your queries
            }
        }
    };
}
