using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Models.Sections;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperCommon.Models.Loggers;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public class ForceTupleBucklingLogic : IForceTupleBucklingLogic
    {
        private IProcessorLogic<IForceTuple> eccentricityLogic;
        private BucklingInputData bucklingInputData;

        public ICompressedMember CompressedMember { get; set; }
        public IAccuracy Accuracy { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public IEnumerable<INdmPrimitive> Primitives { get; set; }

        public ForceTupleBucklingLogic(BucklingInputData inputData)
        {
            bucklingInputData = inputData;
        }

        public GenericResult<IForceTuple> GetForceTupleByBuckling()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);

            var tuple = bucklingInputData.ForceTuple.Clone() as IForceTuple;
            
            var bucklingResult = ProcessBuckling(bucklingInputData);

            if (bucklingResult.IsValid != true)
            {
                TraceLogger?.AddMessage(bucklingResult.Description, TraceLogStatuses.Error);
                var tupleResult = new GenericResult<IForceTuple>()
                {
                    IsValid = false,
                    Description = $"Buckling result:\n{bucklingResult.Description}",
                    Value = tuple
                };
                return tupleResult;
            }
            else
            {
                tuple = CalculateBuckling(tuple, bucklingResult);
                TraceLogger?.AddMessage(string.Intern("Force combination with considering of second order effects"));
                TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(tuple));
            }
            var trueTupleResult = new GenericResult<IForceTuple>()
            {
                IsValid = true,
                Description = string.Empty,
                Value = tuple
            };
            return trueTupleResult;
        }

        private IConcreteBucklingResult ProcessBuckling(BucklingInputData inputData)
        {
            IForceTuple resultTuple;
            IForceTuple longTuple;
            if (inputData.CalcTerm == CalcTerms.LongTerm)
            {
                longTuple = inputData.ForceTuple;
            }
            else
            {
                longTuple = GetLongTuple(inputData.Combination.DesignForces, inputData.LimitState);
            }
            TraceLogger?.AddMessage("Get eccentricity for long term load");
            eccentricityLogic = new ProcessEccentricity(CompressedMember, inputData.Ndms, longTuple)
            {
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            longTuple = eccentricityLogic.GetValue();
            var bucklingCalculator = GetBucklingCalculator(inputData.LimitState, inputData.CalcTerm, inputData.ForceTuple, longTuple);
            if (TraceLogger is not null)
            {
                bucklingCalculator.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
            }
            bucklingCalculator.Run();
            var bucklingResult = bucklingCalculator.Result as IConcreteBucklingResult;

            return bucklingResult;
        }

        private IForceTuple GetLongTuple(List<IDesignForceTuple> designForces, LimitStates limitState)
        {
            IForceTuple longTuple;
            try
            {
                longTuple = designForces
                    .Where(x => x.LimitState == limitState & x.CalcTerm == CalcTerms.LongTerm)
                    .Single()
                    .ForceTuple;
            }
            catch (Exception)
            {
                longTuple = new ForceTuple();
            }
            return longTuple;
        }

        private IConcreteBucklingCalculator GetBucklingCalculator(LimitStates limitStates, CalcTerms calcTerms, IForceTuple calcTuple, IForceTuple longTuple)
        {
            var options = new ConcreteBucklingOptions()
            {
                CompressedMember = CompressedMember,
                LimitState = limitStates,
                CalcTerm = calcTerms,
                CalcForceTuple = calcTuple,
                LongTermTuple = longTuple,
                Primitives = Primitives
            };
            var bucklingCalculator = new ConcreteBucklingCalculator(options, Accuracy);
            return bucklingCalculator;
        }

        private ForceTuple CalculateBuckling(IForceTuple calcTuple, IConcreteBucklingResult bucklingResult)
        {
            var newTuple = calcTuple.Clone() as ForceTuple;
            newTuple.Mx *= bucklingResult.EtaFactorAlongY;
            newTuple.My *= bucklingResult.EtaFactorAlongX;
            return newTuple;
        }

    }
}
