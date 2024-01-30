using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StructureHelper.Windows.ViewModels
{
    internal static class SourceTargetFactory
    {
        const string ColoredItemTemplate = "ColoredItemTemplate";
        const string SimpleItemTemplate = "SimpleItemTemplate";
        public static SourceTargetVM<PrimitiveBase> GetSourceTargetVM(IEnumerable<INdmPrimitive> allowedPrimitives, IEnumerable<INdmPrimitive> targetPrimitives)
        {
            var sourceViewPrimitives = PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(allowedPrimitives);
            var result = new SourceTargetVM<PrimitiveBase>();
            if (targetPrimitives is not null)
            {
                var viewPrimitives = sourceViewPrimitives.Where(x => targetPrimitives.Contains(x.GetNdmPrimitive()));
                result.SetTargetItems(viewPrimitives);
            }
            result.SetSourceItems(sourceViewPrimitives);
            result.ItemDataDemplate = GetDataTemplate(ColoredItemTemplate);
            return result;
        }

        public static SourceTargetVM<IForceAction> GetSourceTargetVM(IEnumerable<IForceAction> allowedCombinations, IEnumerable<IForceAction> targetCombinations)
        {
            var result = new SourceTargetVM<IForceAction>();
            result.SetTargetItems(targetCombinations);
            result.SetSourceItems(allowedCombinations);
            result.ItemDataDemplate = GetDataTemplate(SimpleItemTemplate);
            return result;
        }

        private static DataTemplate GetDataTemplate(string dataTemplateName)
        {
            DataTemplate dataTemplate;
            try
            {
                dataTemplate = Application.Current.Resources[dataTemplateName] as DataTemplate;
            }
            catch (Exception ex)
            {
                throw new StructureHelperException(ErrorStrings.ObjectNotFound);
            }
            if (dataTemplate is null)
            {
                throw new StructureHelperException(ErrorStrings.NullReference);
            }
            return dataTemplate;
        }
    }
}
