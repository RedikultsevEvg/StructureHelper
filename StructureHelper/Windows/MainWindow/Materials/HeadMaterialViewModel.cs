using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.AddMaterialWindow;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Materials;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.ViewModels.Materials
{
    public class HeadMaterialViewModel : OkCancelViewModelBase
    {
        private readonly IHeadMaterial headMaterial;
        private readonly HelperMaterialViewModel helperMaterialViewModel;
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
                    if (headMaterial is ILibMaterial libMaterial)
                    {
                        var wnd = new SafetyFactorsView(libMaterial.SafetyFactors);
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
            if (helperMaterial is IConcreteLibMaterial concreteMaterial)
            {
                helperMaterialViewModel = new ConcreteViewModel(concreteMaterial);
            }
            else if (helperMaterial is IReinforcementLibMaterial reinforcementMaterial)
            {
                helperMaterialViewModel = new LibMaterialViewModel<IReinforcementMaterialEntity>(reinforcementMaterial);
            }
            else if (helperMaterial is ISteelLibMaterial steelMaterial)
            {
                helperMaterialViewModel = new LibMaterialViewModel<ISteelMaterialEntity>(steelMaterial);
            }
            else if (helperMaterial is IElasticMaterial elasticMaterial)
            {
                if (helperMaterial is IFRMaterial fRMaterial)
                {
                    helperMaterialViewModel = new FRViewModel(fRMaterial);
                }
                else
                {
                    helperMaterialViewModel = new ElasticViewModel(elasticMaterial);
                }
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $". Expected: {typeof(IHelperMaterial)}, but was: {helperMaterial.GetType()}");
            }
        }
    }
}
