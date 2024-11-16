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
        public LimitStates LimitState { get; set; } = LimitStates.ULS;
        public CalcTerms CalcTerm { get; set; } = CalcTerms.ShortTerm;
        public string FilePath { get; set; } = string.Empty;
        public int SkipRowBeforeHeaderCount { get; set; } = 2;
        public int SkipRowHeaderCount { get; set; } = 1;
        public double GlobalFactor { get; set; } = 1d;
        public IForceColumnProperty Mx { get; set; } = new ForceColumnProperty("N");
        public IForceColumnProperty My { get; set; } = new ForceColumnProperty("My");
        public IForceColumnProperty Nz { get; set; } = new ForceColumnProperty("Mz");

        public ForceFileProperty(Guid id)
        {
            Id = id;
        }
        public ForceFileProperty() : this (Guid.NewGuid())
        {
            
        }
    }
}
