using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public class FindParameterCalculator : ICalculator, IHasActionByResult
    {
        FindParameterResult result;
        public string Name { get; set; }
        public double StartValue { get; set; }
        public double EndValue { get; set; }
        public Predicate<double> Predicate { get; set; }
        public IAccuracy Accuracy {get;set;}
        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public FindParameterCalculator()
        {
            StartValue = 0d;
            EndValue = 1d;
            Accuracy = new Accuracy() { IterationAccuracy = 0.001d, MaxIterationCount = 1000 };
        }
        public void Run()
        {
            result = new();
            try
            {
                FindMinimumValue(StartValue, EndValue, Predicate);
            }
            catch(Exception ex)
            {
                result.IsValid = false;
                result.Description += ex;
            }
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }

        private void FindMinimumValue(double start, double end, Predicate<double> predicate)
        {
            TraceLogger?.AddMessage($"Calculating parameter by iterations is started,\nrequired precision {Accuracy.IterationAccuracy}");
            if (predicate(end) == false)
            {
                TraceLogger?.AddMessage($"Predicate for end value must be true", TraceLogStatuses.Error);
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": predicate for end value must be true");
            }
            double precision = Accuracy.IterationAccuracy;
            int maxIterationCount = Accuracy.MaxIterationCount;
            double current = start;
            double step = (end - start) / 2d;
            int iterationNum = 0;
            while (step > precision)
            {
                TraceLogger?.AddMessage($"Iteration number {iterationNum} is started", TraceLogStatuses.Debug);
                if (predicate(current) == true)
                {
                    TraceLogger?.AddMessage($"Predicate value in {current} is true", TraceLogStatuses.Debug, 50);
                    end = current;
                }
                else
                {
                    TraceLogger?.AddMessage($"Predicate value in {current} is false", TraceLogStatuses.Debug, 50);
                    start = current;
                }
                current = (start + end) / 2d;
                TraceLogger?.AddMessage($"New current value Cur=({start}+{end})/2={current}", TraceLogStatuses.Debug);
                step = (end - start) / 2d;
                TraceLogger?.AddMessage($"New step S={current}", TraceLogStatuses.Debug, 50);
                iterationNum++;

                result.IsValid = false;
                result.IterationNumber = iterationNum;
                result.CurrentAccuracy = step;
                ActionToOutputResults?.Invoke(result);

                if (iterationNum > maxIterationCount)
                {
                    TraceLogger?.AddMessage($"Recuired precision was not achieved, current step {step}, required precision {precision}", TraceLogStatuses.Error);
                    result.Description = "Parameter was not found succefully: \n";
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": violation of iteration count");
                }
            }
            TraceLogger?.AddMessage($"Parameter value Cur={current} was found,\nCalculation has finished succefully");
            result.Parameter = current;
            result.Description = "Parameter was found succefully";
            result.IsValid = true;
            ActionToOutputResults?.Invoke(result);
        }
    }
}
