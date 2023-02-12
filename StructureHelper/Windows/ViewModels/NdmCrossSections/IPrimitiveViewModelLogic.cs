using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public interface IPrimitiveViewModelLogic : ICRUDViewModel<PrimitiveBase>
    {
        RelayCommand SetToFront { get; }
        RelayCommand SetToBack { get; }
        int PrimitivesCount { get; }
        void Refresh();
    }
}
