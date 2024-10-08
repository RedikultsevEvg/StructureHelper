using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Analyses
{
    public class DateVersion : IDateVersion
    {
        public Guid Id { get; }
        public DateTime DateTime { get; set; }
        public ISaveable AnalysisVersion { get; set; }

        public DateVersion(Guid id)
        {
            Id = id;
        }

        public DateVersion() : this (Guid.NewGuid())
        {
            
        }
    }
}
