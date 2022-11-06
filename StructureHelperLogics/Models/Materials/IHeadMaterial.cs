using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelper.Models.Materials
{
    public interface IHeadMaterial : ICloneable
    {
        string Name { get; set; }
        Color Color { get; set; }
        IHelperMaterial HelperMaterial { get; set; }
        //MaterialDefinitionBase Material { get; set; }
    }
}
