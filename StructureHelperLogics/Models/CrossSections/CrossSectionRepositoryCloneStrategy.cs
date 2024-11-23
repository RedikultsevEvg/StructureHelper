using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionRepositoryCloneStrategy : ICloneStrategy<ICrossSectionRepository>
    {
        private ICloningStrategy cloningStrategy;
        private CrossSectionRepository targetRepository;
        private IUpdateStrategy<ILimitCurvesCalculatorInputData> limitCurvesInputDataUpdateStrategy;

        public CrossSectionRepositoryCloneStrategy(
            ICloningStrategy cloningStrategy,
            IUpdateStrategy<ILimitCurvesCalculatorInputData> limitCurvesInputDataUpdateStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.limitCurvesInputDataUpdateStrategy = limitCurvesInputDataUpdateStrategy;
        }

        public CrossSectionRepositoryCloneStrategy() : this (
            new DeepCloningStrategy(),
            new LimitCurvesCalculatorInputDataUpdateStrategy())
        {
            
        }

        public ICrossSectionRepository GetClone(ICrossSectionRepository sourceObject)
        {
            targetRepository = new();
            ProcessForces(targetRepository, sourceObject);
            ProcessMaterials(targetRepository, sourceObject);
            ProcessPrimitives(targetRepository, sourceObject);
            ProcessCalculators(targetRepository, sourceObject);
            return targetRepository;
        }

        private void ProcessCalculators(IHasCalculators targetObject, IHasCalculators sourceObject)
        {
            targetObject.Calculators.Clear();
            foreach (var calculator in sourceObject.Calculators)
            {
                var newCalculator = cloningStrategy.Clone(calculator);
                if (calculator is IForceCalculator forceCalculator)
                {
                    ProcessForceCalculator(newCalculator, forceCalculator);
                }
                else if (calculator is CrackCalculator crackCalculator)
                {
                    ProcessCrackCalculator(newCalculator, crackCalculator);
                }
                else if (calculator is  ILimitCurvesCalculator limitCalculator)
                {
                    ProcessLimitCurvesCalculator(newCalculator, limitCalculator);
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(calculator));
                }
                targetRepository.Calculators.Add(newCalculator);
            }
        }

        private void ProcessLimitCurvesCalculator(ICalculator newCalculator, ILimitCurvesCalculator limitCalculator)
        {
            var sourceData = limitCalculator.InputData;
            var targetData = ((ILimitCurvesCalculator)newCalculator).InputData;
            limitCurvesInputDataUpdateStrategy.Update(targetData, sourceData);
            foreach (var series in targetData.PrimitiveSeries)
            {
                List<INdmPrimitive> collection = UpdatePrimitivesCollection(series);
                series.Collection.AddRange(collection);
            }
        }

        private void ProcessCrackCalculator(ICalculator newCalculator, CrackCalculator crackCalculator)
        {
            var sourceData = crackCalculator.InputData;
            var targetData = ((ICrackCalculator)newCalculator).InputData;
            ProcessPrimitives(targetData, sourceData);
            ProcessForces(targetData, sourceData);
        }

        private void ProcessForceCalculator(ICalculator newCalculator, IForceCalculator forceCalculator)
        {
            var sourceData = forceCalculator.InputData;
            var targetData = ((IForceCalculator)newCalculator).InputData;
            ProcessPrimitives(targetData, sourceData);
            ProcessForces(targetData, sourceData);
        }

        private List<INdmPrimitive> UpdatePrimitivesCollection(NamedCollection<INdmPrimitive> series)
        {
            List<INdmPrimitive> collection = new();
            foreach (var item in series.Collection)
            {
                var newItem = cloningStrategy.Clone(item);
                collection.Add(newItem);
            }
            series.Collection.Clear();
            return collection;
        }

        private void ProcessMaterials(IHasHeadMaterials targetObject, IHasHeadMaterials sourceObject)
        {
            targetObject.HeadMaterials.Clear();
            foreach (var material in sourceObject.HeadMaterials)
            {
                var newMaterial = cloningStrategy.Clone(material);
                targetRepository.HeadMaterials.Add(newMaterial);
            }
        }

        private void ProcessForces(IHasForceActions targetObject, IHasForceActions sourceObject)
        {
            targetObject.ForceActions.Clear();
            foreach (var force in sourceObject.ForceActions)
            {
                var newForce = cloningStrategy.Clone(force);
                targetObject.ForceActions.Add(newForce);
            }
        }

        private void ProcessPrimitives(IHasPrimitives targetObject, IHasPrimitives sourceObject)
        {
            targetObject.Primitives.Clear();
            foreach (var primitive in sourceObject.Primitives)
            {
                var newPrimitive = cloningStrategy.Clone(primitive);
                var material = cloningStrategy.Clone(primitive.NdmElement.HeadMaterial);
                newPrimitive.NdmElement.HeadMaterial = material;
                targetObject.Primitives.Add(newPrimitive);
            }
        }
    }
}
