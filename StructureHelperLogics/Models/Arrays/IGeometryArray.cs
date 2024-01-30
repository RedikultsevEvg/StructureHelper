using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Arrays
{
    internal interface IGeometryArray : IGeometryArrayMember
    {
        List<IGeometryArrayMember> Children { get;}
        List<IGeometryArrayMember> GetAllChildren();
        List<IGeometryArrayMember> Explode();
    }
}
