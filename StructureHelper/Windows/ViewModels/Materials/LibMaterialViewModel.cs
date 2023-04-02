using LoaderCalculator.Data.Materials;
using StructureHelper.Infrastructure;
using StructureHelper.Windows.AddMaterialWindow;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal abstract class LibMaterialViewModel : HelperMaterialViewModel
    {
        ILibMaterial material;
        ICommand showSafetyFactors;
        SafetyFactorsViewModel safetyFactorsViewModel;

        public ILibMaterialEntity MaterialEntity
        {
            get => material.MaterialEntity;
            set
            {
                material.MaterialEntity = value;
                OnPropertyChanged(nameof(MaterialEntity));
            }
        }
        public abstract IEnumerable<ILibMaterialEntity> MaterialLibrary { get; }
        public SafetyFactorsViewModel SafetyFactors => safetyFactorsViewModel;

        public ICommand ShowSafetyFactors =>
            showSafetyFactors ??= new RelayCommand(o =>
            {
                var wnd = new SafetyFactorsView(material.SafetyFactors);
                wnd.ShowDialog();
                safetyFactorsViewModel = new SafetyFactorsViewModel(material.SafetyFactors);
                OnPropertyChanged(nameof(SafetyFactors));
            });


        public LibMaterialViewModel(ILibMaterial material)
        {
            this.material = material;
            safetyFactorsViewModel = new SafetyFactorsViewModel(material.SafetyFactors);
        }
    }
}
