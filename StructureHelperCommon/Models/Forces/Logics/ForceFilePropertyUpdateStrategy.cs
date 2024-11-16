using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceFilePropertyUpdateStrategy : IUpdateStrategy<IForceFileProperty>
    {
        private IUpdateStrategy<IForceColumnProperty> columnUpdateStrategy;

        public ForceFilePropertyUpdateStrategy(IUpdateStrategy<IForceColumnProperty> columnUpdateStrategy)
        {
            this.columnUpdateStrategy = columnUpdateStrategy;
        }

        public ForceFilePropertyUpdateStrategy() : this (new ForceColumnPropertyUpdateStrategy())
        {
            
        }

        public void Update(IForceFileProperty targetObject, IForceFileProperty sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.LimitState = sourceObject.LimitState;
            targetObject.CalcTerm = sourceObject.CalcTerm;
            targetObject.FilePath = sourceObject.FilePath;
            targetObject.GlobalFactor = sourceObject.GlobalFactor;
            targetObject.SkipRowBeforeHeaderCount = sourceObject.SkipRowBeforeHeaderCount;
            targetObject.SkipRowHeaderCount = sourceObject.SkipRowHeaderCount;
            columnUpdateStrategy.Update(targetObject.Mx, sourceObject.Mx);
            columnUpdateStrategy.Update(targetObject.My, sourceObject.My);
            columnUpdateStrategy.Update(targetObject.Nz, sourceObject.Nz);
        }
    }
}
