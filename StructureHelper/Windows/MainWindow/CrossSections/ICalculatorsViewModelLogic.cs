using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public interface ICalculatorsViewModelLogic : ICRUDViewModel<ICalculator>
    {
        RelayCommand Run { get; }
    }
}