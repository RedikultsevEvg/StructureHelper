using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2024 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Input data for calculation of crack for specific force tuple
    /// </summary>
    public class TupleCrackInputData : IInputData, IHasPrimitives
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        public string TupleName { get; set; }
        /// <summary>
        /// Force tuple for long term calculations
        /// </summary>
        public IForceTuple? LongTermTuple { get; set; }
        /// <summary>
        /// Force tuple for short term calculations
        /// </summary>
        public IForceTuple? ShortTermTuple { get; set; }
        /// <inheritdoc/>
        public List<INdmPrimitive>? Primitives { get; set;}
        /// <summary>
        /// Settings ajusted by user
        /// </summary>
        public UserCrackInputData UserCrackInputData { get; set; }
    }
}
