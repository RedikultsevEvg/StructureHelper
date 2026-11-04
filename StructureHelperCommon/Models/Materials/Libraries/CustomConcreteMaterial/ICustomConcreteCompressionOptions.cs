using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public interface ICustomConcreteCompressionOptions : ISaveable, ICloneable
    {
        double PeakStress { get; set; }
        double PeakStrain { get; set; }
        double LimitStrain { get; set; }
        double DescendingScaleFactor { get; set; }
    }
}
