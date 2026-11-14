using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.Forces;
using System;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class StrainTuple : IStrainTuple
    {
        private readonly IUpdateStrategy<IForceTuple> updateStrategy = new ForceTupleUpdateStrategy();
        private static IForceTupleServiceLogic forceTupleServiceLogic;
        private static IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public double Mx { get; set; }
        /// <inheritdoc/>
        public double My { get; set; }
        /// <inheritdoc/>
        public double Nz { get; set; }
        /// <inheritdoc/>
        public double Qx { get; set; }
        /// <inheritdoc/>
        public double Qy { get; set; }
        /// <inheritdoc/>
        public double Mz { get; set; }

        public StrainTuple(Guid id)
        {
            Id = id;
        }

        public StrainTuple() : this(Guid.NewGuid())
        {

        }

        public void Clear()
        {
            Mx = 0d;
            My = 0d;
            Nz = 0d;
            Qx = 0d;
            Qy = 0d;
            Mz = 0d;
        }

        public IForceTuple GetClone(IForceTuple sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference)
            {
                return sourceObject;
            }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            var newItem = new StrainTuple(id);
            updateStrategy.Update(newItem, this);
            return newItem;
        }

        public static StrainTuple operator +(StrainTuple first) => first;
        public static StrainTuple operator +(StrainTuple first, ForceTuple second)
        {
            return ForceTupleServiceLogic.SumTuples(first, second) as StrainTuple;
        }
    }
}
