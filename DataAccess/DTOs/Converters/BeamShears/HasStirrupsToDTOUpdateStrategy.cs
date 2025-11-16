using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class HasStirrupsToDTOUpdateStrategy : IUpdateStrategy<IHasStirrups>
    {
        private Dictionary<(Guid id, Type type), ISaveable> referenceDictionary;
        private IShiftTraceLogger traceLogger;
        private IUpdateStrategy<IHasVisualProperty> visualUpdateStrategy;

        public HasStirrupsToDTOUpdateStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            this.referenceDictionary = referenceDictionary;
            this.traceLogger = traceLogger;
        }

        public void Update(IHasStirrups targetObject, IHasStirrups sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.ThrowIfNull(sourceObject.Stirrups);
            CheckObject.ThrowIfNull(targetObject.Stirrups);
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
            traceLogger?.AddMessage($"Stirrup Id = {stirrup.Id} has been started", TraceLogStatuses.Debug);
            IStirrup newItem;
            if (stirrup is IStirrupByRebar rebar)
            {
                newItem = ProcessRebar(rebar);
            }
            else if (stirrup is IStirrupGroup group)
            {
                newItem = ProcessGroup(group);
            }
            else if (stirrup is IStirrupByInclinedRebar inclinatedRebar)
            {
                newItem = ProcessInclinatedRebar(inclinatedRebar);
            }
            else if (stirrup is IStirrupByDensity density)
            {
                newItem = ProcessDensity(density);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(stirrup));
            }
            visualUpdateStrategy = new HasVisualPropertyToDTOUpdateStrategy(referenceDictionary, traceLogger);
            visualUpdateStrategy.Update(newItem, stirrup);
            return newItem;
        }

        private IStirrup ProcessInclinatedRebar(IStirrupByInclinedRebar inclinedRebar)
        {
            traceLogger?.AddMessage("Stirrup is inclined rebar");
            var convertStrategy = new DictionaryConvertStrategy<StirrupByInclinedRebarDTO, IStirrupByInclinedRebar>
                (referenceDictionary, traceLogger, new StirrupByInclinedRebarToDTOConvertStrategy(referenceDictionary, traceLogger));
            return convertStrategy.Convert(inclinedRebar);
        }

        private IStirrup ProcessGroup(IStirrupGroup group)
        {
            traceLogger?.AddMessage("Stirrup is stirrup group");
            var convertStrategy = new DictionaryConvertStrategy<StirrupGroupDTO, IStirrupGroup>
                (referenceDictionary, traceLogger, new StirrupGroupToDTOConvertStrategy(referenceDictionary, traceLogger));
            return convertStrategy.Convert(group);
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
