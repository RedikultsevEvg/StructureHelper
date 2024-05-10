using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    internal class MeshCrackedConcreteLogic : IMeshPrimitiveLogic
    {
        private IMeshPrimitiveLogic regularMeshLogic;
        public List<INdm> NdmCollection { get; set; }
        public INdmPrimitive Primitive { get; set; }
        public ITriangulationOptions TriangulationOptions { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        List<INdm> IMeshPrimitiveLogic.MeshPrimitive()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            CheckPrimitive();
            List<INdm> ndmCollection = new();
            if (Primitive.HeadMaterial.HelperMaterial is ICrackedMaterial)
            {
                TraceLogger?.AddMessage($"Primitive {Primitive.Name} is crackable primitive", TraceLogStatuses.Service);
                var newPrimititve = Primitive.Clone() as INdmPrimitive;
                SetNewMaterial(newPrimititve);
                List<INdm> ndms = GetNdms(newPrimititve);
                ndmCollection.AddRange(ndms);
            }
            else if (Primitive is RebarPrimitive rebar)
            {
                TraceLogger?.AddMessage($"Primitive {Primitive.Name} is rebar primitive", TraceLogStatuses.Service);
                var newPrimititve = rebar.Clone() as RebarPrimitive;
                var newHostPrimitive = rebar.HostPrimitive.Clone() as INdmPrimitive;
                SetNewMaterial(newHostPrimitive);
                newPrimititve.HostPrimitive = newHostPrimitive;
                List<INdm> ndms = GetNdms(newPrimititve);
                ndmCollection.AddRange(ndms);
            }
            else
            {
                TraceLogger?.AddMessage($"Primitive {Primitive.Name} is non-crackable primitive", TraceLogStatuses.Service);
                List<INdm> ndms = GetNdms(Primitive);
                ndmCollection.AddRange(ndms);
            }
            return ndmCollection;
        }

        private void SetNewMaterial(INdmPrimitive? newPrimititve)
        {
            TraceLogger?.AddMessage($"Process material {newPrimititve.HeadMaterial.Name} has started");
            var newMaterial = newPrimititve.HeadMaterial.HelperMaterial.Clone() as ICrackedMaterial;
            TraceLogger?.AddMessage($"Set work in tension zone for material {newPrimititve.HeadMaterial.Name}");
            newMaterial.TensionForSLS = false;
            newPrimititve.HeadMaterial.HelperMaterial = newMaterial as IHelperMaterial;
        }

        private List<INdm> GetNdms(INdmPrimitive primitive)
        {
            TraceLogger?.AddMessage($"Triangulation primitive has started");
            regularMeshLogic = new MeshPrimitiveLogic()
            {
                Primitive = primitive,
                TriangulationOptions = TriangulationOptions,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            List<INdm> ndms = regularMeshLogic.MeshPrimitive();
            return ndms;
        }

        private void CheckPrimitive()
        {
            if (TriangulationOptions.LimiteState is not LimitStates.SLS)
            {
                string errorMessage = string.Intern(ErrorStrings.DataIsInCorrect + $": Limit state for cracking must correspondent limit state of serviceability");
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
            if (TriangulationOptions.CalcTerm is not CalcTerms.ShortTerm)
            {
                string errorMessage = string.Intern(ErrorStrings.DataIsInCorrect + $": Calc term for cracked concrete must correspondent short term");
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
            TraceLogger?.AddMessage($"Primitive check is ok");
        }
    }
}
