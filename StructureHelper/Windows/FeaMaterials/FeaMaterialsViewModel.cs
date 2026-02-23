using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.FeaMaterials;

namespace StructureHelper.Windows.FeaMaterials
{
    public class FeaMaterialsViewModel : ViewModelBase
    {
        public FeaMaterialsListViewModel MaterialList { get; }
        public FeaMaterialsViewModel(IFeaMaterialRepository repository)
        {
            MaterialList = new(repository.FeaMaterials);
        }
    }
}
