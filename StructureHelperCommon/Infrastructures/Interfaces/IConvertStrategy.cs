using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Converts object of type of ISaveable to another one
    /// </summary>
    /// <typeparam name="T">Target object type</typeparam>
    /// <typeparam name="V">Source object type</typeparam>
    public interface IConvertStrategy<T,V> : IBaseConvertStrategy
        where T :ISaveable
        where V :ISaveable
    {
        /// <summary>
        /// Converts object to another one
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Converted object</returns>
        T Convert(V source);
    }
}
