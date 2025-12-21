using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking.CheckLogics
{
    /// <summary>
    /// Checks whether a collection of NDM primitives contains
    /// at least one material that supports crack appearance.
    /// </summary>
    public class PrimitivesForCrackMaterialCheckLogic
        : CheckEntityLogic<IEnumerable<INdmPrimitive>>
    {
        public override bool Check()
        {
            if (Entity is null)
            {
                TraceMessage("Collection of primitives is null");
                return false;
            }

            CheckResult = string.Empty;

            bool hasCrackMaterial = Entity.Any(primitive =>
                primitive?.NdmElement?.HeadMaterial?
                    .GetLoaderMaterial(LimitStates.SLS, CalcTerms.ShortTerm)
                is ICrackMaterial);

            if (!hasCrackMaterial)
            {
                TraceMessage(
                    "Collection of primitives does not have material which supports cracking");
            }

            return hasCrackMaterial;
        }
    }
}

