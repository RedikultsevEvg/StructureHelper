using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class HasForcesAndPrimitivesProcessLogic : IProcessLogic<IHasForcesAndPrimitives>
    {
        private ConvertDirection convertDirection;
        private IProcessLogic<IHasForceActions> forcesLogic;
        private IProcessLogic<IHasPrimitives> primitivesLogic;
        private IProcessLogic<IHasForceActions> ForcesLogic => forcesLogic ??= new HasForceActionsProcessLogic(convertDirection) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
        private IProcessLogic<IHasPrimitives> PrimitivesLogic => primitivesLogic ??=new HasPrimitivesProcessLogic(convertDirection) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IHasForcesAndPrimitives Source { get; set; }
        public IHasForcesAndPrimitives Target { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public HasForcesAndPrimitivesProcessLogic(ConvertDirection convertDirection)
        {
            this.convertDirection = convertDirection;
        }

        public HasForcesAndPrimitivesProcessLogic(
            ConvertDirection convertDirection,
            IProcessLogic<IHasForceActions> forcesLogic,
            IProcessLogic<IHasPrimitives> primitivesLogic)
        {
            this.convertDirection = convertDirection;
            this.forcesLogic = forcesLogic;
            this.primitivesLogic = primitivesLogic;
        }

        public void Process()
        {
            Check();
            ProcessForces();
            ProcessPrimitives();
        }

        private void Check()
        {
            CheckObject.ThrowIfNull(ReferenceDictionary, ": reference dictionary");
            CheckObject.ThrowIfNull(Source, ": source object");
            CheckObject.ThrowIfNull(Target, ": target object");
        }

        private void ProcessPrimitives()
        {
            PrimitivesLogic.Source = Source;
            PrimitivesLogic.Target = Target;
            PrimitivesLogic.Process();
        }

        private void ProcessForces()
        {
            ForcesLogic.Source = Source;
            ForcesLogic.Target = Target;
            ForcesLogic.Process();
        }
    }
}
