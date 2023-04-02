using StructureHelper.Infrastructure.Enums;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class MaterialsViewModel : CRUDViewModelBase<IHeadMaterial>
    {
        public override void AddMethod(object parameter)
        {
            CheckParameters(parameter);
            var paramType = (MaterialType)parameter;
            if (paramType == MaterialType.Concrete) { AddConcrete(); }
            else if (paramType == MaterialType.Reinforcement) { AddReinforcement(); }
            else if (paramType == MaterialType.Elastic) { AddElastic(); }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $". Expected: {typeof(MaterialType)}, Actual type: {nameof(paramType)}");
            base.AddMethod(parameter);
        }
        public override void DeleteMethod(object parameter)
        {
#error
            //to do delete method
            base.DeleteMethod(parameter);
        }
        public override void EditMethod(object parameter)
        {
            var wnd = new HeadMaterialView(SelectedItem);
            wnd.ShowDialog();
            base.EditMethod(parameter);
        }

        private void AddElastic()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Elastic200, ProgramSetting.CodeType);
            material.Name = "New Elastic Material";
            NewItem = material;
        }
        private void AddReinforcement()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforecement400, ProgramSetting.CodeType);
            material.Name = "New Reinforcement";
            NewItem = material;
        }
        private void AddConcrete()
        {
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40, ProgramSetting.CodeType);
            material.Name = "New Concrete";
            NewItem = material;
        }
        private void CheckParameters(object parameter)
        {
            if (parameter is null) { throw new StructureHelperException(ErrorStrings.ParameterIsNull); }
            if (parameter is not MaterialType) { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $". Expected: {typeof(MaterialType)} . Actual type: {nameof(parameter)}"); }
        }
    }
}
