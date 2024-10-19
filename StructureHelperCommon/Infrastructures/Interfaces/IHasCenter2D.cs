using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Implement center of geometry shape on 2D plane
    /// </summary>
    public interface IHasCenter2D
    {
        /// <summary>
        /// 2D point of center
        /// </summary>
        IPoint2D Center {get;set;}
        /// <summary>
        /// Angle of rotation orbitrary center
        /// </summary>
        double RotationAngle { get; set; }
    }
}
