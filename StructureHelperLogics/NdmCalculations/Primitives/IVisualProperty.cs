using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.VisualProperties;
using System.Windows.Media;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface IVisualProperty : IPrimitiveVisualProperty
    {
        /// <summary>
        /// Flag of assigning of color from material or from primitive's settings
        /// </summary>
        bool SetMaterialColor { get; set; }
    }
}
