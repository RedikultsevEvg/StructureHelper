using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.BeamShears;
using System.Collections.Generic;

namespace StructureHelper.Windows.BeamShears
{
    public class StirrupGroupViewModel : OkCancelViewModelBase
    {
        private IStirrupGroup _stirrupGroup;

        public string Name
        {
            get => _stirrupGroup.Name;
            set
            {
                _stirrupGroup.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public BeamStirrupsViewModel Stirrups { get; }

        public StirrupGroupViewModel(IStirrupGroup stirrupGroup)
        {
            _stirrupGroup = stirrupGroup;
            Stirrups = new(_stirrupGroup);
        }
    }
}
