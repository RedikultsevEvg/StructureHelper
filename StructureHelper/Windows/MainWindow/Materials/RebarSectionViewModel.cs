using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.MainWindow.Materials
{
    public class RebarSectionViewModel : ViewModelBase
    {
        private readonly IRebarSection rebarSection;

        public double MinRebarDiameter { get; set; } = 0.003;
        public double MaxRebarDiameter { get; set; } = 0.050;
        public ReinforcementViewModel Material { get; private set; }
        public double Diameter
        {
            get => rebarSection.Diameter;
            set
            {
                try
                {
                    if (value < MinRebarDiameter) { value = MinRebarDiameter; }
                    else if (value > MaxRebarDiameter) { value = MaxRebarDiameter; }
                    rebarSection.Diameter = value;
                }
                catch (Exception ex)
                {

                }          
            }
        }

        public RebarSectionViewModel(IRebarSection rebarSection)
        {
            this.rebarSection = rebarSection;
            Material = new(this.rebarSection.Material) { MaterialLogicVisibility = false };
        }
    }
}
