﻿using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;
using StructureHelper.Services.Primitives;
using StructureHelper.Windows.AddMaterialWindow;
using StructureHelper.Windows.MainWindow;
using StructureHelper.Windows.MainWindow.Materials;
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
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Elastic200);
            material.Name = "New Elastic Material";
            HeadMaterials.Add(material);
            headMaterials.Add(material);
            SelectedMaterial = material;
        }

        public ICommand CopyHeadMaterialCommand { get; set; }
        public ICommand EditColorCommand => editColorCommand ??= new RelayCommand(o => EditColor(), o => SelectedMaterial is not null);
        public ICommand EditCommand => editCommand ??= new RelayCommand(o => Edit(), o => SelectedMaterial is not null);

        private void Edit()
        {
            var wnd = new HeadMaterialView(SelectedMaterial);
            wnd.ShowDialog();
        }

        public ICommand DeleteMaterialCommand { get; set; }

        private ICommand showSafetyFactors;

        public ICommand ShowSafetyFactors
        {
            get
            {
                return showSafetyFactors ??= new RelayCommand(o =>
                    {
                        if (selectedMaterial.HelperMaterial is ILibMaterial)
                        {
                            var material = selectedMaterial.HelperMaterial as ILibMaterial;
                            var wnd = new SafetyFactorsView(material.SafetyFactors);
                            wnd.ShowDialog();
                            OnPropertyChanged(nameof(Items));
                        }
                    }, o=> SelectedLibMaterial != null
                    );
            }
        }

        public ICommand ShowMaterialDiagram
        {
            get
            {
                return showMaterialDiagram ??= new RelayCommand(o =>
                    {
                        var material = selectedMaterial;
                        var wnd = new MaterialDiagramView(headMaterials, material);
                        wnd.ShowDialog();

                    }, o => SelectedMaterial != null
                    );
            }
        }


        private ICommand? addElasticMaterialCommand;
        private ICommand? showMaterialDiagram;
        private ICommand? editColorCommand;
        private ICommand editCommand;

        public ObservableCollection<IHeadMaterial> HeadMaterials { get; private set; }
        public IHeadMaterial SelectedMaterial
        {
            get => selectedMaterial;
            set
            {
                OnPropertyChanged(value, ref selectedMaterial);
                if (selectedMaterial is not null && selectedMaterial.HelperMaterial is ILibMaterial)
                {
                    var libMaterial = selectedMaterial.HelperMaterial as ILibMaterial;
                    selectedLibMaterial = libMaterial.MaterialEntity;
                    OnPropertyChanged(nameof(selectedLibMaterial));
                }             
            }
        }

        public ObservableCollection<IMaterialSafetyFactor> Items
        {
            get
            {
                if (selectedMaterial.HelperMaterial is ILibMaterial)
                {
                    var material = selectedMaterial.HelperMaterial as ILibMaterial;
                    return new ObservableCollection<IMaterialSafetyFactor>(material.SafetyFactors);
                }
                else return null;
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
                return LibMaterialPepository.GetConcreteRepository();
            }
        }

        public IEnumerable<ILibMaterialEntity> LibReinforcementMaterials
        {
            get
            {
                return LibMaterialPepository.GetReinforcementRepository();
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
                    var primitivesWithMaterial = primitives.Where(x => x.NdmElement.HeadMaterial == SelectedMaterial);
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
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            material.Name = "New Concrete";
            HeadMaterials.Add(material);
            headMaterials.Add(material);
            SelectedMaterial = material;
        }

        private void AddReinforcementMaterial()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforcement400);
            material.Name = "New Reinforcement";
            HeadMaterials.Add(material);
            headMaterials.Add(material);
            SelectedMaterial = material;
        }
    }
}
