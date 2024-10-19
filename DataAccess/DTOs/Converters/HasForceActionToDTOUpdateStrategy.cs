using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class HasForceActionToDTOUpdateStrategy : IUpdateStrategy<IHasForceCombinations>
    {
        private readonly IConvertStrategy<IForceAction, IForceAction> forceActionStrategy;

        public HasForceActionToDTOUpdateStrategy(IConvertStrategy<IForceAction, IForceAction> forceActionStrategy)
        {
            this.forceActionStrategy = forceActionStrategy;
        }

        public HasForceActionToDTOUpdateStrategy() : this (new ForceActionToDTOConvertStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public void Update(IHasForceCombinations targetObject, IHasForceCombinations sourceObject)
        {
            if (sourceObject.ForceActions is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull);
            }
            targetObject.ForceActions.Clear();
            targetObject.ForceActions.AddRange(ProcessForceActions(sourceObject.ForceActions));
        }

        private List<IForceAction> ProcessForceActions(List<IForceAction> source)
        {
            List<IForceAction> forceActions = new();
            forceActionStrategy.ReferenceDictionary = ReferenceDictionary;
            forceActionStrategy.TraceLogger = TraceLogger;
            foreach (var item in source)
            {
                forceActions.Add(forceActionStrategy.Convert(item));
            }
            return forceActions;
        }
    }
}
