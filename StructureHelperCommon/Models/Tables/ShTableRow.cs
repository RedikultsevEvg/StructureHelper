using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Tables
{
    public class ShTableRow<T> : IShTableRow<T>
    {
        private List<T> elements;

        public int RowSize { get; }

        public ShTableRow(int rowSize)
        {
            if (rowSize <= 0)
            {
                throw new ArgumentException("Row size must be greater than 0.", nameof(rowSize));
            }

            RowSize = rowSize;
            elements = new List<T>(rowSize);
            for (int i = 0; i < rowSize; i++)
            {
                elements.Add(default);
            }
        }

        // Property to access elements in the row
        public List<T> Elements => elements;

        internal void Add(object value)
        {
            throw new NotImplementedException();
        }
    }
}
