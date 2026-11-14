using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Materials
{
    public class HeadMaterialCloningStrategy : ICloneStrategy<HeadMaterial>
    {
        public HeadMaterial GetClone(HeadMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
