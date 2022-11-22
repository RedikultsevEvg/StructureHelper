using StructureHelper.Infrastructure.Enums;
using StructureHelper.Services.Primitives;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    internal class LineViewPrimitive : PrimitiveBase
    {
        public LineViewPrimitive(ILinePrimitive primitive) : base(primitive)
        {

        }

        //public LineViewPrimitive(double x, double y, MainViewModel ownerVM) : base(x, y, ownerVM)
        //{
        //}
    }
}
