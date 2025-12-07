using netDxf.Entities;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class CrossSectionRepositoryCheckLogic : CheckEntityLogic<ICrossSectionRepository>
    {
        private bool result;
        private
        ICheckEntityLogic<IHasForcesAndPrimitives> primitivesCheckLogic;
        ICheckEntityLogic<IHasForcesAndPrimitives> PrimitivesCheckLogic => primitivesCheckLogic ??= new RepositoryHasForcesAndPrimitivesCheckLogic(Entity) { TraceLogger = TraceLogger};
        public override bool Check()
        {
            result = true;
            CheckObject.ThrowIfNull(Entity);
            foreach (var item in Entity.Calculators)
            {
                if (item is IForceCalculator forceCalculator)
                {
                    ProcessCalculatorInputData(forceCalculator.InputData);
                }
                else if (item is ICrackCalculator crackCalculator)
                {
                    ProcessCalculatorInputData(crackCalculator.InputData);
                }
                else if (item is IValueDiagramCalculator valueDiagramCalculator)
                {
                    ProcessCalculatorInputData(valueDiagramCalculator.InputData);
                }
                else if (item is ICurvatureCalculator curvatureCalculator)
                {
                    ProcessCalculatorInputData(curvatureCalculator.InputData);
                }
                else
                {
                    // skip
                }
            }
            return result;
        }

        private void ProcessCalculatorInputData(IHasForcesAndPrimitives inputData)
        {
            PrimitivesCheckLogic.Entity = inputData;
            if (PrimitivesCheckLogic.Check() == false)
            {
                CheckResult += "\n" + PrimitivesCheckLogic.CheckResult;
                result = false;
            }
        }
    }
}
