using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.Forces;
using System;
using System.ComponentModel.DataAnnotations;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ForceTuple : IForceTuple
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

        public ForceTuple(Guid id)
        {
            Id = id;
        }

        public ForceTuple() : this (Guid.NewGuid())
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
            if (cloneMode == CloneMode.Reference) { return sourceObject;}
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            var newItem = new ForceTuple(id);
            updateStrategy.Update(newItem, sourceObject);
            return newItem;
        }

        public static ForceTuple operator +(ForceTuple first) => first;
        public static ForceTuple operator +(ForceTuple first, ForceTuple second)
        {
            return ForceTupleServiceLogic.SumTuples(first, second) as ForceTuple;
        }
    }
}
