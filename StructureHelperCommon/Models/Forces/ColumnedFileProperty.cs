using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ColumnedFileProperty : IColumnedFileProperty
    {
        private IUpdateStrategy<IColumnedFileProperty> updateStrategy;
        /// <inheritdoc/>
        public Guid Id { get; private set; }
        /// <inheritdoc/>
        public string FilePath { get; set; } = string.Empty;
        /// <inheritdoc/>
        public int SkipRowBeforeHeaderCount { get; set; } = 2;
        /// <inheritdoc/>
        public int SkipRowHeaderCount { get; set; } = 1;
        /// <inheritdoc/>
        public double GlobalFactor { get; set; } = 1d;
        /// <inheritdoc/>
        public List<IColumnFileProperty> ColumnProperties { get; } = new();
        public bool SkipWrongRows { get; set; } = false;

        public ColumnedFileProperty(Guid id)
        {
            Id = id;
        }
        public ColumnedFileProperty() : this (Guid.NewGuid())
        {
            
        }

        public IColumnedFileProperty GetClone(IColumnedFileProperty sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            ColumnedFileProperty newItem = new(id);
            updateStrategy ??= new ColumnedFilePropertyUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
