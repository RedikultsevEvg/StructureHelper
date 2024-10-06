using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Graphs
{
    internal class Graph : IGraph
    {
        public Guid Id { get; private set; }
        public Graph()
        {
          
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
