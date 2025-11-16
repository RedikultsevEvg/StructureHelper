using StructureHelperCommon.Models;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public abstract class CheckEntityLogic<TEntity> : ICheckEntityLogic<TEntity> where TEntity : class
    {
        public TEntity Entity { get; set; }

        public virtual string CheckResult { get; set; } = string.Empty;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public abstract bool Check();

        public virtual void TraceMessage(string errorString)
        {
            CheckResult += errorString + "\n";
            TraceLogger?.AddMessage(errorString);
        }
    }
}
