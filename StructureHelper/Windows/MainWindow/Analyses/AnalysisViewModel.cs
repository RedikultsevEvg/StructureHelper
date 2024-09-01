using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.MainWindow.Analyses
{
    public class AnalysisViewModel : OkCancelViewModelBase
    {
        private readonly IAnalysis analysis;

        public string Name
        {
            get => analysis.Name;
            set => analysis.Name = value;
        }

        public string Tags
        {
            get => analysis.Tags;
            set => analysis.Tags = value;
        }

        public AnalysisViewModel(IAnalysis analysis)
        {
            this.analysis = analysis;
        }
    }
}
