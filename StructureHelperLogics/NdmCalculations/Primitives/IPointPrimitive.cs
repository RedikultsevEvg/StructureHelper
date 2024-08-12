﻿using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <summary>
    /// Geometry primitive of point
    /// </summary>
    public interface IPointPrimitive : INdmPrimitive, IPointShape
    {
    }
}
