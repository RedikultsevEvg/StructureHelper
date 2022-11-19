using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;
using StructureHelper.Services.Primitives;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.ViewModels.Materials
{
    public class HeadMaterialsViewModel : ViewModelBase
    {
        IHeadMaterialRepository materialRepository;
        IEnumerable<IHeadMaterial> headMaterials;
        IEnumerable<ILibMaterial> libMaterials;
        IHeadMaterial selectedMaterial;
        ILibMaterial selectedLibMaterial;

        public ICommand AddNewMaterialCommand { get; set; }
        public ICommand AddElasticMaterialCommand
        {
            get
            {
                return addElasticMaterialCommand ??
                    (
                    addElasticMaterialCommand = new RelayCommand(o => AddElasticMaterial())
                    );
            }
        }

        private void AddElasticMaterial()
        {
            IHeadMaterial material = new HeadMaterial() { Name = "New elastic material" };
            material.HelperMaterial = new ElasticMaterial() { Modulus = 2e11d, CompressiveStrength = 4e8d, TensileStrength = 4e8d };
            HeadMaterials.Add(material);
            materialRepository.HeadMaterials.Add(material);
            SelectedMaterial = material;
        }

        public ICommand CopyHeadMaterialCommand { get; set; }
        public ICommand EditColorCommand { get; set; }
        public ICommand DeleteMaterialCommand { get; set; }
        public ICommand EditHeadMaterial;

        private ICommand addElasticMaterialCommand;

        public ObservableCollection<IHeadMaterial> HeadMaterials { get; private set; }
        public IHeadMaterial SelectedMaterial
        {
            get => selectedMaterial;
            set
            {
                OnPropertyChanged(value, ref selectedMaterial);
                if (!(selectedMaterial is null))
                {
                    selectedLibMaterial = selectedMaterial.HelperMaterial as ILibMaterial;
                    OnPropertyChanged(nameof(selectedLibMaterial));
                }             
            }
        }

        public string SelectedName
        {
            get => selectedMaterial.Name;
            set
            {
                selectedMaterial.Name = value;
                OnPropertyChanged(nameof(selectedMaterial));
            }
        }

        public ILibMaterial SelectedLibMaterial
        {
            get
            {
                if (selectedLibMaterial is null) { return null; }
                else { return selectedLibMaterial; }
            }
            set
            {
                selectedMaterial.HelperMaterial = value;
            }
        }

        public IEnumerable<ILibMaterial> LibMaterials
        {
            get
            {
                //if (SelectedMaterial is null)
                //{
                //    return null;
                //}
                return libMaterials;//.Where(x => x.MaterialType == (SelectedMaterial.HelperMaterial as ILibMaterial).MaterialType);
            }
        }

        public HeadMaterialsViewModel(IHeadMaterialRepository headMaterialRepository)
        {
            materialRepository = headMaterialRepository;
            headMaterials = materialRepository.HeadMaterials;
            HeadMaterials = new ObservableCollection<IHeadMaterial>();
            foreach (var material in headMaterials)
            {
                HeadMaterials.Add(material);
            }
           libMaterials = materialRepository.LibMaterials;
            AddNewMaterialCommand = new RelayCommand(o => AddNewMaterial(MaterialTypes.Reinforcement));
            CopyHeadMaterialCommand = new RelayCommand(o => CopyMaterial(), o => !(SelectedMaterial is null));
            EditColorCommand = new RelayCommand(o => EditColor(), o=> ! (SelectedMaterial is null));
            DeleteMaterialCommand = new RelayCommand(o => DeleteMaterial(), o => !(SelectedMaterial is null));
        }

        private void CopyMaterial()
        {
            var material = SelectedMaterial.Clone() as IHeadMaterial;
            HeadMaterials.Add(material);
            materialRepository.HeadMaterials.Add(material);
            SelectedMaterial = material;
        }

        private void DeleteMaterial()
        {
            var mainModel = materialRepository.Parent as MainModel;
            var primitivesWithMaterial = mainModel.PrimitiveRepository.Primitives.Where(x => x.HeadMaterial == SelectedMaterial);
            int primitivesCount = primitivesWithMaterial.Count();
            if (primitivesCount > 0)
            {
                MessageBox.Show("Some primitives reference to this material", "Material can not be deleted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var dialogResult = MessageBox.Show("Delete material?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                materialRepository.HeadMaterials.Remove(SelectedMaterial);
                HeadMaterials.Remove(SelectedMaterial);    
            }
        }

        private void EditColor()
        {
            Color color = SelectedMaterial.Color;
            ColorProcessor.EditColor(ref color);
            SelectedMaterial.Color = color;
            OnPropertyChanged(nameof(selectedMaterial.Color));
            OnPropertyChanged(nameof(selectedMaterial));
        }

        private void AddNewMaterial(MaterialTypes materialType)
        {
            IHeadMaterial material = new HeadMaterial() { Name = "New material" };
            material.HelperMaterial = LibMaterials.Where(x => (x.MaterialType == MaterialTypes.Concrete & x.Name.Contains("40"))).First();
            HeadMaterials.Add(material);
            //headMaterials.Append(material);
            materialRepository.HeadMaterials.Add(material);
            SelectedMaterial = material;
        }
    }
}
