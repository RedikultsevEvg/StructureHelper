using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public interface ILineShape : IShape
    {
        ICenter StartPoint { get; set; }
        ICenter EndPoint { get; set; }
        double Thickness { get; set; }
    }
}
