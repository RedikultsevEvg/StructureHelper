using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAccess.DTOs
{
    public class VersionItemToDTOConvertStrategy : IConvertStrategy<ISaveable, ISaveable>
    {
        private const string Message = "Analysis type is";
        private IConvertStrategy<CrossSectionDTO, ICrossSection> crossSectionConvertStrategy = new CrossSectionToDTOConvertStrategy();
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }


        public ISaveable Convert(ISaveable source)
        {
            Check();
            ISaveable saveable;
            if (source is ICrossSection crossSection)
            {
                saveable = ProcessCrossSection(crossSection);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            return saveable;
        }

        private ISaveable ProcessCrossSection(ICrossSection crossSection)
        {
            TraceLogger?.AddMessage(Message + " Cross-Section Ndm Analysis", TraceLogStatuses.Debug);
            ISaveable saveable;
            crossSectionConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            crossSectionConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<CrossSectionDTO, ICrossSection>()
            {
                ReferenceDictionary = ReferenceDictionary,
                ConvertStrategy = crossSectionConvertStrategy,
                TraceLogger = TraceLogger
            };
            saveable = convertLogic.Convert(crossSection);
            return saveable;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<ISaveable, ISaveable>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
