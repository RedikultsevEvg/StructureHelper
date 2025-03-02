using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials
{
    public class MaterialStrengthByLibMaterial : IMaterialStrengthByLibMaterial
    {
        public Guid Id { get; }
        public ILibMaterial LibMaterial { get; }

        public MaterialStrengthByLibMaterial(Guid id, ILibMaterial libMaterial)
        {
            Id = id;
            LibMaterial = libMaterial;
        }



        public (double Compressive, double Tensile) GetStrength(LimitStates limitState, CalcTerms calcTerm)
        {
            return LibMaterial.GetStrength(limitState, calcTerm);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
