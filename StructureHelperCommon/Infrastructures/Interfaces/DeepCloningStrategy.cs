using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public class DeepCloningStrategy : ICloningStrategy
    {
        private readonly Dictionary<object, object> _clonedObjects = new Dictionary<object, object>();

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

        //private T CreateClone<T>(T original) where T : class
        //{
        //    // Example logic for creating a clone (modify as needed for your use case)
        //    var type = original.GetType();
        //    var clone = Activator.CreateInstance(type);

        //    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        //    {
        //        var value = field.GetValue(original);
        //        field.SetValue(clone, Clone(value));
        //    }

        //    return (T)clone;
        //}
    }

}
