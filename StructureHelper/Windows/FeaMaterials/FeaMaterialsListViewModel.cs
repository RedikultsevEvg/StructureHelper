using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;

namespace StructureHelper.Windows.FeaMaterials
{
    public class FeaMaterialsListViewModel : SelectItemVM<IFeaMaterial>
    {
        private RelayCommand showDiagram;

        public RelayCommand ShowDiagram => showDiagram ??= new RelayCommand(o => ShowDigramMethod(), o => SelectedItem != null);

        private void ShowDigramMethod()
        {
            if (SelectedItem is null) return;
            if (SelectedItem is IConcreteFeaMaterial concrete)
            {
                var logic = new ConcreteToCDPConvertStrategy();
                var cdp = logic.Convert(concrete);
            }
        }

        public FeaMaterialsListViewModel(List<IFeaMaterial> collection) : base(collection)
        {
            
        }

        public override void AddMethod(object parameter)
        {
            CheckObject.ThrowIfNull(parameter);
            SafetyProcessor.RunSafeProcess<object>(parameter, GetMaterial, $"Error of adding of FEA material");
        }

        public override void EditMethod(object parameter)
        {
            if (SelectedItem is null) { return; }
            SafetyProcessor.RunSafeProcess(EditSelectedItem, $"Error of editing of material Name = {SelectedItem.Name}");
            base.EditMethod(parameter);
        }

        private void EditSelectedItem()
        {
            if (SelectedItem is IElasticFeaMaterial elastic)
            {
                var cloneLogic = new ElasticFeaMaterialCloneStrategy();
                var clone = cloneLogic.GetClone(elastic);
                var window = new ElasticFeaMaterialView(elastic);
                window.ShowDialog();
                if (window.DialogResult != true)
                {
                    var updateLogic = new ElasticFeaMaterialUpdateStrategy();
                    updateLogic.Update(elastic, clone);
                }
            }
            else if (SelectedItem is IConcreteFeaMaterial concrete)
            {

            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(SelectedItem));
            }
        }

        private void GetMaterial(object parameter)
        {
            if (parameter is MaterialType type)
            {
                NewItem = FeaMaterialFactory.GetFeaMaterial(type);
                base.AddMethod(parameter);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(parameter));
            }
        }
    }
}
