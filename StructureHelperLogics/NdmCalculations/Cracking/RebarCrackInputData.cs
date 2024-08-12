﻿using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <inheritdoc/>
    public class RebarCrackInputData : IRebarCrackInputData
    {
        /// <inheritdoc/>
        public IEnumerable<INdm> CrackableNdmCollection { get; set; }
        /// <inheritdoc/>
        public IEnumerable<INdm> CrackedNdmCollection { get; set; }
        /// <inheritdoc/>
        public ForceTuple ForceTuple { get; set; }
        /// <inheritdoc/>
        public double LengthBeetwenCracks { get; set; }
    }
}
