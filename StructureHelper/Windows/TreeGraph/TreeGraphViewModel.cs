using NLog.Common;
using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.TreeGraph
{
    public class TreeGraphViewModel : ViewModelBase
    {
        private ObservableCollection<IOneVariableFunction> nodes;
        public ObservableCollection<IOneVariableFunction> Nodes { get; set; }
        public TreeGraphViewModel(IOneVariableFunction function)
        {
            Nodes = new ObservableCollection<IOneVariableFunction>();
            Nodes.Add(function);
        }
        
    }
}
