using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearFromDTOConvertStrategy : ConvertStrategy<BeamShear, BeamShearDTO>
    {
        private IUpdateStrategy<IBeamShear> updateStrategy;

        public BeamShearFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override BeamShear GetNewItem(BeamShearDTO source)
        {
            ChildClass = this;
            CheckObject.IsNull(source);
            CheckObject.IsNull(source.Repository);
            GetNewBeamShear(source);
            return NewItem;
        }

        private void GetNewBeamShear(IBeamShear source)
        {
            if (source.Repository is not BeamShearRepositoryDTO repositoryDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.Repository));
            }
            updateStrategy ??= new BeamShearUpdateStrategy();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            BeamShearRepositoryFromDTOConvertStrategy convertStrategy = new(this);
            NewItem.Repository = convertStrategy.Convert(repositoryDTO);
        }
    }
}
