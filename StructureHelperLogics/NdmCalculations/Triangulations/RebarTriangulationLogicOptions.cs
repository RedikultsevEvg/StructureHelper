﻿using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class RebarTriangulationLogicOptions : ITriangulationLogicOptions
    {
        public ITriangulationOptions triangulationOptions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IPoint2D Center { get; }
        /// <inheritdoc />
        public double Area { get; }
        public StrainTuple Prestrain { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }
        public INdmPrimitive HostPrimitive { get; set; }


        /// <inheritdoc />
        public RebarTriangulationLogicOptions(RebarNdmPrimitive primitive)
        {
            Center = primitive.Center.Clone() as Point2D;
            Area = primitive.Area;
            HeadMaterial = primitive.NdmElement.HeadMaterial;
            HostPrimitive = primitive.HostPrimitive;
            Prestrain = ForceTupleService.SumTuples(primitive.NdmElement.UsersPrestrain, primitive.NdmElement.AutoPrestrain) as StrainTuple;
        }
    }
}
