using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.WorkPlanes
{
    public class WorkPlanePropertyUpdateStrategy : IUpdateStrategy<IWorkPlaneProperty>
    {
        public void Update(IWorkPlaneProperty targetObject, IWorkPlaneProperty sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.AxisLineThickness = sourceObject.AxisLineThickness;
            targetObject.GridSize = sourceObject.GridSize;
            targetObject.GridLineThickness = sourceObject.GridLineThickness;
            targetObject.Height = sourceObject.Height;
            targetObject.Width = sourceObject.Width;
        }
    }
}
