using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.Models.Materials.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionRepositoryCloneStrategy : ICloneStrategy<ICrossSectionRepository>
    {
        private ICloningStrategy cloningStrategy;
        private CrossSectionRepository targetRepository;
        private IUpdateStrategy<IHasForceActions> forcesUpdateStrategy;
        private IUpdateStrategy<IHasHeadMaterials> materialsUpdateStrategy;
        private IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy;
        private IUpdateStrategy<IHasCalculators> calculatorsUpdateStrategy;
        

        public CrossSectionRepositoryCloneStrategy(
            ICloningStrategy cloningStrategy,
            IUpdateStrategy<IHasForceActions> forcesUpdateStrategy,
            IUpdateStrategy<IHasHeadMaterials> materialsUpdateStrategy,
            IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy,
            IUpdateStrategy<IHasCalculators> calculatorsUpdateStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forcesUpdateStrategy = forcesUpdateStrategy;
            this.materialsUpdateStrategy = materialsUpdateStrategy;
            this.primitivesUpdateStrategy = primitivesUpdateStrategy;
            this.calculatorsUpdateStrategy = calculatorsUpdateStrategy;
        }

        public CrossSectionRepositoryCloneStrategy() : this (
            new DeepCloningStrategy(),
            new HasForceActionUpdateCloningStrategy(null),
            new HasMaterialsUpdateCloningStrategy(null),
            new HasPrimitivesUpdateCloningStrategy(null),
            new HasCalculatorsUpdateCloningStrategy(null))
        {      
            forcesUpdateStrategy = new HasForceActionUpdateCloningStrategy(cloningStrategy);
            materialsUpdateStrategy = new HasMaterialsUpdateCloningStrategy(cloningStrategy);
            primitivesUpdateStrategy = new HasPrimitivesUpdateCloningStrategy(cloningStrategy);
            calculatorsUpdateStrategy = new HasCalculatorsUpdateCloningStrategy(cloningStrategy);
        }

        public ICrossSectionRepository GetClone(ICrossSectionRepository sourceObject)
        {
            targetRepository = new();
            forcesUpdateStrategy.Update(targetRepository, sourceObject);
            materialsUpdateStrategy.Update(targetRepository, sourceObject);
            primitivesUpdateStrategy.Update(targetRepository, sourceObject);
            calculatorsUpdateStrategy.Update(targetRepository, sourceObject);
            return targetRepository;
        }
    }
}
