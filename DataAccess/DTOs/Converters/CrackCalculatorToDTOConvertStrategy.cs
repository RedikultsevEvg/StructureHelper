using DataAccess.DTOs.Converters;
using DataAccess.DTOs.DTOEntities;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CrackCalculatorToDTOConvertStrategy : IConvertStrategy<CrackCalculatorDTO, ICrackCalculator>
    {
        private readonly IUpdateStrategy<ICrackCalculator> updateStrategy;

        public CrackCalculatorToDTOConvertStrategy(IUpdateStrategy<ICrackCalculator> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public CrackCalculatorToDTOConvertStrategy() : this (new CrackCalculatorUpdateStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public CrackCalculatorDTO Convert(ICrackCalculator source)
        {
            try
            {
                Check();
                return GetNewItem(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private CrackCalculatorDTO GetNewItem(ICrackCalculator source)
        {
            CrackCalculatorDTO newItem = new() { Id = source.Id};
            updateStrategy.Update(newItem, source);
            ProcessForceActions(newItem.InputData, source.InputData);
            ProcessPrimitives(newItem.InputData, source.InputData);
            return newItem;
        }

        private void ProcessPrimitives(IHasPrimitives target, IHasPrimitives source)
        {
            HasPrimitivesToDTOUpdateStrategy updateStrategy = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            updateStrategy.Update(target, source);
        }

        private void ProcessForceActions(IHasForceCombinations target, IHasForceCombinations source)
        {
            HasForceActionToDTOUpdateStrategy updateStrategy = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            updateStrategy.Update(target, source);
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<CrackCalculatorDTO, ICrackCalculator>(this);
            checkLogic.Check();
        }
    }
}
