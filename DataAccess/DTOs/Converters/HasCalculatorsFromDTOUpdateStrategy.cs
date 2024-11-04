using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class HasCalculatorsFromDTOUpdateStrategy : IUpdateStrategy<IHasCalculators>
    {
        private IConvertStrategy<ICalculator, ICalculator> convertStrategy;

        public HasCalculatorsFromDTOUpdateStrategy(IConvertStrategy<ICalculator, ICalculator> convertStrategy)
        {
            this.convertStrategy = convertStrategy;
        }

        public void Update(IHasCalculators targetObject, IHasCalculators sourceObject)
        {
            targetObject.Calculators.Clear();
            foreach (var item in sourceObject.Calculators)
            {
                var newItem = convertStrategy.Convert(item);
                targetObject.Calculators.Add(newItem);
            }
        }
    }
}
