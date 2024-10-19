using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackCalculatorUpdateStrategy : IUpdateStrategy<ICrackCalculator>
    {
        private IUpdateStrategy<ICrackCalculatorInputData> inputDataUpdateStrategy;

        public CrackCalculatorUpdateStrategy(IUpdateStrategy<ICrackCalculatorInputData> inputDataUpdateStrategy)
        {
            this.inputDataUpdateStrategy = inputDataUpdateStrategy;
        }
        public CrackCalculatorUpdateStrategy() : this(new CrackInputDataUpdateStrategy()) { }
        public void Update(ICrackCalculator targetObject, ICrackCalculator sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }

            targetObject.Name = sourceObject.Name;
            targetObject.InputData ??= new CrackCalculatorInputData();
            inputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
        }
    }
}
