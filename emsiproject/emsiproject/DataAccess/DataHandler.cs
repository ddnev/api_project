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
                        // Execute query
                        using (SQLiteCommand command = ComposeQuery(connection, name, abbr, display_id))
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

        private SQLiteCommand ComposeQuery(SQLiteConnection connection, string name, string abbr, string display_id)
        {
            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "SELECT DISTINCT name, abbr, display_id FROM areas WHERE (@name is null OR name LIKE @name2) AND (@abbr is null OR abbr LIKE @abbr2) AND (@display_id is null OR display_id LIKE @display_id2)";
            command.Parameters.Add(new SQLiteParameter("@name", name));
            command.Parameters.Add(new SQLiteParameter("@abbr", abbr));
            command.Parameters.Add(new SQLiteParameter("@display_id", display_id));
            command.Parameters.AddWithValue("@name2", name + "%");
            command.Parameters.AddWithValue("@abbr2", abbr + "%");
            command.Parameters.AddWithValue("@display_id2", display_id + "%");

            string query = command.CommandText;
            foreach (SQLiteParameter p in command.Parameters)
            {
                query = query.Replace(p.ParameterName, p.Value == null ? "null" : p.Value.ToString());
            }

            return command;
        }

    }
}
