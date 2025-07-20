using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;
using System;

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
                OnPropertyChanged(nameof(Name));
            }
        }
        public double ReinforcementArea
        {
            get => beamShearSection.ReinforcementArea;
            set
            {
                value = Math.Max(value, 0);
                beamShearSection.ReinforcementArea = value;
                OnPropertyChanged(nameof(ReinforcementArea));
            }
        }
        public double CenterCover
        {
            get => beamShearSection.CenterCover;
            set
            {
                value = Math.Max(value, 0);
                beamShearSection.CenterCover = value;
            }
        }
        public IShape Shape { get; }
        public ConcreteViewModel ConcreteMaterial { get; }
        public ReinforcementViewModel ReinforcementMaterial { get; }

        public SectionViewModel(IBeamShearSection beamShearSection)
        {
            this.beamShearSection = beamShearSection;
            ConcreteMaterial = new(beamShearSection.ConcreteMaterial)
            {
                MaterialLogicVisibility = false,
                TensionForULSVisibility = false,
                TensionForSLSVisibility = false,
                HumidityVisibility = false
            };
            Shape = beamShearSection.Shape;
            ReinforcementMaterial = new(beamShearSection.ReinforcementMaterial) { MaterialLogicVisibility = false };
        }
    }
}
