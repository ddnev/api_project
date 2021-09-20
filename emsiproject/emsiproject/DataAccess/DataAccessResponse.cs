using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace emsiproject.DataAccess
{
    public class DataAccessResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsValid { get { return (Errors == null || Errors.Count == 0); } }
        public int RecordsReturned { get; set; }

        public string DbPath { get; set; }
    }
}
