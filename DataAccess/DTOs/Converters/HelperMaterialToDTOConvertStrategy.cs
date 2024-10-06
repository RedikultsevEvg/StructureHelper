using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    internal class HelperMaterialToDTOConvertStrategy : IConvertStrategy<IHelperMaterial, IHelperMaterial>
    {
        private LibMaterialToDTOConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial> concreteConvertStrategy;
        private LibMaterialToDTOConvertStrategy<ReinforcementLibMaterialDTO, IReinforcementLibMaterial> reinforcementConvertStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public HelperMaterialToDTOConvertStrategy(
            LibMaterialToDTOConvertStrategy<ConcreteLibMaterialDTO, IConcreteLibMaterial> concreteConvertStrategy,
            LibMaterialToDTOConvertStrategy<ReinforcementLibMaterialDTO, IReinforcementLibMaterial> reinforcementConvertStrategy)
        {
            this.concreteConvertStrategy = concreteConvertStrategy;
            this.reinforcementConvertStrategy = reinforcementConvertStrategy;
        }

        public HelperMaterialToDTOConvertStrategy() : this (
            new ConcreteLibMaterialToDTOConvertStrategy(),
            new ReinforcementLibMaterialToDTOConvertStrategy())
        {
            
        }

        public IHelperMaterial Convert(IHelperMaterial source)
        {
            Check();
            if (source is IConcreteLibMaterial concreteLibMaterial)
            {
                concreteConvertStrategy.ReferenceDictionary = ReferenceDictionary;
                concreteConvertStrategy.TraceLogger = TraceLogger;
                return concreteConvertStrategy.Convert(concreteLibMaterial);
            }
            if (source is IReinforcementLibMaterial reinforcementMaterial)
            {
                reinforcementConvertStrategy.ReferenceDictionary = ReferenceDictionary;
                reinforcementConvertStrategy.TraceLogger = TraceLogger;
                return reinforcementConvertStrategy.Convert(reinforcementMaterial);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source));
            }
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<IHelperMaterial, IHelperMaterial>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }
    }
}
