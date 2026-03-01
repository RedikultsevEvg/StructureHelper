using StructureHelper.Windows.Graphs;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.FeaMaterials
{
    public class CdpToChartSeriesConvertStrategy : IObjectConvertStrategy<List<Series>, IFeaMaterialCDP>
    {
        const string StressUnitString = "MPa";
        private IUnit unitStress;

        public CdpToChartSeriesConvertStrategy()
        {
            GetUnitLogic unitLogic = new GetUnitLogic();
            unitStress = unitLogic.GetUnit(UnitTypes.Stress, StressUnitString);
        }
        public List<Series> Convert(IFeaMaterialCDP source)
        {
            List<Series> result = [];
            ArrayParameter<double> compressionParameter = GetParameter(source.CompressionStrain);
            Series compression = new(compressionParameter) { Name = "Compression"};
            result.Add(compression);
            ArrayParameter<double> tensionParameter = GetParameter(source.TensionStrain);
            Series tension = new(tensionParameter) { Name = "Tension"};
            result.Add(tension);
            return result;
        }

        private ArrayParameter<double> GetParameter(ICDPInelasticStrain source)
        {
            int rowCount = source.InelasticStrainList.Count;
            List<string> labels = ["inelastic strain", "stress", "damage"];
            ArrayParameter<double> compressionParameter = new(rowCount, labels.Count, labels);
            var data = compressionParameter.Data;
            for (int i = 0; i < rowCount; i++)
            {
                data[i, 0] = source.InelasticStrainList[i];
                data[i, 1] = source.StressList[i] * unitStress.Multiplayer;
                data[i, 2] = source.DamageList[i];
            }
            return compressionParameter;
        }
    }
}
