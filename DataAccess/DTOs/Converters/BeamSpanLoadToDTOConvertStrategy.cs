using DataAccess.DTOs.DTOEntities;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class BeamSpanLoadToDTOConvertStrategy : ConvertStrategy<IBeamSpanLoad, IBeamSpanLoad>
    {
        public BeamSpanLoadToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override IBeamSpanLoad GetNewItem(IBeamSpanLoad source)
        {
            try
            {
                GetNewBeamAction(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }

        }

        private void GetNewBeamAction(IBeamSpanLoad source)
        {
            TraceLogger?.AddMessage($"Converting of beam shear action Id = {source.Id} has been started", TraceLogStatuses.Debug);
            if (source is IDistributedLoad distributedLoad)
            {
                ProcessDistributedLoad(distributedLoad);
            }
            else if (source is IConcentratedForce concentratedForce)
            {
                ProcessConcentratedForce(concentratedForce);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source) + ": type of span load");
            }
            TraceLogger?.AddMessage($"Converting of beam shear action Id = {source.Id} has been finished", TraceLogStatuses.Debug);
        }

        private void ProcessConcentratedForce(IConcentratedForce concentratedForce)
        {
            var convertStrategy = new DictionaryConvertStrategy<ConcentratedForceDTO, IConcentratedForce>
                (this, new ConcentratedForceToDTOConvertStrategy(this));
            ConcentratedForceDTO concentratedForceDTO = convertStrategy.Convert(concentratedForce);
            NewItem = concentratedForceDTO;
        }

        private void ProcessDistributedLoad(IDistributedLoad distributedLoad)
        {
            var convertStrategy = new DictionaryConvertStrategy<DistributedLoadDTO, IDistributedLoad>
                (this, new DistributedLoadToDTOConvertStrategy(this));
            DistributedLoadDTO distributedLoadDTO = convertStrategy.Convert(distributedLoad);
            NewItem = distributedLoadDTO;
        }
    }
}
