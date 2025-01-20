using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;

namespace DataAccess.DTOs
{
    public class AccuracyToDTOConvertStrategy : ConvertStrategy<AccuracyDTO, IAccuracy>
    {
        private IUpdateStrategy<IAccuracy> updateStrategy;
        public override AccuracyDTO GetNewItem(IAccuracy source)
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
