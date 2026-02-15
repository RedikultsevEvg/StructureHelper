using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Codes;
using StructureHelperCommon.Models.Codes.Factories;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Models.Projects;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public static class ProgramSetting
    {
        private static List<IMaterialLogic> materialLogics;
        private static List<ICodeEntity> codesList;
        private static List<ICodeRevision> codesRevisions;
        private static IMaterialRepository materialRepository;
        private static NatSystems natSystem;
        private static GeometryNames geometryNames;

        public static CodeTypes CodeType => CodeTypes.SP63_2018;
        public static CodeTypes FRCodeType => CodeTypes.SP164_2014;
        public static NatSystems NatSystem
        {
            get => natSystem;
            set
            {
                natSystem = value;
                SetCodeList();
                materialRepository ??= new MaterialRepository(codesList);
            }
        }
        public static GeometryNames GeometryNames => geometryNames ??= new GeometryNames();
        public static LimitStatesList LimitStatesList => new LimitStatesList();
        public static CalcTermList CalcTermList => new CalcTermList();
        public static List<ICodeEntity> CodesList
        { get
            {
                SetCodeList();
                return codesList;
            }
        }

        private static void SetCodeList()
        {
            if (codesList is null)
            {
                var codes = CodeFactory.GetCodeEntities();
                codesList = codes.codes
                    .Where(x => x.NatSystem == NatSystem)
                    .ToList();
                codesRevisions = codes.codeRevisions
                    .Where(x => codesList.Contains(x.CodeEntity))
                    .ToList();
            }               
        }

        public static IMaterialRepository MaterialRepository
        {
            get
            {
                SetCodeList();
                materialRepository ??= new MaterialRepository(codesList);
                return materialRepository;
            }
        }
        public static List<IProject> Projects { get; } = new();
        public static IProject CurrentProject
        {
            get
            {
                if (Projects.Any()) { return Projects[0]; }
                return null;
            }
        }
        public static List<IMaterialLogic> MaterialLogics
        {
            get
            {
                materialLogics ??= MaterialLogicsFactory.GetMaterialLogics();
                return materialLogics;
            }
        }

        public static List<ICodeRevision> CodesRevisions { get => codesRevisions; set => codesRevisions = value; }

        public static void SetCurrentProjectToNotActual()
        {
            if (CurrentProject is null)
            {
                return;
            }
            CurrentProject.IsActual = false;
        }
        public static IFileVersion GetCurrentFileVersion()
        {
            return new FileVersion()
            {
                VersionNumber = 1,
                //SubVersionNumber = 2 //Add Beam shear analysis
                //SubVersionNumber = 3 //Add stirrup group and inclined rebar
                //SubVersionNumber = 4 //Add polygonshape primitive
                //SubVersionNumber = 5 //Add curvature calculator
                //SubVersionNumber = 6 //Add steel material
                SubVersionNumber = 7 //Add t-shape and o-shape
            };
        }
    }
}
