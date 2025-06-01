using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.NdmCalculations.Analyses;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.BeamShears
{
    public class ExportActionResultToCSVLogic : ExportToCSVLogicBase
    {
        private IBeamShearActionResult result;

        public ExportActionResultToCSVLogic(IBeamShearActionResult result)
        {
            this.result = result;
        }

        public override void ExportHeadings()
        {
            string[] headings =
                {
                    "Limit State",
                    "Calc duration",
                    "Section start,m",
                    "Section end, m",
                    "Nz, N",
                    "Qy, N",
                    "Width, m",
                    "Effective depth, m",
                    "Full depth, m",
                    "Concrete compressive strength, Pa",
                    "Concrete tensile strength, Pa",
                    "Limit force, N",
                    "Concrete force, N",
                    "Stirrups force, N",
                    "Description",
                };
            output.AppendLine(string.Join(separator, headings));
        }
        public override void ExportBoby()
        {
            foreach (var item in result.SectionResults)
            {
                string[] newLine =
                    {
                            result.LimitState.ToString(),
                            result.CalcTerm.ToString(),
                            item.InputData.InclinedSection.StartCoord.ToString(),
                            item.InputData.InclinedSection.EndCoord.ToString(),
                            item.InputData.ForceTuple.Nz.ToString(),
                            item.InputData.ForceTuple.Qy.ToString(),
                            item.InputData.InclinedSection.WebWidth.ToString(),
                            item.InputData.InclinedSection.EffectiveDepth.ToString(),
                            item.InputData.InclinedSection.FullDepth.ToString(),
                            item.InputData.InclinedSection.ConcreteCompressionStrength.ToString(),
                            item.InputData.InclinedSection.ConcreteTensionStrength.ToString(),
                            item.TotalStrength.ToString(),
                            item.ConcreteStrength.ToString(),
                            item.StirrupStrength.ToString(),
                            item.Description,
                        };
                output.AppendLine(string.Join(separator, newLine));
            }
        }
    }
}
