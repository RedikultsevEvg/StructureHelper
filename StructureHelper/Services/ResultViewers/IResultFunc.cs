﻿using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.ResultViewers
{
    public interface IResultFunc <T>
    {
        string Name { get; }
        T ResultFunction { get; }
        string UnitName { get; set; }
        double UnitFactor { get; }
    }
}
