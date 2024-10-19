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
    public class CrackCalculatorInputData : ICrackCalculatorInputData
    {
        public Guid Id { get; } = new();
        /// <inheritdoc/>
        public List<INdmPrimitive> Primitives { get; private set; } = new();
        /// <inheritdoc/>
        public List<IForceAction> ForceActions { get; private set; } = new();
        public IUserCrackInputData UserCrackInputData { get; set; } = GetNewUserData();

        private static UserCrackInputData GetNewUserData()
        {
            return new UserCrackInputData()
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
