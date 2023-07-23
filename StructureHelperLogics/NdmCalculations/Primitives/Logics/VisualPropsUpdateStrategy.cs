using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class VisualPropsUpdateStrategy : IUpdateStrategy<IVisualProperty>
    {
        public void Update(IVisualProperty targetObject, IVisualProperty sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.IsVisible = sourceObject.IsVisible;
            targetObject.Color = sourceObject.Color;
            targetObject.SetMaterialColor = sourceObject.SetMaterialColor;
            targetObject.Opacity = sourceObject.Opacity;
            targetObject.ZIndex = sourceObject.ZIndex;
        }
    }
}
