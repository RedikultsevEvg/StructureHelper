using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections
{
    public class RCSectionTemplate : ICrossSectionTemplate
    {
        IForceLogic forceLogic;
        IMaterialLogic materialLogic;
        ISectionGeometryLogic geometryLogic;
        ICalculatorLogic calculatorLogic;
        IEnumerable<INdmPrimitive> primitives;
        IEnumerable<IForceCombinationList> combinations;
        IEnumerable<INdmCalculator> calculators;

        public ICrossSection GetCrossSection()
        {
            ICrossSection section = new CrossSection();
            var repository = section.SectionRepository;
            var materials = materialLogic.GetHeadMaterials();
            primitives = geometryLogic.GetNdmPrimitives();
            repository.HeadMaterials.AddRange(materials);
            repository.Primitives.AddRange(primitives);
            combinations = forceLogic.GetCombinationList();
            repository.ForceCombinationLists.AddRange(combinations);
            calculators = calculatorLogic.GetNdmCalculators();
            ProcessCalculatorsSetForce();
            ProcessCalculatorsSetPrimitives();
            repository.CalculatorsList.AddRange(calculators);
            return section;
        }

        private void ProcessCalculatorsSetForce()
        {
            foreach (var calculator in calculators)
            {
                if (calculator is IHasForceCombinations)
                {
                    var forceCalculator = calculator as IHasForceCombinations;
                    forceCalculator.ForceCombinationLists.AddRange(combinations);
                }
            }
        }
        private void ProcessCalculatorsSetPrimitives()
        {
            foreach (var calculator in calculators)
            {
                if (calculator is IHasPrimitives)
                {
                    var primitiveCalculator = calculator as IHasPrimitives;
                    primitiveCalculator.Primitives.AddRange(primitives);
                }
            }
        }
    }
}
