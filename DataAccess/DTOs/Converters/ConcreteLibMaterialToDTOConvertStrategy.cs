using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class ConcreteLibMaterialToDTOConvertStrategy : IConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial>
    {
        private IUpdateStrategy<IConcreteLibMaterial> updateStrategy = new ConcreteLibUpdateStrategy();
        private IUpdateStrategy<ILibMaterial> libMaterialUpdateStrategy = new LibMaterialDTOUpdateStrategy();
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ConcreteLibMaterialDTO Convert(IConcreteLibMaterial source)
        {
            Check();
            ConcreteLibMaterialDTO newItem = new()
            {
                Id = source.Id
            };
            try
            {
                updateStrategy.Update(newItem, source);
                libMaterialUpdateStrategy.Update(newItem, source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<ConcreteLibMaterialDTO, IConcreteLibMaterial>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }

    }
}
