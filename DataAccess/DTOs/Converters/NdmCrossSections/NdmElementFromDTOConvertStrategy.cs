using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace DataAccess.DTOs
{
    public class NdmElementFromDTOConvertStrategy : ConvertStrategy<INdmElement, INdmElement>
    {
        private IUpdateStrategy<INdmElement> updateStrategy;
        private IUpdateStrategy<IForceTuple> forceUpdateStrategy = new ForceTupleUpdateStrategy();
        private NdmElement newItem;

        public NdmElementFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override INdmElement GetNewItem(INdmElement source)
        {
            if (source is not NdmElementDTO sourceDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source));
            }
            newItem = new NdmElement(sourceDTO.Id);
            updateStrategy = new NdmElementUpdateStrategy() { UpdateChildren = false};
            updateStrategy.Update(newItem, sourceDTO);
            forceUpdateStrategy.Update(newItem.UsersPrestrain, source.UsersPrestrain);
            //newItem.UsersPrestrain.Id = source.UsersPrestrain.Id;
            //forceUpdateStrategy.Update(newItem.AutoPrestrain, source.AutoPrestrain);
            //(newItem.AutoPrestrain as ForceTupleDTO).Id = source.AutoPrestrain.Id;
            NewItem = newItem;
            return NewItem;
        }
    }
}
