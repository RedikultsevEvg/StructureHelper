using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Units
{
    /// <summary>
    /// Interface for measurements Unit
    /// </summary>
    public interface IUnit
    {

        UnitTypes UnitType { get; }
        string Name { get; }
        double Multiplyer { get; }
    }
}
