using StructureHelperCommon.Models.Materials.Logics;
using StructureHelperCommon.Models.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public class CustomConcreteModel : ICustomConcreteModel
    {
        public Guid Id { get; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; } = string.Empty;
        public double YoungModulus { get; set; } = 3e10;

        public IStateCalcTermPair StateCalcTermPair { get; } = new StateCalcTermPair();

        public ICustomConcreteCompressionOptions CompressionOptions { get; } = new CustomConcreteCompressionOptions(Guid.NewGuid());

        public ICustomConcreteTensionOptions TensionOptions { get; } = new CustomConcreteTensionOptions(Guid.NewGuid());

        public CustomConcreteModel(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            var clone = new CustomConcreteModel(Guid.NewGuid());
            var updateLogic = new CustomConcreteModelUpdateStrategy() { UpdateChildren = true };
            updateLogic.Update(clone, this);
            return clone;
        }
    }
}
