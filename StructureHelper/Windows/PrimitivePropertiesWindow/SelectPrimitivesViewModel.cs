using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;
using System.Windows;

namespace StructureHelper.Windows.PrimitivePropertiesWindow
{
    public class SelectPrimitivesViewModel : OkCancelViewModelBase
    {
        public SelectItemsVM<PrimitiveBase> Items { get; }

        public SelectPrimitivesViewModel(IEnumerable<INdmPrimitive> primitives)
        {
            var primitiveViews = PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(primitives);
            Items = new SelectItemsVM<PrimitiveBase>(primitiveViews) { ShowButtons = true };
            Items.ItemDataTemplate = Application.Current.Resources["ColoredItemTemplate"] as DataTemplate;
        }

        public SelectPrimitivesViewModel(IEnumerable<PrimitiveBase> primitives)
        {
            Items = new SelectItemsVM<PrimitiveBase>(primitives) { ShowButtons = true };
            Items.ItemDataTemplate = Application.Current.Resources["ColoredItemTemplate"] as DataTemplate;
        }
    }
}
