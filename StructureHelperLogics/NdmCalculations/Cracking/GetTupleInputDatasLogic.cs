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
        public IUserCrackInputData UserCrackInputData { get; set; }

        public GetTupleInputDatasLogic(List<INdmPrimitive> primitives,
            List<IForceAction> forceActions,
            IUserCrackInputData userCrackInputData)
        {
            Primitives = primitives;
            ForceActions = forceActions;
            UserCrackInputData = userCrackInputData;
        }

        public List<TupleCrackInputData> GetTupleInputDatas()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);

            List<TupleCrackInputData> resultList = new();
            CheckInputData();
            foreach (var action in ForceActions)
            {
                var tuples = GetCrackTupleByActions(action);
                foreach (var tuple in tuples)
                {
                    var tupleCrackInputDatas = GetTupleCrackInputDatas(action, tuple);
                    resultList.AddRange(tupleCrackInputDatas);
                }
            }
            TraceLogger?.AddMessage(LoggerStrings.CalculationHasDone);
            return resultList;
        }

        private List<TupleCrackInputData> GetTupleCrackInputDatas(IForceAction action, CrackTuple tuple)
        {
            List<TupleCrackInputData> resultList = new();
            if (tuple.IsValid == false)
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

        private List<CrackTuple> GetCrackTupleByActions(IForceAction action)
        {
            var combinations = action.GetCombinations();
            List<CrackTuple> crackTuples = new();
            foreach (var item in combinations)
            {
                var crackTuple = GetCrackTupleByCombination(item.DesignForces);
                crackTuples.Add(crackTuple);
            }

            return crackTuples;
        }

        private CrackTuple GetCrackTupleByCombination(List<IDesignForceTuple> combination)
        {
            IForceTuple longTuple, shortTuple;
            try
            {
                longTuple = GetTupleByCombination(combination, LimitState, LongTerm);
                TraceLogger?.AddMessage("Long term force combination");
                TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(longTuple));
                shortTuple = GetTupleByCombination(combination, LimitState, ShortTerm);
                TraceLogger?.AddMessage("Short term force combination");
                TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(shortTuple));
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage("Force combination is not obtained: \n" + ex, TraceLogStatuses.Error);
                return new CrackTuple()
                {
                    IsValid = false
                };
            }

            var result = new CrackTuple()
            {
                IsValid = true,
                LongTuple = longTuple,
                ShortTuple = shortTuple
            };
            return result;
        }

        private static IForceTuple GetTupleByCombination(List<IDesignForceTuple> combinations, LimitStates limitState, CalcTerms calcTerm)
        {
            return combinations
                .Where(x => x.LimitState == limitState & x.CalcTerm == calcTerm)
                .Single()
                .ForceTuple;
        }

        private class CrackTuple
        {
            public bool IsValid { get; set; }
            public IForceTuple? LongTuple { get; set; }
            public IForceTuple? ShortTuple { get; set; }
        }
    }
}
