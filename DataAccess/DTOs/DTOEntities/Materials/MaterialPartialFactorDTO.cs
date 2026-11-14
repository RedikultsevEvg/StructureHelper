using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class MaterialPartialFactorDTO : IMaterialPartialFactor
    {
        public Guid Id { get; set; }
        public double FactorValue { get; set; }
        public StressStates StressState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public LimitStates LimitState { get; set; }

        public MaterialPartialFactorDTO()
            : this(Guid.NewGuid())
        {
        }

        public MaterialPartialFactorDTO(Guid id)
        {
            Id = id;
        }

        public IPartialFactor GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            MaterialPartialFactorDTO newItem = new(id);
            new MaterialPartialFactorUpdateStrategy().Update(newItem, this);
            return newItem;
        }
    }
}
