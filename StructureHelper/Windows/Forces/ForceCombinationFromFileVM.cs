using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Forces
{
    public class ForceCombinationFromFileVM : ForceActionVMBase
    {
        private readonly IForceCombinationFromFile model;
        private ListOfFilesVM files;
        private FactoredCombinationPropertyVM combinationProperty;

        public FactoredCombinationPropertyVM CombinationProperty
        {
            get => combinationProperty; set
            {
                combinationProperty = value;
                OnPropertyChanged(nameof(CombinationProperty));
            }
        }

        public ListOfFilesVM Files
        {
            get => files; set
            {
                files = value;
                OnPropertyChanged();
            }
        }

        public ForceCombinationFromFileVM(IForceCombinationFromFile model) : base(model)
        {
            this.model = model;
            files = new(this.model.ForceFiles);
            CombinationProperty = new FactoredCombinationPropertyVM(model.CombinationProperty);
        }

    }
}
