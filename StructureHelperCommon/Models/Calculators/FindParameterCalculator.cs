using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public class FindParameterCalculator : ICalculator
    {
        FindParameterResult result;
        public string Name { get; set; }
        public double StartValue { get; set; }
        public double EndValue { get; set; }
        public Predicate<double> Predicate { get; set; }
        public IAccuracy Accuracy {get;set;}
        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public FindParameterCalculator()
        {
            StartValue = 0d;
            EndValue = 1d;
            Accuracy = new Accuracy() { IterationAccuracy = 0.001, MaxIterationCount = 1000 };
        }
        public void Run()
        {
            result = new() { IsValid = true};
            try
            {
                result.Parameter = FindMinimumValue(StartValue, EndValue, Predicate);
                result.Description = "Parameter was found succefully";
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

        private double FindMinimumValue(double start, double end, Predicate<double> predicate)
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
                if (predicate(current))
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
                if (iterationNum > maxIterationCount)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": violation of iteration count");
                }
            }

            return current;
        }
    }
}
