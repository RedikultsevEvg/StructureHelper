﻿using StructureHelperCommon.Models.Calculators;
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
    public class LimitCurveResult : IResult, IiterationResult
    {
        public bool IsValid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IPoint2D> Points { get; set; }
        public int IterationNumber { get; set; }
        public int MaxIterationCount { get; set; }

        public LimitCurveResult()
        {
            Points = new List<IPoint2D>();
        }
    }
}
