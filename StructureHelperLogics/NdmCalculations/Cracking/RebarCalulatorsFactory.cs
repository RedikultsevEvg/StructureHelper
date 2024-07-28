using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarCalulatorsFactory : IRebarCalulatorsFactory
    {
        private IRebarCrackInputDataFactory inputFactory;

        public IEnumerable<RebarPrimitive> Rebars { get; set; }
        public TupleCrackInputData InputData { get; set; }
        public double LongLength { get; set; }
        public double ShortLength { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public RebarCalulatorsFactory(IRebarCrackInputDataFactory inputFactory)
        {
            this.inputFactory = inputFactory;
        }

        public RebarCalulatorsFactory() : this(new RebarCrackInputDataFactory()) { }

        public List<IRebarCrackCalculator> GetCalculators()
        {
            List<IRebarCrackCalculator> calculators = new List<IRebarCrackCalculator>();
            foreach (var rebar in Rebars)
            {
                inputFactory.Rebar = rebar;
                inputFactory.InputData = InputData;
                inputFactory.LongLength = LongLength;
                inputFactory.ShortLength = ShortLength;
                var calculator = new RebarCrackCalculator
                {
                    InputData = inputFactory.GetInputData()
                };
                calculators.Add(calculator);
            }
            return calculators;
        }

    }
}
