using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Sections
{
    public class CompressedMemberUpdateStrategy : IUpdateStrategy<ICompressedMember>
    {
        public void Update(ICompressedMember targetObject, ICompressedMember sourceObject)
        {
            targetObject.Buckling = sourceObject.Buckling;
            targetObject.GeometryLength = sourceObject.GeometryLength;
            targetObject.LengthFactorX = sourceObject.LengthFactorX;
            targetObject.DiagramFactorX = sourceObject.DiagramFactorX;
            targetObject.LengthFactorY = sourceObject.LengthFactorY;
            targetObject.DiagramFactorY = sourceObject.DiagramFactorY;
        }
    }
}
