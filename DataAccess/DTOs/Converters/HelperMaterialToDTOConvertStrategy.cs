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
    internal class HelperMaterialToDTOConvertStrategy : IConvertStrategy<IHelperMaterial, IHelperMaterial>
    {
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public IHelperMaterial Convert(IHelperMaterial source)
        {
            Check();
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<IHelperMaterial, IHelperMaterial>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
