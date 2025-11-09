using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IPoint2DRange : ISaveable, ICloneable
    {
        IPoint2D StartPoint { get; set; }
        IPoint2D EndPoint { get; set; }
    }
}
