using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials.Logics;
using StructureHelperCommon.Models.States;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Materials
{
    public class CustomConcreteModel : ICustomConcreteModel
    {
        public Guid Id { get; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; } = string.Empty;
        public double YoungModulus { get; set; } = 3e10;

        public IStateCalcTermPair StateCalcTermPair { get; set; } = new StateCalcTermPair(Guid.NewGuid());

        public ICustomConcreteCompressionOptions CompressionOptions { get; private set; } = new CustomConcreteCompressionOptions(Guid.NewGuid());

        public ICustomConcreteTensionOptions TensionOptions { get; private set; } = new CustomConcreteTensionOptions(Guid.NewGuid());

        public CustomConcreteModel(Guid id)
        {
            Id = id;
        }

        public ICustomConcreteModel GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            CustomConcreteModel clone = new(id);
            CustomConcreteModelUpdateStrategy updateStrategy = new() { UpdateChildren = false };
            updateStrategy.Update(clone, this);
            if (cloneDepth == CloneDepth.Shallow || cloneDepth == CloneDepth.Deep)
            {
                clone.StateCalcTermPair = StateCalcTermPair.GetClone(cloneMode);
                clone.CompressionOptions = CompressionOptions.GetClone(cloneMode, cloneDepth);
                clone.TensionOptions = TensionOptions.GetClone(cloneMode, cloneDepth);
            }
            else if (cloneDepth == CloneDepth.DeleteChildren)
            {
                //Skip
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cloneDepth));
            }

            return clone;
        }
    }
}
