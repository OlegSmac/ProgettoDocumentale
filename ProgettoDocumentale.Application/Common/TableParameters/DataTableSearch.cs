using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Common.TableParameters
{
    public class DataTableSearch
    {
        public DataTableSearch()
        {
            Values = new List<string>();
        }

        public string Value { get; set; }
        public ICollection<string> Values { get; set; }
        public string Regex { get; set; }
    }
}
