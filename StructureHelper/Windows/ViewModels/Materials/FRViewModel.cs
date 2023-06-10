using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class FRViewModel : ElasticViewModel
    {
        public FRViewModel(IFRMaterial material) : base(material)
        {
            
        }
    }
}
