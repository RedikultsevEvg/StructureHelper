using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Materials
{
    public class HeadMaterialsViewModel : ViewModelBase
    {
        IEnumerable<IHeadMaterial> headMaterials;

        public ICommand AddHeadMaterial;
        public ICommand CopyHeadMaterial;
        public ICommand DeleteHeadMaterial;
        public ICommand EditHeadMaterial;

        public ObservableCollection<IHeadMaterial> HeadMaterials { get; private set; }
        public IHeadMaterial SelectedMaterial { get; set; }

        public HeadMaterialsViewModel(IEnumerable<IHeadMaterial> materials)
        {
            headMaterials = materials;
            HeadMaterials = new ObservableCollection<IHeadMaterial>();
            foreach (var material in headMaterials)
            {
                HeadMaterials.Add(material);
            }
        }
    }
}
