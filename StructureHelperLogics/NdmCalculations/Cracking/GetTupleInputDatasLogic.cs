using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class GetTupleInputDatasLogic : IGetTupleInputDatasLogic
    {
        public List<IForceAction> ForceActions { get; set; }
        public List<INdmPrimitive> Primitives { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms LongTerm { get; set; }
        public CalcTerms ShortTerm { get; set; }
        public UserCrackInputData UserCrackInputData { get; set; }

        public GetTupleInputDatasLogic(List<INdmPrimitive> primitives, List<IForceAction> forceActions, UserCrackInputData userCrackInputData)
        {
            Primitives = primitives;
            ForceActions = forceActions;
            UserCrackInputData = userCrackInputData;
        }

        public List<TupleCrackInputData> GetTupleInputDatas()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);

            List<TupleCrackInputData> resultList = new();
            CheckInputData();
            foreach (var action in ForceActions)
            {
                var tuple = GetTuplesByActions(action);
                if (tuple.isValid == false)
                {
                    resultList.Add(new TupleCrackInputData()
                    {
                        IsValid = false,
                    });
                }
                else
                {
                    resultList.Add(new TupleCrackInputData()
                    {
                        IsValid = true,
                        TupleName = action.Name,
                        LongTermTuple = tuple.LongTuple,
                        ShortTermTuple = tuple.ShortTuple,
                        Primitives = Primitives,
                        UserCrackInputData = UserCrackInputData
                    });
                }
            }
            TraceLogger?.AddMessage(LoggerStrings.CalculationHasDone);
            return resultList;
        }

        private void CheckInputData()
        {
            if (ForceActions is null)
            {
                TraceLogger?.AddMessage("Force action list is null", TraceLogStatuses.Error);
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": {nameof(ForceActions)} is null");
            }
        }

        private (bool isValid, IForceTuple? LongTuple, IForceTuple? ShortTuple) GetTuplesByActions(IForceAction action)
        {
            IForceTuple longTuple, shortTuple;
            var combinations = action.GetCombinations().DesignForces;
            try
            {
                longTuple = GetTupleByCombination(combinations, LimitState, LongTerm);
                TraceLogger?.AddMessage("Long term force combination");
                TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(longTuple));
                shortTuple = GetTupleByCombination(combinations, LimitState, ShortTerm);
                TraceLogger?.AddMessage("Short term force combination");
                TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(shortTuple));
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage("Force combination is not obtained: \n" + ex, TraceLogStatuses.Error);
                return (false, null, null);
            }
            return (true, longTuple, shortTuple);
        }

        private static IForceTuple GetTupleByCombination(List<IDesignForceTuple> combinations, LimitStates limitState, CalcTerms calcTerm)
        {
            return combinations
                .Where(x => x.LimitState == limitState & x.CalcTerm == calcTerm)
                .Single()
                .ForceTuple;
        }
    }
}
