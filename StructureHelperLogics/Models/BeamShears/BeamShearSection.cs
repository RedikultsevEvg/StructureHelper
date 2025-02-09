using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSection : IBeamShearSection
    {
        public Guid Id { get; }
        public string? Name { get; set; }
        public IConcreteLibMaterial Material { get; set; }

        public IShape Shape { get; }

        public double CenterCover { get; set; }

        public BeamShearSection(Guid id, IShape shape)
        {
            Id = id;
            Shape = shape;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
