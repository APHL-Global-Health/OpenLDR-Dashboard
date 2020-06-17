using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Models
{
    public class QueriesHelper
    {
        public String Key { get; set; }
        public List<String> Query { get; set; }

        public String ToSql()
        {
            if (Query != null) return String.Join(" ", Query);

            return "";
        }
    }
}
