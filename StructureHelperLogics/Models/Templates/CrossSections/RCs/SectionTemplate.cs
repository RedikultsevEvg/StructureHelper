using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections.RCs
{
    public class SectionTemplate : ICrossSectionTemplate
    {
        IForceLogic forceLogic;
        IMaterialLogic materialLogic;
        IRCGeometryLogic geometryLogic;
        ICalculatorLogic calculatorLogic;
        IEnumerable<INdmPrimitive> primitives;
        IEnumerable<IForceCombinationList> combinations;
        IEnumerable<INdmCalculator> calculators;

        public SectionTemplate(IRCGeometryLogic geometryLogic)
        {
            this.geometryLogic = geometryLogic;
            materialLogic = new MaterialLogic();
            forceLogic = new ForceLogic();
            calculatorLogic = new CalculatorLogic();
        }

        public ICrossSection GetCrossSection()
        {
            ICrossSection section = new CrossSection();
            var repository = section.SectionRepository;
            var materials = materialLogic.GetHeadMaterials();
            geometryLogic.HeadMaterials = materials;
            primitives = geometryLogic.GetNdmPrimitives();
            repository.HeadMaterials.AddRange(materials);
            repository.Primitives.AddRange(primitives);
            combinations = forceLogic.GetCombinationList();
            repository.ForceCombinationLists.AddRange(combinations);
            calculators = calculatorLogic.GetNdmCalculators();
            AddAllForcesToCalculators();
            AddAllPrimitivesToCalculator();
            repository.CalculatorsList.AddRange(calculators);
            return section;
        }

        private void AddAllForcesToCalculators()
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
        private void AddAllPrimitivesToCalculator()
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
