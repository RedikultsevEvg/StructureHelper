using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    public class BeamShearToDTOConvertStrategy : ConvertStrategy<BeamShearDTO, IBeamShear>
    {
        private IUpdateStrategy<IBeamShear> updateStrategy;
        private IConvertStrategy<BeamShearRepositoryDTO, IBeamShearRepository> repositoryConvertStrategy;

        public BeamShearToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override BeamShearDTO GetNewItem(IBeamShear source)
        {
            try
            {
                GetNewBeamShear(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewBeamShear(IBeamShear source)
        {
            TraceLogger?.AddMessage($"Converting of beam shear Id = {source.Id} has been started");
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.Repository = repositoryConvertStrategy.Convert(source.Repository);
            TraceLogger?.AddMessage($"Converting of beam shear Id = {NewItem.Id} has been finished successfully");
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearUpdateStrategy();
            repositoryConvertStrategy = new DictionaryConvertStrategy<BeamShearRepositoryDTO, IBeamShearRepository>(
                ReferenceDictionary,
                TraceLogger,
                new BeamShearRepositoryToDTOConvertStrategy(ReferenceDictionary, TraceLogger)
                );
        }
    }
}
