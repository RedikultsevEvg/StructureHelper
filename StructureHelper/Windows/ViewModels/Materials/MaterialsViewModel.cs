using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Models.Materials;
using StructureHelper.Services.Settings;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Materials
{
    public class MaterialsViewModel : SelectedItemViewModel<IHeadMaterial>
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
            var paramType = (MaterialType)parameter;
            if (paramType == MaterialType.Concrete) { AddConcrete(); }
            else if (paramType == MaterialType.Reinforcement) { AddReinforcement(); }
            else if (paramType == MaterialType.Elastic) { AddElastic(); }
            else if (paramType == MaterialType.CarbonFiber) { AddCarbonFiber(); }
            else if (paramType == MaterialType.GlassFiber) { AddGlassFiber(); }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $". Expected: {typeof(MaterialType)}, Actual type: {nameof(paramType)}");
            GlobalRepository.Materials.Create(NewItem);
            base.AddMethod(parameter);
        }
        public override void DeleteMethod(object parameter)
        {
            var primitives = repository.Primitives;
            var primitivesWithMaterial = primitives.Where(x => x.HeadMaterial == SelectedItem);
            int primitivesCount = primitivesWithMaterial.Count();
            if (primitivesCount > 0)
            {
                MessageBox.Show("Some primitives reference to this material", "Material can not be deleted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var dialogResult = MessageBox.Show("Delete material?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                GlobalRepository.Materials.Delete(SelectedItem.Id);
                base.DeleteMethod(parameter);
            }
        }
        public override void EditMethod(object parameter)
        {
            var copyObject = GlobalRepository.Materials.GetById(SelectedItem.Id).Clone() as IHeadMaterial;
            var wnd = new HeadMaterialView(SelectedItem);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                GlobalRepository.Materials.Update(SelectedItem);
            }
            else
            {
                var updateStrategy = new MaterialUpdateStrategy();
                updateStrategy.Update(SelectedItem, copyObject);
            }
            base.EditMethod(parameter);
        }
        public override void CopyMethod(object parameter)
        {
            NewItem = SelectedItem.Clone() as IHeadMaterial;
            NewItem.Name = $"{NewItem.Name} copy";
            GlobalRepository.Materials.Create(NewItem);
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
            var wnd = new HeadMaterialsView(repository);
            wnd.ShowDialog();
            Refresh();
        }
    }
}
