using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class HasPrimitivesToDTOUpdateStrategy : IUpdateStrategy<IHasPrimitives>
    {
        private IConvertStrategy<INdmPrimitive, INdmPrimitive> convertStrategy;

        public HasPrimitivesToDTOUpdateStrategy(IConvertStrategy<INdmPrimitive, INdmPrimitive> primitiveConvertStrategy)
        {
            this.convertStrategy = primitiveConvertStrategy;
        }

        public HasPrimitivesToDTOUpdateStrategy() : this(new NdmPrimitiveToDTOConvertStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public void Update(IHasPrimitives targetObject, IHasPrimitives sourceObject)
        {
            if (sourceObject.Primitives is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull);
            }
            targetObject.Primitives.Clear();
            ProcessPrimitives(targetObject, sourceObject);
        }

        private void ProcessPrimitives(IHasPrimitives targetObject, IHasPrimitives sourceObject)
        {
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            foreach (var item in sourceObject.Primitives)
            {
                INdmPrimitive primtiveDTO = convertStrategy.Convert(item);
                targetObject.Primitives.Add(primtiveDTO);
            }
        }
    }
}
