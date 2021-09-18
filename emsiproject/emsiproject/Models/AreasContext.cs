using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace emsiproject.Models
{
    public class AreasContext : DbContext
    {
        public string DbPath { get; private set; }

        public AreasContext(DbContextOptions<AreasContext> options) : base(options)
        {
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory;

            //if "bin" is present, remove all the path starting from "bin" word
            if (BaseDir.Contains("bin"))
            {
                int index = BaseDir.IndexOf("bin");
                BaseDir = BaseDir.Substring(0, index);
            }

            DbPath = BaseDir + "Data\\areas.sqlite3";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }

        public DbSet<Area> Areas { get; set; }
    }
}
