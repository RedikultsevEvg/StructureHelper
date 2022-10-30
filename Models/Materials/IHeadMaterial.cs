using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelper.Models.Materials
{
    public interface IHeadMaterial
    {
        string Name { get; set; }
        Color Color { get; set; }
        MaterialDefinitionBase Material { get; set; }
    }
}
