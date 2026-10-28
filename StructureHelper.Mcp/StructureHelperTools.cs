using System.ComponentModel;
using System.Text.Json;
using DataAccess.Infrastructures;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using StructureHelperCommon.Models.Analyses;

namespace StructureHelper.Mcp;

[McpServerToolType]
public sealed class StructureHelperTools
{
    [McpServerTool, Description("Read a StructureHelper .shpj file and return its project and analysis summary. This tool does not modify the file.")]
    public static string GetProjectSummary(
        IProjectAccessLogic projectAccessLogic,
        [Description("Absolute path to a StructureHelper .shpj project file.")] string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new McpException("A project file path is required.");
        }

        if (!string.Equals(Path.GetExtension(filePath), ".shpj", StringComparison.OrdinalIgnoreCase))
        {
            throw new McpException("The file must have the .shpj extension.");
        }

        if (!File.Exists(filePath))
        {
            throw new McpException($"Project file not found: {filePath}");
        }

        var result = projectAccessLogic.OpenProject(filePath);
        if (!result.IsValid || result.Project is null)
        {
            throw new McpException(result.Description ?? "The project file could not be opened.");
        }

        var summary = new
        {
            result.Project.FileName,
            result.Project.FullFileName,
            AnalysisCount = result.Project.VisualAnalyses.Count,
            Analyses = result.Project.VisualAnalyses.Select(visualAnalysis => new
            {
                visualAnalysis.Analysis.Name,
                visualAnalysis.Analysis.Tags,
                visualAnalysis.Analysis.Comment,
                Type = visualAnalysis.Analysis.GetType().Name,
                VersionCount = visualAnalysis.Analysis.VersionProcessor.Versions.Count
            })
        };

        return JsonSerializer.Serialize(summary, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
