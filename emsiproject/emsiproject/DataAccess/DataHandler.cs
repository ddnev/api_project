using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace emsiproject.DataAccess
{
    public class DataHandler : IDataHandler
    {
        public string DbPath { get; private set; }
        public DataAccessResponse Response { get; set; }

        public DataHandler()
        {
            // Initialize response object
            Response = new DataAccessResponse();

            DbPath = "Data Source=areas.sqlite3";
            Response.DbPath = DbPath;
        }

        public (string, DataAccessResponse) Search(string name, string abbr, string display_id)
        {
            string jsonString = string.Empty;

            try
            {
                // Open connection to db file
                using (SQLiteConnection connection = new SQLiteConnection(DbPath))
                {
                    connection.Open();

                    try
                    {
                        // Compose query from predicate parameters
                        string predicate = ComposeQuery(name, abbr, display_id);

                        // Execute query
                        using (SQLiteCommand command = new SQLiteCommand(predicate, connection))
                        {
                            // Execute query against db
                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                // Store results
                                DataTable dataTable = new DataTable();
                                dataTable.Load(reader);

                                // Count results for logging
                                Response.RecordsReturned = dataTable.Rows.Count;
                                
                                // Convert to JSON for user consumption
                                jsonString = JsonConvert.SerializeObject(dataTable);
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        Response.Success = false;
                        Response.Errors.Add("DataHandler query failed: " + e.Message);
                    }

                    connection.Close();
                    Response.Success = true;
                }
            }
            catch(Exception e)
            {
                Response.Success = false;
                Response.Errors.Add("DataHandler failed to open connection to database: " + e.Message);
            }

            return (jsonString, Response);

        }

        private string ComposeQuery(string name, string abbr, string display_id)
        {
            // Build command
            string predicate = "SELECT DISTINCT name, abbr, display_id FROM areas WHERE";
            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate + string.Format(" name like '{0}%'", name);
            }

            if (!string.IsNullOrEmpty(abbr))
            {
                if (!string.IsNullOrEmpty(name))
                {
                    predicate = predicate + " and ";
                }
                predicate = predicate + string.Format(" abbr like '{0}%'", abbr);
            }

            if (!string.IsNullOrEmpty(display_id))
            {
                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(abbr))
                {
                    predicate = predicate + " and ";
                }
                predicate = predicate + string.Format(" display_id like '{0}%'", display_id);
            }

            return predicate;
        }

    }
}
