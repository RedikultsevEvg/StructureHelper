using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using LoaderCalculator.Logics.Geometry;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    internal class SofteningFactorLogic : ILogic
    {
        public static IStressLogic stressLogic = new StressLogic();
        public IEnumerable<INdm> NdmCollection { get; set; }
        public StrainTuple StrainTuple { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public StrainTuple GetSofteningFactors()
        {
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Logic of analisys based on calculating sum of secant stifness of elementary parts EA,i = A,i * Esec,i");
            TraceLogger?.AddMessage($"Calculating geometry properies for strains");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(StrainTuple));
            var loaderStainMatrix = TupleConverter.ConvertToLoaderStrainMatrix(StrainTuple);
            var (MxFactor, MyFactor, NzFactor) = GeometryOperations.GetSofteningsFactors(NdmCollection, loaderStainMatrix);
            TraceLogger?.AddMessage($"Reducing of stiffness in {ProgramSetting.GeometryNames.SndAxisName}-plane: EIx_cracked / EIx_uncracked = {MxFactor}");
            TraceLogger?.AddMessage($"Reducing of stiffness in {ProgramSetting.GeometryNames.FstAxisName}-plane: EIy_cracked / EIy_uncracked = {MyFactor}");
            TraceLogger?.AddMessage($"Reducing of along {ProgramSetting.GeometryNames.TrdAxisName}-axis: EA_cracked / EA_uncracked = {NzFactor}");
            var strainTuple = new StrainTuple
            {
                Mx = MxFactor,
                My = MyFactor,
                Nz = NzFactor
            };
            return strainTuple;
        }
    }
}
