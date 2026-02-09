using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoDocumentale.Application.Common.TableParameters
{
    public class DataTableParameters
    {       
        public int TotalCount { get; set; }
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<DataTableColumn> Columns { get; set; }
        public DataTableSearch Search { get; set; }
        public List<DataTableOrder> Order { get; set; }

        /// <summary>
        /// Used for sorting
        /// </summary>
        public void SetColumnName()
        {
            foreach (var item in Order)
            {
                item.Name = Columns[item.Column].Data;
            }
        }
        /// <summary>
        /// Gets the <see cref="DataTableColumn"/> with the specified column name.
        /// </summary>
        /// <value>
        /// The <see cref="DataTableColumn"/>.
        /// </value>
        /// <param name="columnName">The column name.</param>
        /// <returns></returns>
        public DataTableColumn this[string columnName] => Columns.FirstOrDefault(x => x.Data == columnName);
    }
}
