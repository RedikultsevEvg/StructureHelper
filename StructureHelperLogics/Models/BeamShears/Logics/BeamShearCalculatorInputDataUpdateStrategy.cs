using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class BeamShearCalculatorInputDataUpdateStrategy : IUpdateStrategy<IBeamShearCalculatorInputData>
    {
        private IUpdateStrategy<IHasBeamShearActions>? hasActionUpdateStrategy;
        private IUpdateStrategy<IHasStirrups>? hasStirrupsUpdateStrategy;
        private IUpdateStrategy<IHasBeamShearSections> hasSectionsUpdateStrategy;
        public void Update(IBeamShearCalculatorInputData targetObject, IBeamShearCalculatorInputData sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            InitializeStrategies();
            hasActionUpdateStrategy?.Update(targetObject, sourceObject);
            hasStirrupsUpdateStrategy?.Update(targetObject, sourceObject);

        }

        private void InitializeStrategies()
        {
            hasActionUpdateStrategy ??= new HasBeamShearActionUpdateStrategy();
            hasStirrupsUpdateStrategy ??= new HasStirrupsUpdateStrategy();
            hasSectionsUpdateStrategy ??= new HasBeamShearSectionUpdateStrategy();
        }
    }
}
