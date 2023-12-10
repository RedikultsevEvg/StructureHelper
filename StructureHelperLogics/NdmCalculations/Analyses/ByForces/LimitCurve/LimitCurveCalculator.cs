using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurveCalculator : ICalculator, IHasActionByResult
    {
        private LimitCurveResult result;
        private List<IPoint2D> surroundList;
        private List<IPoint2D> factoredList;
        private ILimitCurveLogic limitCurveLogic;

        public string Name { get; set; }
        public SurroundData SurroundData { get; set; }
        public ISurroundProc SurroundProcLogic { get; set; }

        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get; set; }

        public LimitCurveCalculator(ILimitCurveLogic limitCurveLogic)
        {
            this.limitCurveLogic = limitCurveLogic;
            SurroundData = new();
            SurroundProcLogic = new RectSurroundProc();
        }

        public LimitCurveCalculator(Predicate<IPoint2D> limitPredicate)
            : this(new LimitCurveLogic(limitPredicate))
        {
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            result = new LimitCurveResult();
            result.IsValid = true;
            SurroundProcLogic.SurroundData = SurroundData;
            surroundList = SurroundProcLogic.GetPoints();
            try
            {
                limitCurveLogic.ActionToOutputResults = GetCurrentStepNumber;
                factoredList = limitCurveLogic.GetPoints(surroundList);
                result.Points = factoredList;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex.Message;
            }
        }
        
        private void GetCurrentStepNumber(IResult calcResult)
        {
            if (calcResult is not FindParameterResult)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(FindParameterResult), calcResult));
            }
            var parameterResult = calcResult as FindParameterResult;
            result.IterationNumber = parameterResult.IterationNumber;
            ActionToOutputResults?.Invoke(result);
        }
    }
}
