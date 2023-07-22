using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Codes;
using StructureHelperCommon.Models.Codes.Factories;
using StructureHelperCommon.Models.Materials.Libraries;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public static class ProgramSetting
    {
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
                codesList = CodeFactory.GetCodeEntities()
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
                codesList ??= CodeFactory.GetCodeEntities()
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

    }
}
