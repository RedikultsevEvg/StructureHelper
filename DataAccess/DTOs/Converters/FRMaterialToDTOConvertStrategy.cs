using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class FRMaterialToDTOConvertStrategy : IConvertStrategy<FRMaterialDTO, IFRMaterial>
    {
        private IUpdateStrategy<IFRMaterial> updateStrategy;

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public FRMaterialToDTOConvertStrategy(IUpdateStrategy<IFRMaterial> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public FRMaterialToDTOConvertStrategy() : this (new FRUpdateStrategy())
        {
            
        }

        public FRMaterialDTO Convert(IFRMaterial source)
        {
            Check();
            try
            {
                FRMaterialDTO newItem = new() { Id = source.Id };
                updateStrategy.Update(newItem, source);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<FRMaterialDTO, IFRMaterial>(this);
            checkLogic.Check();
        }
    }
}
