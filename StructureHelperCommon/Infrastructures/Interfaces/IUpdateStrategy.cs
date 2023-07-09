using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Logic for update object of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    public interface IUpdateStrategy<T>
    {
        /// <summary>
        /// Update properties of target object from source object
        /// </summary>
        /// <param name="targetObject">Target object</param>
        /// <param name="sourceObject">Source object</param>
        void Update(T targetObject, T sourceObject);
    }
}
