using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackCalculatorUpdateStrategy : IUpdateStrategy<CrackCalculator>
    {
        private IUpdateStrategy<CrackCalculatorInputData> inputDataUpdateStrategy;

        public CrackCalculatorUpdateStrategy(IUpdateStrategy<CrackCalculatorInputData> inputDataUpdateStrategy)
        {
            this.inputDataUpdateStrategy = inputDataUpdateStrategy;
        }
        public CrackCalculatorUpdateStrategy() : this(new CrackInputDataUpdateStrategy()) { }
        public void Update(CrackCalculator targetObject, CrackCalculator sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);

            targetObject.Name = sourceObject.Name;
            targetObject.InputData ??= new();
            inputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
        }
    }
}
