using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.BeamShears
{
    public class SectionViewModel : OkCancelViewModelBase
    {
        private readonly IBeamShearSection beamShearSection;

        public string Name
        {
            get => beamShearSection.Name;
            set
            {
                beamShearSection.Name = value;
            }
        }
        public double CenterCover
        {
            get => beamShearSection.CenterCover;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                beamShearSection.CenterCover = value;
            }
        }
        public IRectangleShape Shape { get; }
        public ConcreteViewModel Material { get; }

        public SectionViewModel(IBeamShearSection beamShearSection)
        {
            this.beamShearSection = beamShearSection;
            Material = new(beamShearSection.Material)
            {
                MaterialLogicVisibility = false,
                TensionForULSVisibility = false,
                TensionForSLSVisibility = false,
                HumidityVisibility = false
            };
            Shape = beamShearSection.Shape as IRectangleShape;
        }
    }
}
