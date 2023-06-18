using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
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
                base.DeleteMethod(parameter);
            }
        }
        public override void EditMethod(object parameter)
        {
            var wnd = new HeadMaterialView(SelectedItem);
            wnd.ShowDialog();
            base.EditMethod(parameter);
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
