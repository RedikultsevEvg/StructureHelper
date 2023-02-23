using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public static class NdmPrimitivesService
    {
        public static void CopyVisualProperty(IVisualProperty source, IVisualProperty target)
        {
            target.IsVisible = source.IsVisible;
            target.Color = source.Color;
            target.SetMaterialColor = source.SetMaterialColor;
            target.Opacity = source.Opacity;
            target.ZIndex = source.ZIndex;
        }

        public static void CopyNdmProperties (INdmPrimitive source, INdmPrimitive target)
        {
            target.Name = source.Name + " copy" ;
            if (source.HeadMaterial != null) target.HeadMaterial = source.HeadMaterial;
            CopyVisualProperty(source.VisualProperty, target.VisualProperty);
            target.CenterX = source.CenterX;
            target.CenterY = source.CenterY;
            StrainTupleService.CopyProperties(source.UsersPrestrain, target.UsersPrestrain);
        }

        public static void CopyDivisionProperties(IHasDivisionSize source, IHasDivisionSize target)
        {
            CopyNdmProperties(source, target);
            target.NdmMaxSize = source.NdmMaxSize;
            target.NdmMinDivision = source.NdmMinDivision;
        }

        public static List<INdm> GetNdms(IEnumerable<INdmPrimitive> primitives, LimitStates limitState, CalcTerms calcTerm)
        {
            //Настройки триангуляции
            ITriangulationOptions options = new TriangulationOptions { LimiteState = limitState, CalcTerm = calcTerm };

            //Формируем коллекцию элементарных участков для расчета в библитеке (т.е. выполняем триангуляцию)
            List<INdm> ndmCollection = new List<INdm>();
            ndmCollection.AddRange(Triangulation.GetNdms(primitives, options));

            return ndmCollection;
        }
    }
}
