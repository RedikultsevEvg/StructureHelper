using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.WorkPlanes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class WorkPlanePropertyFromDTOConvertStrategy : ConvertStrategy<WorkPlaneProperty, WorkPlanePropertyDTO>
    {
        private IUpdateStrategy<IWorkPlaneProperty> updateStrategy;
        public WorkPlanePropertyFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary,
            IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {   }

        public override WorkPlaneProperty GetNewItem(WorkPlanePropertyDTO source)
        {
            InitializeStrategies();
            try
            {
                GetNewItemBySource(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewItemBySource(IWorkPlaneProperty source)
        {
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new WorkPlanePropertyUpdateStrategy();
        }
    }
}
