using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
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
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ConcreteLibMaterialDTO Convert(IConcreteLibMaterial source)
        {
            Check();

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
