using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;
using StructureHelper.Services.Primitives;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
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
        IHasHeadMaterials parent;
        List<IHeadMaterial> headMaterials;
        IHeadMaterial selectedMaterial;
        ILibMaterialEntity selectedLibMaterial;

        public ICommand AddNewConcreteMaterialCommand { get;}
        public ICommand AddNewReinforcementMaterialCommand { get; }

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
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Elastic200, ProgramSetting.CodeType);
            material.Name = "New Elastic Material";
            HeadMaterials.Add(material);
            headMaterials.Add(material);
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
                    var libMaterial = selectedMaterial.HelperMaterial as ILibMaterial;
                    selectedLibMaterial = libMaterial.MaterialEntity;
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

        public ILibMaterialEntity SelectedLibMaterial
        {
            get
            {
                if (selectedLibMaterial is null) { return null; }
                else { return selectedLibMaterial; }
            }
            set
            {
                var libMaterial = selectedMaterial.HelperMaterial as ILibMaterial;
                libMaterial.MaterialEntity = value;
            }
        }

        public IEnumerable<ILibMaterialEntity> LibConcreteMaterials
        {
            get
            {
                return LibMaterialPepository.GetConcreteRepository(ProgramSetting.CodeType);
            }
        }

        public IEnumerable<ILibMaterialEntity> LibReinforcementMaterials
        {
            get
            {
                return LibMaterialPepository.GetReinforcementRepository(ProgramSetting.CodeType);
            }
        }

        public HeadMaterialsViewModel(IHasHeadMaterials parent) : this(parent.HeadMaterials)
        {
        }

        public HeadMaterialsViewModel(List<IHeadMaterial> _headMaterials)
        {
            headMaterials = _headMaterials;
            HeadMaterials = new ObservableCollection<IHeadMaterial>();
            foreach (var material in headMaterials)
            {
                HeadMaterials.Add(material);
            }
            AddNewConcreteMaterialCommand = new RelayCommand(o => AddConcreteMaterial());
            AddNewReinforcementMaterialCommand = new RelayCommand(o => AddReinforcementMaterial());
            CopyHeadMaterialCommand = new RelayCommand(o => CopyMaterial(), o => !(SelectedMaterial is null));
            EditColorCommand = new RelayCommand(o => EditColor(), o=> ! (SelectedMaterial is null));
            DeleteMaterialCommand = new RelayCommand(o => DeleteMaterial(), o => !(SelectedMaterial is null));
        }

        private void CopyMaterial()
        {
            var material = SelectedMaterial.Clone() as IHeadMaterial;
            HeadMaterials.Add(material);
            headMaterials.Add(material);
            SelectedMaterial = material;
        }

        private void DeleteMaterial()
        {
            if (parent != null)
            {
                if (parent is IHasPrimitives)
                {
                    var primitives = (parent as IHasPrimitives).Primitives;
                    var primitivesWithMaterial = primitives.Where(x => x.HeadMaterial == SelectedMaterial);
                    int primitivesCount = primitivesWithMaterial.Count();
                    if (primitivesCount > 0)
                    {
                        MessageBox.Show("Some primitives reference to this material", "Material can not be deleted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            var dialogResult = MessageBox.Show("Delete material?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                headMaterials.Remove(SelectedMaterial);
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

        private void AddConcreteMaterial()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40, ProgramSetting.CodeType);
            material.Name = "New Concrete";
            HeadMaterials.Add(material);
            headMaterials.Add(material);
            SelectedMaterial = material;
        }

        private void AddReinforcementMaterial()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforecement400, ProgramSetting.CodeType);
            material.Name = "New Reinforcement";
            HeadMaterials.Add(material);
            headMaterials.Add(material);
            SelectedMaterial = material;
        }
    }
}
