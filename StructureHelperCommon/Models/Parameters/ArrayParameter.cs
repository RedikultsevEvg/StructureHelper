using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Parameters
{
    /// <inheritdoc/>
	public class ArrayParameter<T> : IArrayParameter<T>
    {
		private List<string> columnLabels;
		/// <inheritdoc/>
		public List<string> ColumnLabels
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

		/// <inheritdoc/>
        public T[,] Data { get; private set; }

        public ArrayParameter(int rowCount, int columnCount, List<string> columnLabels = null)
        {
			Data = new T[rowCount, columnCount];
			if (columnLabels is not null)
			{
				if (columnLabels.Count > columnCount)
				{
					throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": Count of column labels is greater than count of columns");
				}
				ColumnLabels = columnLabels;
			}
        }
    }
}
