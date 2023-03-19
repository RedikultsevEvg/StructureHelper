using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StructureHelperCommon.Services.Forces
{
    internal static class ForceActionService
    {
        public static void CopyActionProps(IForceAction source, IForceAction target)
        {
            target.Name = source.Name;
            target.SetInGravityCenter = source.SetInGravityCenter;
            target.ForcePoint.X = source.ForcePoint.X;
            target.ForcePoint.Y = source.ForcePoint.Y;
        }
    }
}
