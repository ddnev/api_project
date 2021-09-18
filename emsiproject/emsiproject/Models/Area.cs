using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace emsiproject.Models
{
    [Keyless]
    public class Area
    {
        public string Name { get; set; }

        public string Abbr { get; set; }

        public string Display_Id { get; set; }

        public int Child { get; set; }

        public int Parent { get; set; }

        public int Aggregation_Path { get; set; }

        public int Level { get; set; }
    }
}
