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

        public DataHandler()
        {
            string BaseDir = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory;

            //if "bin" is present, remove all the path starting from "bin" word
            if (BaseDir.Contains("bin"))
            {
                int index = BaseDir.IndexOf("bin");
                BaseDir = BaseDir.Substring(0, index);
            }

            DbPath = BaseDir + "Data\\areas.sqlite3";
        }

        public string Search(string name, string abbr, string display_id)
        {
            string JSONString = string.Empty;

            // Open connection to db file
            using (SQLiteConnection connection = new SQLiteConnection(DbPath))
            {
                connection.Open();

                // Build command
                string predicate = "SELECT DISTINCT name, abbr, display_id FROM areas WHERE";
                if(!string.IsNullOrEmpty(name))
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
                    if(!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(abbr))
                    {
                        predicate = predicate + " and ";
                    }
                    predicate = predicate + string.Format(" display_id like '{0}%'", display_id);
                }

                using (SQLiteCommand command = new SQLiteCommand(predicate, connection))
                {
                    // Execute query against db
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Store results
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        JSONString = JsonConvert.SerializeObject(dataTable);
                    }
                }

                connection.Close();
            }

            return JSONString;

        }

    }
}
