using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    internal class HasStirrupsFromDTOUpdateStrategy : IUpdateStrategy<IHasStirrups>
    {
        private Dictionary<(Guid id, Type type), ISaveable> referenceDictionary;
        private IShiftTraceLogger traceLogger;
        private IUpdateStrategy<IHasVisualProperty> visualUpdateStrategy;

        public HasStirrupsFromDTOUpdateStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
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
            if (stirrup is StirrupByRebarDTO rebar)
            {
                newItem = ProcessRebar(rebar);
            }
            else if (stirrup is StirrupByDensityDTO density)
            {
                newItem = ProcessDensity(density);
            }
            else if (stirrup is StirrupGroupDTO stirrupGroup)
            {
                newItem = ProcessStirrupGroup(stirrupGroup);
            }
            else if (stirrup is StirrupByInclinedRebarDTO inclinedRebar)
            {
                newItem = ProcessInclinedRebar(inclinedRebar);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(stirrup));
            }
            visualUpdateStrategy = new HasVisualPropertyFromDTOUpdateStrategy(referenceDictionary, traceLogger);
            visualUpdateStrategy.Update(newItem, stirrup);
            return newItem;
        }

        private IStirrup ProcessInclinedRebar(StirrupByInclinedRebarDTO inclinedRebar)
        {
            traceLogger?.AddMessage("Stirrup is stirrup by inclined rebar");
            var convertStrategy = new DictionaryConvertStrategy<StirrupByInclinedRebar, IStirrupByInclinedRebar>
                (referenceDictionary, traceLogger, new StirrupByInclinedRebarFromDTOConvertStrategy(referenceDictionary, traceLogger));
            return convertStrategy.Convert(inclinedRebar);
        }

        private IStirrup ProcessStirrupGroup(StirrupGroupDTO stirrupGroup)
        {
            traceLogger?.AddMessage("Stirrup is stirrup group");
            var convertStrategy = new DictionaryConvertStrategy<StirrupGroup, IStirrupGroup>
                (referenceDictionary, traceLogger, new StirrupGroupFromDTOConvertStrategy(referenceDictionary, traceLogger));
            return convertStrategy.Convert(stirrupGroup);
        }

        private StirrupByDensity ProcessDensity(StirrupByDensityDTO density)
        {
            traceLogger?.AddMessage("Stirrup is stirrup by density");
            var convertStrategy = new DictionaryConvertStrategy<StirrupByDensity, StirrupByDensityDTO>
                (referenceDictionary, traceLogger, new StirrupByDensityFromDTOConvertStrategy(referenceDictionary, traceLogger));
            return convertStrategy.Convert(density);
        }

        private StirrupByRebar ProcessRebar(StirrupByRebarDTO rebar)
        {
            traceLogger?.AddMessage("Stirrup is stirrup by rebar");
            var convertStrategy = new DictionaryConvertStrategy<StirrupByRebar, StirrupByRebarDTO>
                (referenceDictionary, traceLogger, new StirrupByRebarFromDTOConvertStrategy(referenceDictionary, traceLogger));
            return convertStrategy.Convert(rebar);
        }
    }
}
