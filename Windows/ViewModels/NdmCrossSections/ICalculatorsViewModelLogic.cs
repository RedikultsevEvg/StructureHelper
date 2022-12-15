using StructureHelper.Infrastructure;
using StructureHelperLogics.NdmCalculations.Analyses;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public interface ICalculatorsViewModelLogic : ICRUDViewModel<INdmCalculator>
    {
        RelayCommand Run { get; }

    }
}