using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Parameters
{
    public class ArrayParameter<T> : IArrayParameter<T>
    {
		private string[] columnLabels;
		public string[] ColumnLabels
		{
			get { return columnLabels; }
			set
			{
				var labelCount = value.Count();
				var columnCount = Data.GetLength(1);
				if (labelCount != columnCount)
					{
					throw new StructureHelperException(ErrorStrings.IncorrectValue + $" of label count: expected {columnCount}, but was {labelCount}");
					}
				columnLabels = value;
			}
		}

        public T[,] Data { get; private set; }

        public ArrayParameter(int rowCount, int columnCount, string[] columnLabels = null)
        {
			Data = new T[rowCount, columnCount];
			if (columnLabels is not null) { ColumnLabels = columnLabels; }
        }
    }
}
