using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <inheritdoc/>
    public class NdmElement : INdmElement
    {
        private IUpdateStrategy<INdmElement> updateStrategy;
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public IPoint2D Center { get; } = new Point2D();
        /// <inheritdoc/>
        public IHeadMaterial? HeadMaterial { get; set; }
        /// <inheritdoc/>
        public bool Triangulate { get; set; }
        /// <inheritdoc/>
        public StrainTuple UsersPrestrain { get; } = new();
        /// <inheritdoc/>
        public StrainTuple AutoPrestrain { get; } = new();

        public NdmElement(Guid id)
        {
            Id = id;
        }

        public NdmElement() : this(Guid.NewGuid())
        {
            
        }

        /// <inheritdoc/>
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
