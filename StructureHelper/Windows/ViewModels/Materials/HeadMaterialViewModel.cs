using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.AddMaterialWindow;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class HeadMaterialViewModel : ViewModelBase
    {
        IHeadMaterial headMaterial;
        HelperMaterialViewModel helperMaterialViewModel;
        private ICommand showSafetyFactors;
        private ICommand editColorCommand;

        public string Name
        {
            get => headMaterial.Name;
            set
            {
                headMaterial.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public Color Color
        {
            get => headMaterial.Color;
        }

        public HelperMaterialViewModel HelperMaterialViewModel => helperMaterialViewModel;

        public ICommand ShowSafetyFactors
        {
            get
            {
                return showSafetyFactors ??= new RelayCommand(o =>
                {
                    if (headMaterial is ILibMaterial)
                    {
                        var material = headMaterial as ILibMaterial;
                        var wnd = new SafetyFactorsView(material.SafetyFactors);
                        wnd.ShowDialog();
                    }
                }, o => headMaterial is LibMaterial
                    );
            }
        }

        public ICommand EditColorCommand => editColorCommand ??= new RelayCommand(o => EditColor());

        private void EditColor()
        {
            Color color = headMaterial.Color;
            ColorProcessor.EditColor(ref color);
            headMaterial.Color = color;
            OnPropertyChanged(nameof(Color));
        }

        public HeadMaterialViewModel(IHeadMaterial headMaterial)
        {
            this.headMaterial = headMaterial;
            var helperMaterial = headMaterial.HelperMaterial;
            if (helperMaterial is IConcreteLibMaterial)
            {
                var material = helperMaterial as IConcreteLibMaterial;
                helperMaterialViewModel = new ConcreteViewModel(material);
            }
            else if (helperMaterial is IReinforcementLibMaterial)
            {
                var material = helperMaterial as IReinforcementLibMaterial;
                helperMaterialViewModel = new ReinforcementViewModel(material);
            }
            else if (helperMaterial is IElasticMaterial)
            {
                var material = helperMaterial as IElasticMaterial;
                helperMaterialViewModel = new ElasticViewModel(material);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $". Expected: {typeof(IHelperMaterial)}, but was: {helperMaterial.GetType()}");
            }
        }
    }
}
