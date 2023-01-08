using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal interface ISafetyFactorViewModel<TItem> : ICRUDViewModel<TItem>
    {
        RelayCommand ShowPartialFactors { get; }
    }
}
