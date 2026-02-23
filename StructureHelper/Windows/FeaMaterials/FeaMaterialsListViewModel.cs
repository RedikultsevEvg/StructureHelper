using StructureHelper.Infrastructure.Enums;
using StructureHelper.Properties;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;

namespace StructureHelper.Windows.FeaMaterials
{
    public class FeaMaterialsListViewModel : SelectItemVM<IFeaMaterial>
    {
        public FeaMaterialsListViewModel(List<IFeaMaterial> collection) : base(collection)
        {
            
        }

        public override void AddMethod(object parameter)
        {
            CheckObject.ThrowIfNull(parameter);
            SafetyProcessor.RunSafeProcess<object>(parameter, GetMaterial, $"Error of adding of FEA material");
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
