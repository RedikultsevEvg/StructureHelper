using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Codes;
using StructureHelperCommon.Models.Codes.Factories;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Models.Projects;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public static class ProgramSetting
    {
        private static List<IMaterialLogic> materialLogics;
        private static List<ICodeEntity> codesList;
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
                codesList = CodeFactory
                    .GetCodeEntities()
                    .Where(x => x.NatSystem == natSystem)
                    .ToList();
                materialRepository = new MaterialRepository(codesList);
            }
        }
        public static GeometryNames GeometryNames => geometryNames ??= new GeometryNames();
        public static LimitStatesList LimitStatesList => new LimitStatesList();
        public static CalcTermList CalcTermList => new CalcTermList();
        public static List<ICodeEntity> CodesList
        { get
            {
                codesList ??= CodeFactory
                    .GetCodeEntities()
                    .Where(x => x.NatSystem == NatSystem)
                    .ToList();
                return codesList;
            }
        }

        public static IMaterialRepository MaterialRepository
        {
            get
            {
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
                SubVersionNumber = 0
            };
        }
    }
}
