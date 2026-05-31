using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelper.Windows.PrimitivePropertiesWindow;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;


//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.


namespace StructureHelper.Windows.ViewModels.Materials
{
    public class MaterialsViewModel : SelectItemVM<IHeadMaterial>
    {
        ICrossSectionRepository repository;
        private ICommand editMaterialsCommand;

        public ICommand EditMaterialsCommand
        {
            get
            {
                return editMaterialsCommand ??
                    (
                    editMaterialsCommand = new RelayCommand(o => EditHeadMaterials())
                    );
            }
            
        }
        public MaterialsViewModel(ICrossSectionRepository repository) : base(repository.HeadMaterials)
        {
            this.repository = repository;
        }
        public override void AddMethod(object parameter)
        {
            CheckParameters(parameter);
            var parameterType = (MaterialType)parameter;
            if (parameterType == MaterialType.Concrete) { AddConcrete(); }
            else if (parameterType == MaterialType.Reinforcement) { AddReinforcement(); }
            else if (parameterType == MaterialType.Elastic) { AddElastic(); }
            else if (parameterType == MaterialType.CarbonFiber) { AddCarbonFiber(); }
            else if (parameterType == MaterialType.GlassFiber) { AddGlassFiber(); }
            else if (parameterType == MaterialType.Steel) { AddSteel(); }
            else if (parameterType == MaterialType.UserMaterial) { AddUserMaterial(); }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $". Expected: {typeof(MaterialType)}, Actual type: {nameof(parameterType)}");
            base.AddMethod(parameter);
        }

        private void AddUserMaterial()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.UserMaterial);
            material.Name = "New User Material";
            NewItem = material;
        }

        private void AddSteel()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.SteelS345);
            material.Name = "New Steel";
            NewItem = material;
        }

        public override void DeleteMethod(object parameter)
        {
            var primitives = repository.Primitives;
            var primitivesWithMaterial = primitives.Where(x => x.NdmElement.HeadMaterial == SelectedItem);
            int primitivesCount = primitivesWithMaterial.Count();
            if (primitivesCount > 0)
            {
                MessageBox.Show("Some primitives reference to this material", "Material can not be deleted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var dialogResult = MessageBox.Show("Delete material?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                //GlobalRepository.Materials.Delete(SelectedItem.Id);
                base.DeleteMethod(parameter);
            }
        }
        public override void EditMethod(object parameter)
        {
            //var copyObject = GlobalRepository.Materials.GetById(SelectedItem.Id).Clone() as IHeadMaterial;
            var copyObject = SelectedItem.Clone() as IHeadMaterial;
            Window wnd;
            if (SelectedItem.HelperMaterial is ISteelLibMaterial steelHeadMaterial)
            {
                wnd = new SteelMaterialView(SelectedItem);
            }
            else if (SelectedItem.HelperMaterial is IUserMaterial)
            {
                UserMaterialViewModel viewModel = new(SelectedItem);
                wnd = new UserMaterialView(viewModel);
            }
            else
            {
                wnd = new HeadMaterialView(SelectedItem);
            }
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                //
            }
            else
            {
                var updateStrategy = new HeadMaterialUpdateStrategy();
                updateStrategy.Update(SelectedItem, copyObject);
            }
            base.EditMethod(parameter);
        }
        public override void CopyMethod(object parameter)
        {
            NewItem = SelectedItem.Clone() as IHeadMaterial;
            NewItem.Name = $"{NewItem.Name} copy";
            Collection.Add(NewItem);
            Items.Add(NewItem);
            SelectedItem = NewItem;
        }

        private void AddElastic()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Elastic200);
            material.Name = "New Elastic Material";
            NewItem = material;
        }
        private void AddCarbonFiber()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Carbon1400);
            material.Name = "New CFR Material";
            NewItem = material;
        }
        private void AddGlassFiber()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Glass1200);
            material.Name = "New GFR Material";
            NewItem = material;
        }
        private void AddReinforcement()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforcement400);
            material.Name = "New Reinforcement";
            NewItem = material;
        }
        private void AddConcrete()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            material.Name = "New Concrete";
            NewItem = material;
        }
        private void CheckParameters(object parameter)
        {
            if (parameter is null) { throw new StructureHelperException(ErrorStrings.ParameterIsNull); }
            if (parameter is not MaterialType) { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $". Expected: {typeof(MaterialType)} . Actual type: {nameof(parameter)}"); }
        }
        private void EditHeadMaterials()
        {
            SafetyProcessor.RunSafeProcess(delegate()
            {
                var wnd = new HeadMaterialsView(repository);
                wnd.ShowDialog();
                Refresh();
            }, "Error of material propertis exibition");
        }

        private RelayCommand setMaterialToPrimititveCommand;
        public ICommand SetMaterialToPrimititveCommand => setMaterialToPrimititveCommand ??= new RelayCommand(SetMaterialToPrimititve, o => SelectedItem != null);

        private void SetMaterialToPrimititve(object commandParameter)
        {
            if (SelectedItem is null) {  return; }
            var vm = new SelectPrimitivesViewModel(repository.Primitives);
            var wnd = new SelectPrimitivesView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                var selectedNdmPrimitives = vm.Items.CollectionItems.Where(x => x.IsSelected == true).Select(x => x.Item.GetNdmPrimitive());
                foreach (var item in selectedNdmPrimitives)
                {
                    item.NdmElement.HeadMaterial = SelectedItem;
                }
                Refresh();
            }
        }
    }
}
