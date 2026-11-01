using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace StructureHelper.Windows.PrimitivePropertiesWindow
{
    /// <summary>
    /// View model for selecting primitives separately for display (Show) and for contour plots (Contours).
    /// </summary>
    public class SelectPrimitivesForShowAndContoursViewModel : OkCancelViewModelBase
    {
        public SelectItemsVM<PrimitiveBase> ShowItems { get; }
        public SelectItemsVM<PrimitiveBase> ContoursItems { get; }

        public SelectPrimitivesForShowAndContoursViewModel(IEnumerable<INdmPrimitive> primitives)
            : this(PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(primitives))
        {
        }

        public SelectPrimitivesForShowAndContoursViewModel(IEnumerable<PrimitiveBase> primitives)
        {
            List<PrimitiveBase> items = primitives.ToList();
            ShowItems = CreateItems(items);
            ContoursItems = CreateItems(items);
        }

        private static SelectItemsVM<PrimitiveBase> CreateItems(IEnumerable<PrimitiveBase> items)
        {
            var selection = new SelectItemsVM<PrimitiveBase>(items)
            {
                ShowButtons = true
            };
            if (Application.Current is not null
                && Application.Current.Resources.Contains("ColoredItemTemplate"))
            {
                selection.ItemDataTemplate = Application.Current.Resources["ColoredItemTemplate"] as DataTemplate;
            }
            return selection;
        }
    }
}
