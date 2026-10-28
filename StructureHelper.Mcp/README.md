# StructureHelper MCP server

This is a standalone stdio MCP server. It reads StructureHelper `.shpj` files using the existing `DataAccess` project and does not start the WPF application.

## Run locally

From the repository root:

```powershell
dotnet run --project .\StructureHelper.Mcp\StructureHelper.Mcp.csproj
```

The process communicates over standard input/output. Logging is sent to standard error so it does not interfere with the MCP protocol.

## Available tool

- `get_project_summary` reads a `.shpj` file and returns its file name, path, analysis count, and each analysis' name, tags, comment, type, and version count. It does not change the project file.

## Configure in VS Code

Add the following to `.vscode\mcp.json` in the workspace:

```json
{
  "servers": {
    "structurehelper": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "${workspaceFolder}\\StructureHelper.Mcp\\StructureHelper.Mcp.csproj"
      ]
    }
  }
}
```
