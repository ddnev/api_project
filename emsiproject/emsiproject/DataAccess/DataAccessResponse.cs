using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace emsiproject.DataAccess
{
    public class DataAccessResponse
    {
        public bool Success { get; set; }
        public List<ValidationResult> ValidationFailures { get; set; } = new List<ValidationResult>();
        public bool IsValid { get { return (ValidationFailures == null || ValidationFailures.Count == 0); } }
    }
}
