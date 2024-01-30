using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Logic for build collection of points by surround points
    /// </summary>
    public interface ILimitCurveLogic : IHasActionByResult
    {
        /// <summary>
        /// Returns list of points by source collection
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        List<IPoint2D> GetPoints(IEnumerable<IPoint2D> points);
        IShiftTraceLogger? TraceLogger { get; set; }
    }
}
