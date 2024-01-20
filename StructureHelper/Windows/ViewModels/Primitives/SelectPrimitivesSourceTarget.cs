using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Properties;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels
{
    public class SelectPrimitivesSourceTarget : SelectItemVM<NamedValue<SourceTargetVM<PrimitiveBase>>>
    {
        public List<INdmPrimitive> AllowedPrimitives { get; set; }
        public override void AddMethod(object parameter)
        {
            var viewModel = SourceTargetFactory.GetSourceTargetVM(AllowedPrimitives, AllowedPrimitives);
            var namedViewModel = new NamedValue<SourceTargetVM<PrimitiveBase>>()
            {
                Name = "New option",
                Value = viewModel
            };
            NewItem = namedViewModel;
            base.AddMethod(parameter);
        }
        public SelectPrimitivesSourceTarget(List<NamedValue<SourceTargetVM<PrimitiveBase>>> collection) : base(collection)
        {
            
        }
    }
}
