using DataAccess.DTOs.Converters;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace DataAccess.DTOs
{
    public class NdmElementDTOConvertStrategy : IConvertStrategy<NdmElementDTO, INdmElement>
    {
        private IUpdateStrategy<INdmElement> updateStrategy;
        private IConvertStrategy<HeadMaterialDTO, IHeadMaterial> headMaterialConvertStrategy;
        private IUpdateStrategy<IForceTuple> forceUpdateStrategy = new ForceTupleUpdateStrategy();

        public NdmElementDTOConvertStrategy(
            IUpdateStrategy<INdmElement> updateStrategy,
            IConvertStrategy<HeadMaterialDTO, IHeadMaterial> headMaterialConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.headMaterialConvertStrategy = headMaterialConvertStrategy;
        }

        public NdmElementDTOConvertStrategy() : this(
            new NdmElementUpdateStrategy(),
            new HeadMaterialToDTOConvertStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public NdmElementDTO Convert(INdmElement source)
        {
            Check();
            try
            {
                return GenNewNdmElementDTO(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private NdmElementDTO GenNewNdmElementDTO(INdmElement source)
        {
            NdmElementDTO newItem = new() { Id = source.Id };
            updateStrategy.Update(newItem, source);
            headMaterialConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            headMaterialConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<HeadMaterialDTO, IHeadMaterial>(this, headMaterialConvertStrategy);
            var headMaterial = convertLogic.Convert(source.HeadMaterial);
            newItem.HeadMaterial = headMaterial;
            forceUpdateStrategy.Update(newItem.UsersPrestrain, source.UsersPrestrain);
            (newItem.UsersPrestrain as ForceTupleDTO).Id = source.UsersPrestrain.Id;
            forceUpdateStrategy.Update(newItem.AutoPrestrain, source.AutoPrestrain);
            (newItem.AutoPrestrain as ForceTupleDTO).Id = source.AutoPrestrain.Id;
            return newItem;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<NdmElementDTO, INdmElement>(this);
            checkLogic.Check();
        }
    }
}
