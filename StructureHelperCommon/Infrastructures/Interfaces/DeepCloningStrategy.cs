using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Creates deep copy of object by dictionary (if copy of object is not created it create copy, otherwise take copy from dictionary) 
    /// </summary>
    public class DeepCloningStrategy : ICloningStrategy
    {
        private readonly Dictionary<object, object> _clonedObjects = new();

        /// <inheritdoc/>
        public T Clone<T>(T original, ICloneStrategy<T>? cloneStrategy = null) where T : class
        {
            if (original == null) return null;

            // Check if the object is already cloned
            if (_clonedObjects.TryGetValue(original, out var existingClone))
                return (T)existingClone;

            // Perform custom cloning logic based on the object's type
            T clone;
            if (cloneStrategy is not null)
            {
                clone = cloneStrategy.GetClone(original);
            }
            // Check if the object implements ICloneable (or IClone) and use that method
            else if (original is ICloneable cloneable)
            {
                clone = cloneable.Clone() as T; // Use the ICloneable interface's Clone method
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": object is not IClonable and cloning strategy is null");

            }
            _clonedObjects[original] = clone;

            return clone;
        }
    }

}
