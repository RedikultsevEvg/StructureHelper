using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Forces
{
    public class PointPrimitiveLogic : ViewModelBase
    {
        public SelectItemsVM<PrimitiveValuePoints> Collection { get; private set; }
        public PointPrimitiveLogic(IEnumerable<PrimitiveBase> primitiveBases)
        {
            List<PrimitiveValuePoints> collection = new();
            foreach (var item in primitiveBases)
            {
                var primitiveValuePoint = new PrimitiveValuePoints(item)
                {
                    PrimitiveBase = item
                };
                collection.Add(primitiveValuePoint);
            }
            Collection = new SelectItemsVM<PrimitiveValuePoints>(collection);
            Collection.InvertSelection();
        }
    }
}
