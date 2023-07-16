using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public static class NdmPrimitivesService
    {
        public static List<INdm> GetNdms(INdmPrimitive primitive, LimitStates limitState, CalcTerms calcTerm)
        {
            //Формируем коллекцию элементарных участков для расчета в библитеке (т.е. выполняем триангуляцию)
            List<INdm> ndmCollection = new List<INdm>();
            var material = primitive.HeadMaterial.GetLoaderMaterial(limitState, calcTerm);
            ndmCollection.AddRange(primitive.GetNdms(material));
            return ndmCollection;
        }
        public static List<INdm> GetNdms(IEnumerable<INdmPrimitive> primitives, LimitStates limitState, CalcTerms calcTerm)
        {
            var orderedNdmPrimitives = primitives.OrderBy(x => x.VisualProperty.ZIndex);
            var ndms = new List<INdm>();
            foreach (var item in orderedNdmPrimitives)
            {
                if (item is IHasDivisionSize)
                {
                    var hasDivision = item as IHasDivisionSize;
                    if (hasDivision.ClearUnderlying == true)
                    {
                        ndms.RemoveAll(x => hasDivision.IsPointInside(new Point2D() { X = x.CenterX, Y = x.CenterY }) == true);
                    }
                }
                if (item.Triangulate == true)
                {
                    ndms.AddRange(GetNdms(item, limitState, calcTerm));
                }
            }
            return ndms;
        }

        public static bool CheckPrimitives(IEnumerable<INdmPrimitive> primitives)
        {
            if (primitives.Count() == 0) { throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Count of primitive must be greater than zero"); }
            if (primitives.Count(x => x.Triangulate == true) == 0) { throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": There are not primitives to triangulate"); }
            foreach (var item in primitives)
            {
                if (item.Triangulate == true &
                    item.HeadMaterial is null)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Primitive: {item.Name} can't be triangulated since material is null");
                }
            } 
            return true;
        }
    }
}
