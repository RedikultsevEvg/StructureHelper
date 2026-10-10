using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IGetShapeFactorLogic : ILogic
    {
        double GetShapeFactor();
    }
}
