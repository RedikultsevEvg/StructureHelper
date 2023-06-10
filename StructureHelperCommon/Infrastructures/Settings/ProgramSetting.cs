using StructureHelperCommon.Infrastructures.Enums;
using System.Windows.Navigation;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public static class ProgramSetting
    {
        public static CodeTypes CodeType => CodeTypes.SP63_2018;
        public static CodeTypes FRCodeType => CodeTypes.SP164_2014;
        public static CrossSectionAxisNames CrossSectionAxisNames => new CrossSectionAxisNames();
        public static LimitStatesList LimitStatesList => new LimitStatesList();
        public static CalcTermList CalcTermList => new CalcTermList();
    }
}
