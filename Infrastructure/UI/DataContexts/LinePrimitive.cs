using StructureHelper.Infrastructure.Enums;
using StructureHelper.Services.Primitives;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    internal class LinePrimitive : PrimitiveBase
    {
        private ILineShape lineShape;
        public LinePrimitive(PrimitiveType type, double x, double y, MainViewModel ownerVM) : base(type, x, y, ownerVM)
        {
        }

        public override INdmPrimitive GetNdmPrimitive(IUnitSystem unitSystem)
        {
            throw new NotImplementedException();
        }
    }
}
