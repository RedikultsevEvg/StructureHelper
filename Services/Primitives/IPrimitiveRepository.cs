using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Models.NdmPrimitives;
using StructureHelperCommon.Models.Shapes;
using Point = StructureHelper.Infrastructure.UI.DataContexts.Point;
using Rectangle = StructureHelper.Infrastructure.UI.DataContexts.Rectangle;

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
