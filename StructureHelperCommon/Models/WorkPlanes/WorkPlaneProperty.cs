using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperCommon.Models.WorkPlanes
{
    public class WorkPlaneProperty : IWorkPlaneProperty
    {
        public Guid Id { get; }
        public double GridSize { get; set; } = 0.05;
        public double Height { get; set; } = 1.2;
        public double Width { get; set; } = 1.2;
        public double AxisLineThickness { get; set; } = 2;
        public double GridLineThickness { get; set; } = 0.25;

        public WorkPlaneProperty(Guid id)
        {
            Id = id;
        }

        public IWorkPlaneProperty GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            WorkPlaneProperty newItem = new(id);
            WorkPlanePropertyUpdateStrategy updateStrategy = new();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
