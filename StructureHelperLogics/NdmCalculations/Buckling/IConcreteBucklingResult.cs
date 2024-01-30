using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    /// <summary>
    /// Results of calculation of buckling of reinforced concrete section
    /// </summary>
    public interface IConcreteBucklingResult : IResult
    {
        /// <summary>
        /// Factor of increasing of bending moment (p-delta effect) in the plain XOZ
        /// </summary>
        double EtaFactorAlongX { get; set; }
        /// <summary>
        /// Factor of increasing of bending moment (p-delta effect) in the plain YOZ
        /// </summary>
        double EtaFactorAlongY { get; set; }
    }
}
