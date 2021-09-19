using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace emsiproject.DataAccess
{
    public interface IDataHandler
    {
        string Search(string name, string abbr, string display_id);
    }
}
