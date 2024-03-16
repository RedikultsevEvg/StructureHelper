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
        public ArrayParameter(int rowCount, List<string> columnLabels) : this(rowCount, columnLabels.Count, columnLabels) { }
		public void AddArray(IArrayParameter<T> array)
		{
			var rowCount = array.Data.GetLength(0);
            int existingRowCount = this.Data.GetLength(0);
            if (rowCount != existingRowCount)
			{
				throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": number of rows in new array {rowCount} is not equal existing row count {existingRowCount}");
			}
			var existingColumnCount = this.Data.GetLength(1);
			var newColumnCount = array.Data.GetLength(1);
			var totalCount = existingColumnCount + newColumnCount;
			var newData = new T[rowCount, totalCount];
			var lackColumns = existingColumnCount - ColumnLabels.Count;
			if (lackColumns > 0)
			{
				for (int i = 0; i < lackColumns; i++)
				{
					ColumnLabels.Add(string.Empty);
				}
			}
			ColumnLabels.AddRange(array.ColumnLabels);
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < existingColumnCount; j++)
				{
					newData[i, j] = Data[i, j];
				}
                for (int j = 0; j < newColumnCount; j++)
                {
                    newData[i, existingColumnCount + j] = array.Data[i,j];
                }
            }
			Data = newData;
		}

		public void AddRow(int rowNumber, IEnumerable<T> values)
        {
            CheckParams(rowNumber, values);
			var valueList = values.ToList();
			for (int i = 0; i < values.Count(); i++)
			{
				Data[rowNumber, i] = valueList[i];
			}
        }

        private void CheckParams(int rowNumber, IEnumerable<T> values)
        {
            int rowCount = Data.GetLength(0) - 1;
            if (rowNumber > rowCount)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": number of rows {rowNumber} is greater than length of array {rowCount}");
            }
            if (values.Count() != Data.GetLength(1))
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": number of colums in values {values.Count()} is not equal existing column count {Data.GetLength(1)}");
            }
        }
    }
}
