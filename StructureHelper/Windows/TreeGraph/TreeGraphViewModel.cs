using NLog.Common;
using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.TreeGraph
{
    public class TreeGraphViewModel : ViewModelBase
    {
        private List<IOneVariableFunction> nodes;
        public List<IOneVariableFunction> Nodes { get; set; }
        public TreeGraphViewModel(IOneVariableFunction function)
        {
            Nodes = new List<IOneVariableFunction>();
            Nodes.Add(function);
        }
    }
}
