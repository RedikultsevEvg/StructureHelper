using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StructureHelper.Windows.ViewModels.PrimitiveProperties
{
    public class SelectPrimitivesViewModel
    {
        public SelectItemsViewModel<PrimitiveBase> Items { get; }

        public SelectPrimitivesViewModel(IEnumerable<INdmPrimitive> primitives)
        {
            var primitiveViews = PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(primitives);
            Items = new SelectItemsViewModel<PrimitiveBase>(primitiveViews) { ShowButtons = true };
            Items.ItemDataDemplate = Application.Current.Resources["ColoredItemTemplate"] as DataTemplate;
        }
    }
}
