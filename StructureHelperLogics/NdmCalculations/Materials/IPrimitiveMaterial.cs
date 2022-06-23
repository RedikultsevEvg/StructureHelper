﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Materials
{
    public interface IPrimitiveMaterial
    {
        string Id { get;}
        MaterialTypes MaterialType { get; }
        string ClassName { get; }
        double Strength { get; }
    }
}