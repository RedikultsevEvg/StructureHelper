using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ColumnedFilePropertyUpdateStrategy : IUpdateStrategy<IColumnedFileProperty>
    {
        /// <inheritdoc/>
        public void Update(IColumnedFileProperty targetObject, IColumnedFileProperty sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            UpdateObjects(targetObject, sourceObject);
        }

        private static void UpdateObjects(IColumnedFileProperty targetObject, IColumnedFileProperty sourceObject)
        {
            targetObject.FilePath = sourceObject.FilePath;
            targetObject.GlobalFactor = sourceObject.GlobalFactor;
            targetObject.SkipRowBeforeHeaderCount = sourceObject.SkipRowBeforeHeaderCount;
            targetObject.SkipRowHeaderCount = sourceObject.SkipRowHeaderCount;
            CheckObject.ThrowIfNull(targetObject.ColumnProperties);
            CheckObject.ThrowIfNull(sourceObject.ColumnProperties);
            targetObject.ColumnProperties.Clear();
            foreach (var item in sourceObject.ColumnProperties)
            {
                IColumnFileProperty clone = (IColumnFileProperty)item.Clone();
                targetObject.ColumnProperties.Add(clone);
            }
        }
    }
}
