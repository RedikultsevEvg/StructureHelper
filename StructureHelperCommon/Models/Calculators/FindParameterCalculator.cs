using StructureHelperCommon.Infrastructures.Exceptions;
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
            if (predicate(end) == false)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": pridicate for end value must be true");

            }
            double precision = Accuracy.IterationAccuracy;
            int maxIterationCount = Accuracy.MaxIterationCount;
            double current = start;
            double step = (end - start) / 2;
            int iterationNum = 0;
            while (step > precision)
            {
                if (predicate(current) == true)
                {
                    end = current;
                }
                else
                {
                    start = current;
                }

                current = (start + end) / 2;
                step = (end - start) / 2;
                iterationNum++;

                result.IsValid = false;
                result.IterationNumber = iterationNum;
                result.CurrentAccuracy = step;
                ActionToOutputResults?.Invoke(result);

                if (iterationNum > maxIterationCount)
                {
                    result.Description = "Parameter was not found succefully: \n";
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": violation of iteration count");
                }
            }

            result.Parameter = current;
            result.Description = "Parameter was found succefully";
            result.IsValid = true;
            ActionToOutputResults?.Invoke(result);
        }
    }
}
