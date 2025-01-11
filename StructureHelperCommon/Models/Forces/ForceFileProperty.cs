using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceFileProperty : IForceFileProperty
    {
        public Guid Id { get; private set; }
        public string FilePath { get; set; } = string.Empty;
        public int SkipRowBeforeHeaderCount { get; set; } = 2;
        public int SkipRowHeaderCount { get; set; } = 1;
        public double GlobalFactor { get; set; } = 1d;
        public IColumnProperty Nz { get; set; } = new ColumnProperty("N") { ColumnIndex = 6};
        public IColumnProperty Mx { get; set; } = new ColumnProperty("My") { ColumnIndex = 8};
        public IColumnProperty My { get; set; } = new ColumnProperty("Mz") { ColumnIndex = 10};

        public ForceFileProperty(Guid id)
        {
            Id = id;
        }
        public ForceFileProperty() : this (Guid.NewGuid())
        {
            
        }
    }
}
