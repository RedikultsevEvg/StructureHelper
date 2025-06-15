using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.WorkPlanes;

namespace DataAccess.DTOs
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
