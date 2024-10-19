using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceCalculatorToDTOConvertStrategy : IConvertStrategy<ForceCalculatorDTO, IForceCalculator>
    {
        private readonly IUpdateStrategy<IForceCalculator> updateStrategy;

        public ForceCalculatorToDTOConvertStrategy(IUpdateStrategy<IForceCalculator> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ForceCalculatorToDTOConvertStrategy() : this (
            new ForceCalculatorUpdateStrategy()
            )
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public ForceCalculatorDTO Convert(IForceCalculator source)
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

        private ForceCalculatorDTO GetNewItem(IForceCalculator source)
        {
            ForceCalculatorDTO newItem = new() { Id = source.Id};
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
            var checkLogic = new CheckConvertLogic<ForceCalculatorDTO, IForceCalculator>(this);
            checkLogic.Check();
        }
    }
}
