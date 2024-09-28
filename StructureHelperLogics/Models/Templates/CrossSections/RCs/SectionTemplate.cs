﻿using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
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
        IEnumerable<IForceAction> combinations;
        IEnumerable<ICalculator> calculators;

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
            foreach (var primitive in primitives)
            {
                primitive.CrossSection = section;
            }
            repository.HeadMaterials.AddRange(materials);
            repository.Primitives.AddRange(primitives);
            combinations = forceLogic.GetCombinationList();
            repository.ForceActions.AddRange(combinations);
            calculators = calculatorLogic.GetNdmCalculators();
            AddAllForcesToCalculators();
            AddAllPrimitivesToCalculator();
            repository.Calculators.AddRange(calculators);
            return section;
        }

        private void AddAllForcesToCalculators()
        {
            foreach (var calculator in calculators)
            {
                if (calculator is ForceCalculator forceCalculator)
                {
                    forceCalculator.InputData.ForceActions.AddRange(combinations);
                }
                if (calculator is CrackCalculator crackCalculator)
                {
                    crackCalculator.InputData.ForceActions.AddRange(combinations);
                }
            }
        }
        private void AddAllPrimitivesToCalculator()
        {
            foreach (var calculator in calculators)
            {
                if (calculator is ForceCalculator forceCalculator)
                {
                    forceCalculator.InputData.Primitives.AddRange(primitives);
                }
                if (calculator is CrackCalculator crackCalculator)
                {
                    crackCalculator.InputData.Primitives.AddRange(primitives);
                }
            }
        }
    }
}
