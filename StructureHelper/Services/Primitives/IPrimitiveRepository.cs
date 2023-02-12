using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using Point = StructureHelper.Infrastructure.UI.DataContexts.PointViewPrimitive;
using Rectangle = StructureHelper.Infrastructure.UI.DataContexts.RectangleViewPrimitive;

namespace StructureHelper.Services.Primitives
{
    public interface IPrimitiveRepository
    {
        IEnumerable<PrimitiveBase> Primitives { get; }
        void Add(PrimitiveBase primitive);
        void Delete(PrimitiveBase primitive);
        void Clear();
    }
}
