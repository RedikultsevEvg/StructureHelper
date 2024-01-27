using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Tables
{
    public class ListTable<T>
    {
        private List<IShTableRow<T>> table;

        public int RowSize { get; }

        public ListTable(int rowSize)
        {
            if (rowSize <= 0)
            {
                throw new ArgumentException("Row size must be greater than 0.", nameof(rowSize));
            }

            RowSize = rowSize;
            table = new List<IShTableRow<T>>();
        }

        // Add a new row to the table
        public void AddRow(IShTableRow<T> tableRow)
        {
            table.Add(tableRow);
        }
        public void AddRow()
        {
            table.Add(new ShTableRow<T>(RowSize));
        }

        /// <summary>
        /// Get all rows in the table
        /// </summary>
        /// <returns></returns>
        public List<IShTableRow<T>> GetAllRows()
        {
            return table;
        }

        public List<T> GetElementsFromRow(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < table.Count)
            {
                return table[rowIndex].Elements;
            }
            else
            {
                throw new IndexOutOfRangeException("Row index is out of range.");
            }
        }

        /// <summary>
        /// Set a value at the specified column and row index
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="value"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void SetValue(int columnIndex, int rowIndex, T value)
        {
            if (columnIndex >= 0 && columnIndex < RowSize &&
                rowIndex >= 0 && rowIndex < table.Count)
            {
                table[rowIndex].Elements[columnIndex] = value;
            }
            else
            {
                throw new IndexOutOfRangeException("Column or row index is out of range.");
            }
        }
    }
}
