using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
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
    public class RebarPrimitive : IPointPrimitive, IHasHostPrimitive
    {
        static readonly RebarUpdateStrategy updateStrategy = new();
        IDataRepository<RebarPrimitive> repository;
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public IPoint2D Center { get; private set; }
        /// <inheritdoc/>
        public IHeadMaterial? HeadMaterial { get; set; }
        public bool Triangulate { get; set; }

        public StrainTuple UsersPrestrain { get; private set; }

        public StrainTuple AutoPrestrain { get; private set; }

        public IVisualProperty VisualProperty { get; private set; }

        public Guid Id { get; set; }
        public double Area { get; set; }
        public INdmPrimitive HostPrimitive { get; set; }
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

        public IEnumerable<INdm> GetNdms(IMaterial material)
        {
            var options = new PointTriangulationLogicOptions(this);
            IPointTriangulationLogic logic = new PointTriangulationLogic(options);
            return logic.GetNdmCollection(material);
        }

        public void Save()
        {
            repository.Create(this);
        }
    }
}
