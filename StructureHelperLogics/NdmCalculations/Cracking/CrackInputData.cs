using StructureHelperCommon.Infrastructures.Interfaces;
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
    public class CrackInputData : IInputData, IHasPrimitives, IHasForceCombinations
    {
        /// <inheritdoc/>
        public List<INdmPrimitive> Primitives { get; private set; }
        /// <inheritdoc/>
        public List<IForceAction> ForceActions { get; private set; }
        public UserCrackInputData UserCrackInputData { get; private set; }
        public CrackInputData()
        {
            Primitives = new();
            ForceActions = new();
            UserCrackInputData = new()
            {
                SetSofteningFactor = true,
                SofteningFactor = 1d,
                SetLengthBetweenCracks = true,
                LengthBetweenCracks = 0.4d,
                UltimateLongCrackWidth = 0.0003d,
                UltimateShortCrackWidth = 0.0004d
            };
        }
    }
}
