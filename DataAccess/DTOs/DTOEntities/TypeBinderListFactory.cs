using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    internal enum TypeFileVersion
    {
        version_v1
    }
    internal static class TypeBinderListFactory
    {
        public static List<(Type type, string name)> GetTypeNameList(TypeFileVersion fileVersion)
        {
            if (fileVersion == TypeFileVersion.version_v1)
            {
                List<(Type type, string name)> typesNames = GetVersionV1();
                return typesNames;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(fileVersion));
            }
        }

        private static List<(Type type, string name)> GetVersionV1()
        {
            List<(Type type, string name)> newList = new List<(Type type, string name)>
            {
                { (typeof(AccuracyDTO), "Accuracy") },
                { (typeof(ColumnFilePropertyDTO), "ColumnFileProperty") },
                { (typeof(ColumnedFilePropertyDTO), "ColumnedFileProperty") },
                { (typeof(CompressedMemberDTO), "CompressedMember") },
                { (typeof(DivisionSizeDTO), "DivisionSize") },
                { (typeof(List<CalcTerms>), "ListOfCalcTerms") },
                { (typeof(List<IColumnFileProperty>), "ColumnFileProperties") },
                { (typeof(List<IColumnedFileProperty>), "ColumnedFileProperties") },
                { (typeof(List<LimitStates>), "ListOfLimitState") },
                { (typeof(List<IPartialFactor>), "ListOfPartialFactor") },
                { (typeof(RebarSectionDTO), "RebarSection") },
                { (typeof(StateCalcTermPairDTO), "StateCalcTermPair") },
                { (typeof(VisualPropertyDTO), "VisualProperty") },
                { (typeof(WorkPlanePropertyDTO), "WorkPlanePropertyDTO") },
            };
            newList.AddRange(GetProjectList());
            newList.AddRange(GetGeometryShapeList());
            newList.AddRange(GetForceList());
            newList.AddRange(GetMaterialList());
            newList.AddRange(GetCalculatorList());
            newList.AddRange(GetNdmPrimitiveList());
            newList.AddRange(GetBeamShearList());
            newList.AddRange(GetValueDiagramList());
            return newList;
        }

        private static IEnumerable<(Type type, string name)> GetValueDiagramList()
        {
            List<(Type type, string name)> newList = new()
            {
                { (typeof(List<IValueDiagramEntity>), "ListOfValueDiagramEntity") },
                { (typeof(ValueDiagramCalculatorDTO), "ValueDiagramCalculator") },
                { (typeof(ValueDiagramCalculatorInputDataDTO), "ValueDiagramCalculatorInputData") },
                { (typeof(ValueDiagramEntityDTO), "ValueDiagramEntity") },
                { (typeof(ValueDiagramDTO), "ValueDiagram") },
            };
            return newList;
        }

        private static List<(Type type, string name)> GetProjectList()
        {
            List<(Type type, string name)> newList = new()
            {
                { (typeof(List<IVisualAnalysis>), "ListOfIVisualAnalysis") },
                { (typeof(List<IDateVersion>), "ListOfIDateVersion") },
                { (typeof(RootObjectDTO), "RootObject") },
                { (typeof(ProjectDTO), "Project") },
                { (typeof(VersionProcessorDTO), "VersionProcessor") },
                { (typeof(DateVersionDTO), "DateVersion") },
                { (typeof(IVisualAnalysis), "IVisualAnalysis") },
                { (typeof(VisualAnalysisDTO), "VisualAnalysis") },
                { (typeof(FileVersionDTO), "FileVersion") },

            };
            return newList;
        }
        private static List<(Type type, string name)> GetMaterialList()
        {
            List<(Type type, string name)> newList = new()
            {
                { (typeof(List<IMaterialPartialFactor>), "ListOfMaterialPartialFactor") },
                { (typeof(ConcreteLibMaterialDTO), "ConcreteLibMaterial") },
                { (typeof(ElasticMaterialDTO), "ElasticMaterial") },
                { (typeof(FRMaterialDTO), "FRMaterial") },
                { (typeof(HeadMaterialDTO), "HeadMaterial") },
                { (typeof(List<IHeadMaterial>), "ListOfIHeadMaterial") },
                { (typeof(MaterialSafetyFactorDTO), "MaterialSafetyFactor") },
                { (typeof(List<IMaterialSafetyFactor>), "ListOfMaterialSafetyFactor") },
                { (typeof(MaterialPartialFactorDTO), "MaterialPartialFactor") },
                { (typeof(ReinforcementLibMaterialDTO), "ReinforcementLibMaterial") },
            };
            return newList;
        }
        private static List<(Type type, string name)> GetCalculatorList()
        {
            List<(Type type, string name)> newList = new()
            {
                { (typeof(List<ICalculator>), "ListOfICalculator") },
                { (typeof(ForceCalculatorDTO), "ForceCalculator") },
                { (typeof(ForceCalculatorInputDataDTO), "ForceCalculatorInputData") },
                { (typeof(CrackCalculatorDTO), "CrackCalculator") },
                { (typeof(CrackCalculatorInputDataDTO), "CrackCalculatorInputData") },
                { (typeof(UserCrackInputDataDTO), "UserCrackInputData") },

            };
            return newList;
        }
        private static List<(Type type, string name)> GetNdmPrimitiveList()
        {
            List<(Type type, string name)> newList = new()
            {
                { (typeof(List<INdmPrimitive>), "ListOfINdmPrimitive") },
                { (typeof(CrossSectionNdmAnalysisDTO), "CrossSectionNdmAnalysis") },
                { (typeof(CrossSectionDTO), "CrossSection") },
                { (typeof(CrossSectionRepositoryDTO), "CrossSectionRepository") },
                { (typeof(NdmElementDTO), "NdmElement") },
                { (typeof(PointNdmPrimitiveDTO), "PointNdmPrimitive") },
                { (typeof(RebarNdmPrimitiveDTO), "RebarNdmPrimitive") },
                { (typeof(RectangleNdmPrimitiveDTO), "RectangleNdmPrimitive") },
                { (typeof(EllipseNdmPrimitiveDTO), "EllipseNdmPrimitive") },
                { (typeof(ShapeNdmPrimitiveDTO), "ShapeNdmPrimitive") },
                { (typeof(PrimitiveVisualPropertyDTO), "PrimitiveVisualProperty") },

            };
            return newList;
        }
        private static List<(Type type, string name)> GetGeometryShapeList()
        {
            List<(Type type, string name)> newList = new()
            {
                { (typeof(List<IVertex>), "ListOfVertex2D") },
                { (typeof(Point2DDTO), "Point2D") },
                { (typeof(Point2DRangeDTO), "Point2DRange") },
                { (typeof(VertexDTO), "Vertex2D") },
                { (typeof(RectangleShapeDTO), "RectangleShape") },
                { (typeof(CircleShapeDTO), "CircleShape") },
                { (typeof(LinePolygonShapeDTO), "LinePolygonShape") },
            };
            return newList;
        }
        private static List<(Type type, string name)> GetForceList()
        {
            List<(Type type, string name)> newList = new()
            {
                { (typeof(List<IForceAction>), "ListOfIForceAction") },
                { (typeof(List<IDesignForceTuple>), "ListOfIDesignForceTuple") },
                { (typeof(List<IForceTuple>), "ListOfIForceTuple") },
                { (typeof(DesignForceTupleDTO), "DesignForceTuple") },
                { (typeof(ConcentratedForceDTO), "ConcentratedForce") },
                { (typeof(DistributedLoadDTO), "DistributedLoad") },
                { (typeof(FactoredForceTupleDTO), "FactoredForceTuple") },
                { (typeof(ForceCombinationByFactorV1_0DTO), "ForceCombinationByFactor") },
                { (typeof(ForceFactoredListDTO), "ForceCombinationByFactor_v1_1") },
                { (typeof(ForceCombinationFromFileDTO), "ForceCombinationFromFile") },
                { (typeof(ForceCombinationListDTO), "ForceCombinationList") },
                { (typeof(FactoredCombinationPropertyDTO), "ForceFactoredCombinationProperty") },
                { (typeof(ForceTupleDTO), "ForceTuple") },
            };
            return newList;
        }
        private static List<(Type type, string name)> GetBeamShearList()
        {
            List<(Type type, string name)> newList = new()
            {
                { (typeof(BeamShearDTO), "BeamShear") },
                { (typeof(BeamShearActionDTO), "BeamShearAction") },
                { (typeof(BeamShearAxisActionDTO), "BeamShearAxisAction") },
                { (typeof(BeamShearAnalysisDTO), "BeamShearAnalysis") },
                { (typeof(BeamShearCalculatorDTO), "BeamShearCalculator") },
                { (typeof(BeamShearCalculatorInputDataDTO), "BeamShearCalculatorInputData") },
                { (typeof(BeamShearDesignRangePropertyDTO), "BeamShearDesignRangeProperty") },
                { (typeof(BeamShearRepositoryDTO), "BeamShearRepository") },
                { (typeof(BeamShearSectionDTO), "BeamShearSection") },
                { (typeof(List<IBeamShearAction>), "ListOfBeamShearActions") },
                { (typeof(List<IBeamShearSection>), "ListOfBeamShearSections") },
                { (typeof(List<IBeamSpanLoad>), "ListOfSpanLoads") },
                { (typeof(List<IStirrup>), "ListOfStirrups") },
                { (typeof(StirrupByDensityDTO), "StirrupByDensity") },
                { (typeof(StirrupGroupDTO), "StirrupGroup") },
                { (typeof(StirrupByInclinedRebarDTO), "StirrupByInclinedRebar") },
                { (typeof(StirrupByRebarDTO), "StirrupByRebar") },
            };
            return newList;
        }
    }
}
