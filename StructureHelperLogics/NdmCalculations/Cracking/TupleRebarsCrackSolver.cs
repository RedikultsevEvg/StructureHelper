using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class TupleRebarsCrackSolver : ITupleRebarsCrackSolver
    {
        private IRebarCalulatorsFactory calculatorsFactory;

        public IEnumerable<IRebarNdmPrimitive> Rebars { get; set; }
        public TupleCrackInputData InputData { get; set; }
        public double LongLength { get; set; }
        public double ShortLength { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public List<RebarCrackResult> Result { get; private set; }
        public bool IsResultValid { get; private set; }
        public string Description { get; private set; }

        public TupleRebarsCrackSolver(IRebarCalulatorsFactory calulatorsFactory)
        {
            this.calculatorsFactory = calulatorsFactory;
        }

        public TupleRebarsCrackSolver() : this(new RebarCalulatorsFactory()) { }

        public void Run()
        {
            Result = new List<RebarCrackResult>();
            Description = string.Empty;
            var rebarCalculators = GetRebarCalculators();
            List<Task<RebarCrackResult>> tasks = new();
            foreach (var calculator in rebarCalculators)
            {
                if (TraceLogger != null)
                {
                    calculator.TraceLogger = new ShiftTraceLogger();
                }
                tasks.Add(new Task<RebarCrackResult>(() => ProcessCalculator(calculator)));
            }

            var taskArray = tasks.ToArray();
            foreach (var task in taskArray)
            {
                task.Start();
            }
            Task.WaitAll(taskArray);

            if (TraceLogger != null)
            {
                for (int i = 0; i < rebarCalculators.Count(); i++)
                {
                    TraceLogger.TraceLoggerEntries.AddRange(rebarCalculators[i].TraceLogger.TraceLoggerEntries);
                }
            }

            for (int i = 0; i < taskArray.Length; i++)
            {
                Result.Add(taskArray[i].Result);
            }

            if (Result.Any(x => x.IsValid == false))
            {
                IsResultValid = false;
                Description += "\n There not valid results for rebar";
                return;
            }
            IsResultValid = true;
        }


        private RebarCrackResult ProcessCalculator(IRebarCrackCalculator calculator)
        {
            calculator.Run();
            var rebarResult = calculator.Result as RebarCrackResult;
            return rebarResult;
        }

        private List<IRebarCrackCalculator> GetRebarCalculators()
        {
            calculatorsFactory.Rebars = Rebars;
            calculatorsFactory.InputData = InputData;
            calculatorsFactory.LongLength = LongLength;
            calculatorsFactory.ShortLength = ShortLength;
            calculatorsFactory.TraceLogger = TraceLogger?.GetSimilarTraceLogger(0);
            return calculatorsFactory.GetCalculators();
        }
    }
}
