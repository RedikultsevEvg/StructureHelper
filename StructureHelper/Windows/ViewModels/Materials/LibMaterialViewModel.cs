using LoaderCalculator.Data.Materials;
using StructureHelper.Infrastructure;
using StructureHelper.Windows.AddMaterialWindow;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Codes;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class LibMaterialViewModel<T> : HelperMaterialViewModel where T: class, ILibMaterialEntity
    {
        ILibMaterial material;
        ICommand showSafetyFactors;
        SafetyFactorsViewModel safetyFactorsViewModel;
        private ICodeEntity codeEntity;
        private IMaterialLogic materialLogic;

        public ILibMaterialEntity MaterialEntity
        {
            get => material.MaterialEntity;
            set
            {
                material.MaterialEntity = value;
                OnPropertyChanged(nameof(MaterialEntity));
            }
        }
        public ICodeEntity CodeEntity
        {
            get
            {
                return codeEntity;
            }

            set
            {
                codeEntity = value;
                OnPropertyChanged(nameof(CodeEntity));
                FillMaterialKinds();
            }
        }

        public List<IMaterialLogic> MaterialLogics => material.MaterialLogics;
        public IMaterialLogic MaterialLogic { get => material.MaterialLogic; set => material.MaterialLogic = value; }

        private void FillMaterialKinds()
        {
            var materialKinds = ProgramSetting
                .MaterialRepository
                .Repository
                .Where(x => x.Code == codeEntity & x is T);

            MaterialLibrary = new ObservableCollection<T>();
            if (materialKinds.Count() > 0)
            {
                foreach (var item in materialKinds)
                {
                    MaterialLibrary.Add((T)item);
                }
                OnPropertyChanged(nameof(MaterialLibrary));
                material.MaterialEntity = MaterialLibrary.First();
                OnPropertyChanged(nameof(MaterialEntity));
            }
        }

        public ObservableCollection<ICodeEntity> CodeList { get; }
        public ObservableCollection<T> MaterialLibrary { get; private set; }
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
            var selectedMaterialKind = this.material.MaterialEntity;
            CodeList = new ObservableCollection<ICodeEntity>();
            var materialsKind = ProgramSetting.MaterialRepository.Repository
                .Where(x => x is T);
            var codes = materialsKind
                .Select(x => x.Code)
                .Distinct();
            foreach (var item in codes)
            {
                CodeList.Add(item);
            }
            CodeEntity = codes
                .Where(x => x == selectedMaterialKind.Code)
                .Single();
            MaterialEntity = MaterialLibrary
                .Single(x => x.Id == selectedMaterialKind.Id);
            OnPropertyChanged(nameof(MaterialEntity));
            safetyFactorsViewModel = new SafetyFactorsViewModel(material.SafetyFactors);
        }
    }
}
