using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;

namespace DataAccess.DTOs
{
    public class AccuracyFromDTOConvertStrategy : ConvertStrategy<Accuracy, AccuracyDTO>
    {
        private IUpdateStrategy<IAccuracy> updateStrategy;
        public override Accuracy GetNewItem(AccuracyDTO source)
        {
            updateStrategy ??= new AccuracyUpdateStrategy();
            try
            {
                NewItem = new(source.Id);
                updateStrategy.Update(NewItem, source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }
    }
}
