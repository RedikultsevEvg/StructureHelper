using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelper.UnitSystem.Systems;

namespace StructureHelper.UnitSystem
{
    public class UnitSystemService
    {
        private IUnitSystem currentSystem;
        public IUnitSystem GetCurrentSystem() => currentSystem;
        public UnitSystemService()
        {
            currentSystem = new SystemSi();
        }
    }
}
