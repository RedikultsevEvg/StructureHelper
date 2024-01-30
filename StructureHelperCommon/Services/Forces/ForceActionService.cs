using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StructureHelperCommon.Services.Forces
{
    internal static class ForceActionService
    {
        public static List<IDesignForcePair> ConvertCombinationToPairs(IForceCombinationList combinations)
        {
            var resultList = new List<IDesignForcePair>();
            var limitStates = new List<LimitStates>() { LimitStates.ULS, LimitStates.SLS };
            var calcTerms = new List<CalcTerms>() { CalcTerms.ShortTerm, CalcTerms.LongTerm };
            foreach (var limitState in limitStates)
            {
                var tuples = new IForceTuple[2];
                for (int i = 0; i < calcTerms.Count; i++)
                {
                    var forceTupleList = combinations.DesignForces.Where(x => x.LimitState == limitState && x.CalcTerm == calcTerms[i]).Select(x => x.ForceTuple);
                    var sumLongTuple = ForceTupleService.MergeTupleCollection(forceTupleList);
                    tuples[i] = sumLongTuple;
                }
                var pair = new DesignForcePair()
                {
                    Name = combinations.Name,
                    ForcePoint = (IPoint2D)combinations.ForcePoint.Clone(),
                    SetInGravityCenter = combinations.SetInGravityCenter,
                    LimitState = limitState,
                    FullForceTuple = tuples[0],
                    LongForceTuple = tuples[1]
                };
                resultList.Add(pair);
            }
            return resultList;
        }
        public static List<IDesignForcePair> ConvertCombinationToPairs(IForceCombinationByFactor combinations)
        {
            var resultList = new List<IDesignForcePair>();
            var limitStates = new List<LimitStates>() { LimitStates.ULS, LimitStates.SLS };
            var calcTerms = new List<CalcTerms>() { CalcTerms.ShortTerm, CalcTerms.LongTerm };
            foreach (var limitState in limitStates)
            {
                var tuples = new IForceTuple[2];
                for (int i = 0; i < calcTerms.Count; i++)
                {
                    var stateFactor = limitState is LimitStates.SLS ? 1d : combinations.ULSFactor;
                    var termFactor = calcTerms[i] == CalcTerms.ShortTerm ? 1d : combinations.LongTermFactor;
                    var forceTupleList = ForceTupleService.MultiplyTuples(combinations.FullSLSForces, stateFactor * termFactor);
                    tuples[i] = forceTupleList;
            }
                var pair = new DesignForcePair()
                {
                    Name = combinations.Name,
                    ForcePoint = (IPoint2D)combinations.ForcePoint.Clone(),
                    SetInGravityCenter = combinations.SetInGravityCenter,
                    LimitState = limitState,
                    FullForceTuple = tuples[0],
                    LongForceTuple = tuples[1]
                };
                resultList.Add(pair);
            }
            return resultList;
        }

        public static List<IDesignForcePair> ConvertCombinationToPairs(IForceAction forceAction)
        {
            var resultList = new List<IDesignForcePair>();
            if (forceAction is IForceCombinationList)
            {
                var item = forceAction as IForceCombinationList;
                resultList.AddRange(ConvertCombinationToPairs(item));
            }
            else if (forceAction is IForceCombinationByFactor)
            {
                var item = forceAction as IForceCombinationByFactor;
                resultList.AddRange(ConvertCombinationToPairs(item));
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $": expected {typeof(IForceAction)}, but was {forceAction.GetType()}");
            }
            return resultList;
        }

    }
}
