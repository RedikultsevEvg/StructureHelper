using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCalculatorInputDataUpdateStrategy : IParentUpdateStrategy<IBeamShearCalculatorInputData>
    {
        private IUpdateStrategy<IHasBeamShearActions>? hasActionUpdateStrategy;
        private IUpdateStrategy<IHasStirrups>? hasStirrupsUpdateStrategy;
        private IUpdateStrategy<IHasBeamShearSections> hasSectionsUpdateStrategy;
        private IUpdateStrategy<IBeamShearDesignRangeProperty> designRangeUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

        public void Update(IBeamShearCalculatorInputData targetObject, IBeamShearCalculatorInputData sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            if (UpdateChildren)
            {
                InitializeStrategies();
                hasActionUpdateStrategy?.Update(targetObject, sourceObject);
                hasSectionsUpdateStrategy?.Update(targetObject, sourceObject);
                hasStirrupsUpdateStrategy?.Update(targetObject, sourceObject);
                CheckObject.IsNull(sourceObject.DesignRangeProperty);
                CheckObject.IsNull(targetObject.DesignRangeProperty);
                designRangeUpdateStrategy.Update(targetObject.DesignRangeProperty, sourceObject.DesignRangeProperty);
            }
            targetObject.CodeType = sourceObject.CodeType;
        }

        private void InitializeStrategies()
        {
            hasActionUpdateStrategy ??= new HasBeamShearActionUpdateStrategy();
            hasStirrupsUpdateStrategy ??= new HasStirrupsUpdateStrategy();
            hasSectionsUpdateStrategy ??= new HasBeamShearSectionUpdateStrategy();
            designRangeUpdateStrategy ??= new BeamShearDesignRangePropertyUpdateStrategy();
        }
    }
}
