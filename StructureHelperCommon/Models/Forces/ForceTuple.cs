﻿using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services.Forces;
using System;
using System.ComponentModel.DataAnnotations;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ForceTuple : IForceTuple
    {
        private readonly IUpdateStrategy<IForceTuple> updateStrategy = new ForceTupleUpdateStrategy();
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

        /// <inheritdoc/>
        public object Clone()
        {
            var newItem = new ForceTuple();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
        public static ForceTuple operator +(ForceTuple first) => first;
        public static ForceTuple operator +(ForceTuple first, ForceTuple second)
        {
            return ForceTupleService.SumTuples(first, second) as ForceTuple;
        }
    }
}
