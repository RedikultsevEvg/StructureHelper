using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.JsonConverters
{
    /// <inheritdoc/>
    public class GetIdFromObjectLogic : IGetIdFromObjectLogic
    {
        /// <inheritdoc/>
        public IShiftTraceLogger? TraceLogger { get; set; }
        /// <inheritdoc/>
        public Guid GetId(object obj)
        {
            var idProp = obj.GetType().GetProperty("Id");
            return idProp != null ? (Guid)idProp.GetValue(obj) : Guid.Empty;
        }
    }
}
