using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class PredicateFactory
    {
        
        private ForceTupleCalculator calculator;
        private ForceTuple tuple;
        private ForceTupleInputData inputData;
        public IEnumerable<INdm> Ndms { get; set; }
        public IConvert2DPointTo3DPointLogic ConvertLogic { get; set; }
        public PredicateFactory()
        {
            inputData = new();
            calculator = new() { InputData = inputData };                  
        }
        public bool IsSectionFailure(IPoint2D point2D)
        {
            var point3D = ConvertLogic.GetPoint3D(point2D);
            tuple = new()
            {
                Nz = point3D.Z,
                Mx = point3D.X,
                My = point3D.Y
            };
            inputData.Tuple = tuple;
            inputData.NdmCollection = Ndms;
            calculator.Run();
            var result = calculator.Result;
            return !result.IsValid;
        }

        public bool IsSectionCracked(IPoint2D point2D)
        {
            var logic = new HoleSectionCrackedLogic();
            var point3D = ConvertLogic.GetPoint3D(point2D);
            tuple = new()
            {
                Nz = point3D.Z,
                Mx = point3D.X,
                My = point2D.Y
            };
            logic.Tuple = tuple;
            logic.NdmCollection = Ndms;
            try
            {
                var result = logic.IsSectionCracked();
                return result;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
