using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Windows.Media;

namespace StructureHelperCommon.Models.WorkPlanes
{
    /// <summary>
    /// Implements properties of work plane
    /// </summary>
    public interface IWorkPlaneProperty : ISaveable, IRectangleShape
    {
        /// <summary>
        /// Size of grid of work plane
        /// </summary>
        double GridSize { get; set; }
        /// <summary>
        /// Thickness of axis line
        /// </summary>
        double AxisLineThickness { get; set; }
        /// <summary>
        /// Thickness of lines of main grid
        /// </summary>
        double GridLineThickness { get; set; }
    }
}