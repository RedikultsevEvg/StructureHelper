using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.AddMaterialWindow;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.MainWindow.Materials
{
    public class HeadMaterialBaseViewModel : OkCancelViewModelBase
    {
        private readonly IHeadMaterial headMaterial;
        private ICommand showSafetyFactors;
        private ICommand editColorCommand;

        public HeadMaterialBaseViewModel(IHeadMaterial headMaterial)
        {
            this.headMaterial = headMaterial;
        }

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

        public HelperMaterialViewModel HelperMaterialViewModel { get; set; }

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
    }
}
