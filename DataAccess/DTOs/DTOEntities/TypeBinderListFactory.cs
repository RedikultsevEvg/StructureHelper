using DataAccess.DTOs.DTOEntities;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                { (typeof(ConcreteLibMaterialDTO), "ConcreteLibMaterial") },
                { (typeof(CompressedMemberDTO), "CompressedMember") },
                { (typeof(CrackCalculatorDTO), "CrackCalculator") },
                { (typeof(CrackCalculatorInputDataDTO), "CrackCalculatorInputData") },
                { (typeof(CrossSectionDTO), "CrossSection") },
                { (typeof(CrossSectionNdmAnalysisDTO), "CrossSectionNdmAnalysis") },
                { (typeof(CrossSectionRepositoryDTO), "CrossSectionRepository") },
                { (typeof(DateVersionDTO), "DateVersion") },
                { (typeof(DesignForceTupleDTO), "DesignForceTuple") },
                { (typeof(DivisionSizeDTO), "DivisionSize") },
                { (typeof(ElasticMaterialDTO), "ElasticMaterial") },
                { (typeof(EllipseNdmPrimitiveDTO), "EllipseNdmPrimitive") },
                { (typeof(FileVersionDTO), "FileVersion") },
                { (typeof(ForceCalculatorDTO), "ForceCalculator") },
                { (typeof(ForceCalculatorInputDataDTO), "ForceCalculatorInputData") },
                { (typeof(ForceCombinationByFactorDTO), "ForceCombinationByFactor") },
                { (typeof(ForceCombinationListDTO), "ForceCombinationList") },
                { (typeof(ForceTupleDTO), "ForceTuple") },
                { (typeof(FRMaterialDTO), "FRMaterial") },
                { (typeof(HeadMaterialDTO), "HeadMaterial") },
                { (typeof(MaterialSafetyFactorDTO), "MaterialSafetyFactor") },
                { (typeof(NdmElementDTO), "NdmElement") },
                { (typeof(IVisualAnalysis), "IVisualAnalysis") },
                { (typeof(List<CalcTerms>), "ListOfCalcTerms") },
                { (typeof(List<ICalculator>), "ListOfICalculator") },
                { (typeof(List<IDateVersion>), "ListOfIDateVersion") },
                { (typeof(List<IDesignForceTuple>), "ListOfIDesignForceTuple") },
                { (typeof(List<IForceAction>), "ListOfIForceAction") },
                { (typeof(List<IHeadMaterial>), "ListOfIHeadMaterial") },
                { (typeof(List<LimitStates>), "ListOfLimitState") },
                { (typeof(List<IMaterialSafetyFactor>), "ListOfMaterialSafetyFactor") },
                { (typeof(List<IMaterialPartialFactor>), "ListOfMaterialPartialFactor") },
                { (typeof(List<INdmPrimitive>), "ListOfINdmPrimitive") },
                { (typeof(List<IPartialFactor>), "ListOfPartialFactor") },
                { (typeof(List<IVisualAnalysis>), "ListOfIVisualAnalysis") },
                { (typeof(Point2DDTO), "Point2D") },
                { (typeof(PointNdmPrimitiveDTO), "PointNdmPrimitive") },
                { (typeof(ProjectDTO), "Project") },
                { (typeof(RebarNdmPrimitiveDTO), "RebarNdmPrimitive") },
                { (typeof(RectangleNdmPrimitiveDTO), "RectangleNdmPrimitive") },
                { (typeof(RectangleShapeDTO), "RectangleShape") },
                { (typeof(ReinforcementLibMaterialDTO), "ReinforcementLibMaterial") },
                { (typeof(MaterialPartialFactorDTO), "MaterialPartialFactor") },
                { (typeof(VersionProcessorDTO), "VersionProcessor") },
                { (typeof(VisualAnalysisDTO), "VisualAnalysis") },
                { (typeof(VisualPropertyDTO), "VisualProperty") },
                { (typeof(UserCrackInputDataDTO), "UserCrackInputData") },
            };
            return newList;
        }
    }
}
