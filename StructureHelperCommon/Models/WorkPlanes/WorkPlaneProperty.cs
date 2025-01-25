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
    }
}
