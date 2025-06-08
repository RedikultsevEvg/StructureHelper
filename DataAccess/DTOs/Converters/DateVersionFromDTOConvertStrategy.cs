using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class DateVersionFromDTOConvertStrategy : ConvertStrategy<IDateVersion, IDateVersion>
    {
        private IUpdateStrategy<IDateVersion> updateStrategy;
        private IConvertStrategy<ISaveable, ISaveable> convertStrategy;
        private IConvertStrategy<ISaveable, ISaveable> convertLogic;

        public override IDateVersion GetNewItem(IDateVersion source)
        {
            ChildClass = this;
            return GetDateVersion(source);
        }

        private DateVersion GetDateVersion(IDateVersion source)
        {
            TraceLogger?.AddMessage("Date version converting is started", TraceLogStatuses.Service);
            InitializeStrategies();
            DateVersion newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            newItem.AnalysisVersion = convertLogic.Convert(source.AnalysisVersion);
            TraceLogger?.AddMessage($"Date version date = {newItem.DateTime} converting has been finished", TraceLogStatuses.Service);
            return newItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new DateVersionUpdateStrategy();
            convertLogic = new DictionaryConvertStrategy<ISaveable, ISaveable>
                (this, new VersionItemFromDTOConvertStrategy(this));
        }
    }
}
