using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperLogics.Models.Templates.CrossSections
{
    internal class CalculatorLogic : ICalculatorLogic
    {
        public IEnumerable<ICalculator> GetNdmCalculators()
        {
            var calculators = new List<ICalculator>();
            ForceCalculator forceCalculator = GetForceCalculator();
            calculators.Add(forceCalculator);
            CrackCalculator crackCalculator = GetCrackCalculator();
            calculators.Add(crackCalculator);
            ValueDiagramCalculator diagramCalculator = GetDiagramCalculator();
            calculators.Add(diagramCalculator);
            return calculators;
        }

        private static ValueDiagramCalculator GetDiagramCalculator()
        {
            ValueDiagramCalculator diagramCalculator = new(Guid.NewGuid()) { Name = "New value diagram calcualtor"};
            ValueDiagramEntity diagramEntity = new(Guid.NewGuid()) { Name = "New diagram" };
            diagramEntity.ValueDiagram.Point2DRange.StartPoint.Y = 0.25;
            diagramEntity.ValueDiagram.Point2DRange.EndPoint.Y = - 0.25;
            diagramCalculator.InputData.Diagrams.Add(diagramEntity);
            return diagramCalculator;
        }

        private static ForceCalculator GetForceCalculator()
        {
            return new ForceCalculator(Guid.NewGuid())
            {
                Name = "New Force Calculator",
                TraceLogger = new ShiftTraceLogger()
            };
        }

        private static CrackCalculator GetCrackCalculator()
        {
            var newInputData = new CrackCalculatorInputData();
            var checkLogic = new CheckCrackCalculatorInputDataLogic
            {
                Entity = newInputData
            };
            checkLogic.Entity = newInputData;
            var crackCalculator = new CrackCalculator(checkLogic, new CrackCalculatorUpdateStrategy(), null)
            {
                Name = "New Crack Calculator",
                TraceLogger = new ShiftTraceLogger()
            };
            crackCalculator.InputData = newInputData;
            return crackCalculator;
        }
    }
}
