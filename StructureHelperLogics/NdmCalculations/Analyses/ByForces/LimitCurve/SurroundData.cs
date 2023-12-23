using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
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
    /// Limits of coordinates for workplane
    /// </summary>
    public class SurroundData
    {
        public double XMax { get; set; }
        public double XMin { get; set; }
        public double YMax { get; set; }
        public double YMin { get; set; }
        /// <summary>
        /// Constant value of coordinate in direction, which is normal to specific workplane
        /// </summary>
        public double ConstZ { get; set; }
        /// <summary>
        /// Logic for transformation of 2D worplane to 3D space
        /// </summary>
        public ConstOneDirectionConverter ConvertLogicEntity { get; set; }
        /// <summary>
        /// Returns new instance of class
        /// </summary>
        public SurroundData()
        {
            XMax = 1e7d;
            XMin = -1e7d;
            YMax = 1e7d;
            YMin = -1e7d;
            ConvertLogicEntity = ConvertLogics.ConverterLogics[0];
        }
    }
}
