using System;
using System.Collections.Generic;

namespace API.Models
{
    public class QueriesHelper
    {
        public String Key { get; set; }
        public List<String> Query { get; set; }

        public String ToSql()
        {
            if (Query != null) return String.Join(' ', Query);

            return "";
        }
    }
}
