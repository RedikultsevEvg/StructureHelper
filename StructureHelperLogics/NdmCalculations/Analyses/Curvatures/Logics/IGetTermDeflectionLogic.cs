using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface IGetTermDeflectionLogic : ILogic
    {
        IDeflectionFactor DeflectionFactor { get; set; }
        /// <summary>
        /// Curvature of cross-section form long term load with long term properties of material
        /// </summary>
        IForceTuple LongCurvature { get; set; }
        /// <summary>
        /// Curvature of cross-section form long term load with short term properties of material
        /// </summary>
        IForceTuple ShortLongCurvature { get; set; }
        /// <summary>
        /// Curvature of cross-section form short term load with short term properties of material
        /// </summary>
        IForceTuple ShortFullCurvature { get; set; }
        ICurvatureTermResult GetLongResult();
        ICurvatureTermResult GetShortResult();
    }
}
