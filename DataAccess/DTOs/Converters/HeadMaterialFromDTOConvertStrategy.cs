using DataAccess.DTOs.Converters;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class HeadMaterialFromDTOConvertStrategy : ConvertStrategy<IHeadMaterial, IHeadMaterial>
    {
        private IUpdateStrategy<IHeadMaterial> updateStrategy;
        private IConvertStrategy<IHelperMaterial, IHelperMaterial> helperMaterialConvertStrategy;

        public HeadMaterialFromDTOConvertStrategy(
            IUpdateStrategy<IHeadMaterial> updateStrategy,
            IConvertStrategy<IHelperMaterial, IHelperMaterial> helperMaterialConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.helperMaterialConvertStrategy = helperMaterialConvertStrategy;
        }

        public HeadMaterialFromDTOConvertStrategy() : this (
            new HeadMaterialBaseUpdateStrategy(),
            new HelperMaterialFromDTOConvertStrategy())
        {
            
        }
        public override IHeadMaterial GetNewItem(IHeadMaterial source)
        {
            TraceLogger?.AddMessage("Head material converting is started", TraceLogStatuses.Service);
            HeadMaterial newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            IHelperMaterial helperMaterial = GetHelperMaterial(source.HelperMaterial);
            newItem.HelperMaterial = helperMaterial;
            //GlobalRepository
            TraceLogger?.AddMessage($"Head material Name = {newItem.Name} converting has been finished successfully", TraceLogStatuses.Service);
            return newItem;
        }

        private IHelperMaterial GetHelperMaterial(IHelperMaterial source)
        {
            TraceLogger?.AddMessage("Helper material converting is started", TraceLogStatuses.Service);
            helperMaterialConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            helperMaterialConvertStrategy.TraceLogger = TraceLogger;
            IHelperMaterial newItem = helperMaterialConvertStrategy.Convert(source);
            TraceLogger?.AddMessage($"Object of type <<{newItem.GetType()}>> was obtained", TraceLogStatuses.Service);
            return newItem;
        }

    }
}
