using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
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
    public class ReinforcementPrimitive : IPointPrimitive, IHasHostPrimitive
    {
        IDataRepository<ReinforcementPrimitive> repository;
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public double CenterX { get; set; }
        /// <inheritdoc/>
        public double CenterY { get; set; }
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

        public ReinforcementPrimitive(Guid id)
        {
            Id = id;
            Name = "New Reinforcement";
            Area = 0.0005d;
            VisualProperty = new VisualProperty();
            UsersPrestrain = new StrainTuple();
            AutoPrestrain = new StrainTuple();
            Triangulate = true;
        }
        public ReinforcementPrimitive() : this(Guid.NewGuid())
        {
                
        }

        public object Clone()
        {
            var primitive = new ReinforcementPrimitive();
            NdmPrimitivesService.CopyNdmProperties(this, primitive);
            primitive.Area = Area;
            primitive.HostPrimitive = HostPrimitive;
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
            repository.Save(this);
        }
    }
}
