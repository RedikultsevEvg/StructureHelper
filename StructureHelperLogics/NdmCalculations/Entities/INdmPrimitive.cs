﻿using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Entities
{
    public interface INdmPrimitive
    {
        ICenter Center { get; set; }
        IShape Shape { get; set; }
        IPrimitiveMaterial PrimitiveMaterial {get;set;}
        double NdmMaxSize { get; set; }
        int NdmMinDivision { get; set; }
    }
}
