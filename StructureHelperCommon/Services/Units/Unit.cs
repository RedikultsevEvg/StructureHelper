using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Units
{
    public class Unit : IUnit
    {
        public UnitTypes UnitType { get; set; }
        public string Name { get; set; }
        public double Multiplyer { get; set; }
    }
}
