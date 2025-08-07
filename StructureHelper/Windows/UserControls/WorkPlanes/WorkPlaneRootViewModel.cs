using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.UserControls.WorkPlanes
{
    public class WorkPlaneRootViewModel : ViewModelBase
    {
        public WorkPlaneConfigViewModel WorkPlaneConfig { get; } = new();
        public PrimitiveCollectionViewModel PrimitiveCollection { get; } = new();
    }
}
