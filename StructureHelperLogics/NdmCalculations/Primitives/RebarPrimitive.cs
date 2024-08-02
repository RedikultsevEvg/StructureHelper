using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <inheritdoc/>
    public class RebarPrimitive : IRebarPrimitive
    {
        static readonly RebarUpdateStrategy updateStrategy = new();

        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public IPoint2D Center { get; private set; }
        /// <inheritdoc/>
        public IHeadMaterial? HeadMaterial { get; set; }
        /// <inheritdoc/>
        public bool Triangulate { get; set; }
        /// <inheritdoc/>
        public StrainTuple UsersPrestrain { get; private set; }
        /// <inheritdoc/>
        public StrainTuple AutoPrestrain { get; private set; }
        /// <inheritdoc/>
        public IVisualProperty VisualProperty { get; private set; }
        /// <inheritdoc/>
        public Guid Id { get; set; }
        /// <inheritdoc/>
        public double Area { get; set; }
        /// <inheritdoc/>
        public INdmPrimitive HostPrimitive { get; set; }
        /// <inheritdoc/>
        public ICrossSection? CrossSection { get; set; }


        public RebarPrimitive(Guid id)
        {
            Id = id;
            Name = "New Reinforcement";
            Area = 0.0005d;
            Center = new Point2D();
            VisualProperty = new VisualProperty();
            UsersPrestrain = new StrainTuple();
            AutoPrestrain = new StrainTuple();
            Triangulate = true;
        }
        public RebarPrimitive() : this(Guid.NewGuid())
        {

        }

        public object Clone()
        {
            var primitive = new RebarPrimitive();
            updateStrategy.Update(primitive, this);
            return primitive;
        }

        public IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions)
        {
            List<INdm> ndms = new()
            {
                GetConcreteNdm(triangulationOptions),
                GetRebarNdm(triangulationOptions)
            };
            return ndms;
        }

        public RebarNdm GetRebarNdm(ITriangulationOptions triangulationOptions)
        {
            var options = new RebarTriangulationLogicOptions(this)
            {
                triangulationOptions = triangulationOptions
            };
            var logic = new RebarTriangulationLogic(options);
            var rebar = logic.GetRebarNdm();
            return rebar;
        }

        public Ndm GetConcreteNdm(ITriangulationOptions triangulationOptions)
        {
            var options = new RebarTriangulationLogicOptions(this)
            {
                triangulationOptions = triangulationOptions
            };
            var logic = new RebarTriangulationLogic(options);
            var concrete = logic.GetConcreteNdm();
            return concrete;
        }

        public List<INamedAreaPoint> GetValuePoints()
        {
            var points = new List<INamedAreaPoint>();
            var newPoint = new NamedAreaPoint
            {
                Name = "Center",
                Point = Center.Clone() as Point2D,
                Area = Area
            };
            points.Add(newPoint);
            return points;
        }
    }
}
