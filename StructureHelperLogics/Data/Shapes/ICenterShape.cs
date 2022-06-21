using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Data.Shapes
{
    public interface ICenterShape
    {
        ICenter Center {get;}
        IShape Shape { get;}
    }
}
