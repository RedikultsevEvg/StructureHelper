using StructureHelper.Models.Materials;
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
            return new List<(Type type, string name)>
            {
                { (typeof(CirclePrimitiveDTO), "CircleNdmPrimitive") },
                { (typeof(ConcreteLibMaterialDTO), "ConcreteLibMaterial") },
                { (typeof(CrossSectionDTO), "CrossSection") },
                { (typeof(CrossSectionNdmAnalysisDTO), "CrossSectionNdmAnalysis") },
                { (typeof(CrossSectionRepositoryDTO), "CrossSectionRepository") },
                { (typeof(DateVersionDTO), "DateVersion") },
                { (typeof(FileVersionDTO), "FileVersion") },
                { (typeof(HeadMaterialDTO), "HeadMaterial") },
                { (typeof(MaterialSafetyFactorDTO), "MaterialSafetyFactor") },
                { (typeof(NdmPrimitiveDTO), "NdmPrimitive") },
                { (typeof(IVisualAnalysis), "IVisualAnalysis") },
                { (typeof(List<ICalculator>), "ListOfICalculator") },
                { (typeof(List<IDateVersion>), "ListOfIDateVersion") },
                { (typeof(List<IForceAction>), "ListOfIForceAction") },
                { (typeof(List<IHeadMaterial>), "ListOfIHeadMaterial") },
                { (typeof(List<IMaterialSafetyFactor>), "ListOfMaterialSafetyFactor") },
                { (typeof(List<IMaterialPartialFactor>), "ListOfMaterialPartialFactor") },
                { (typeof(List<INdmPrimitive>), "ListOfINdmPrimitive") },
                { (typeof(List<IPartialFactor>), "ListOfPartialFactor") },
                { (typeof(List<IVisualAnalysis>), "ListOfIVisualAnalysis") },
                { (typeof(ProjectDTO), "Project") },
                { (typeof(ReinforcementLibMaterialDTO), "ReinforcementLibMaterial") },
                { (typeof(MaterialPartialFactorDTO), "MaterialPartialFactor") },
                { (typeof(VersionProcessorDTO), "VersionProcessor") },
                { (typeof(VisualAnalysisDTO), "VisualAnalysis") },
            };
        }
    }
}
