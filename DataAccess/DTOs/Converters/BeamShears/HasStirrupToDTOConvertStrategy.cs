using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DataAccess.DTOs
{
    public class HasStirrupToDTOConvertStrategy : IUpdateStrategy<IHasStirrups>
    {
        private Dictionary<(Guid id, Type type), ISaveable> referenceDictionary;
        private IShiftTraceLogger traceLogger;

        public HasStirrupToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            this.referenceDictionary = referenceDictionary;
            this.traceLogger = traceLogger;
        }

        public void Update(IHasStirrups targetObject, IHasStirrups sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.IsNull(sourceObject.Stirrups);
            CheckObject.IsNull(targetObject.Stirrups);
            targetObject.Stirrups.Clear();
            List<IStirrup> stirrups = GetStirrups(sourceObject.Stirrups);
            targetObject.Stirrups.AddRange(stirrups);
        }

        private List<IStirrup> GetStirrups(IEnumerable<IStirrup> sourceStirrups)
        {
            List<IStirrup> stirrups = new();
            foreach (var stirrup in sourceStirrups)
            {
                IStirrup newItem = ProcessStirrup(stirrup);
                stirrups.Add(newItem);
            }
            return stirrups;
        }

        private IStirrup ProcessStirrup(IStirrup stirrup)
        {
            IStirrup newItem;
            if (stirrup is IStirrupByRebar rebar)
            {
                newItem = ProcessRebar(rebar);
            }
            else if (stirrup is IStirrupByDensity density)
            {
                newItem = ProcessDensity(density);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(stirrup));
            }

            return newItem;
        }

        private IStirrup ProcessDensity(IStirrupByDensity density)
        {
            traceLogger?.AddMessage("Stirrup is stirrup by density");
            var convertStrategy = new DictionaryConvertStrategy<StirrupByDensityDTO, IStirrupByDensity>
                (referenceDictionary, traceLogger, new StirrupByDensityToDTOConvertStrategy(referenceDictionary, traceLogger));
            return convertStrategy.Convert(density);
        }

        private StirrupByRebarDTO ProcessRebar(IStirrupByRebar rebar)
        {
            traceLogger?.AddMessage("Stirrup is stirrup by rebar");
            var convertStrategy = new DictionaryConvertStrategy<StirrupByRebarDTO, IStirrupByRebar>
                (referenceDictionary, traceLogger, new StirrupByRebarToDTOConvertStrategy(referenceDictionary, traceLogger));
            return convertStrategy.Convert(rebar);
        }
    }
}
