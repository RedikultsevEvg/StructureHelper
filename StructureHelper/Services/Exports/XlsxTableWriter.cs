using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace StructureHelper.Services.Exports
{
    /// <summary>
    /// Writes a simple Excel workbook (*.xlsx) with one worksheet from a table of values.
    /// </summary>
    public static class XlsxTableWriter
    {
        public static void Write(
            string fileName,
            string sheetName,
            IReadOnlyList<string> headers,
            IEnumerable<IReadOnlyList<object?>> rows)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": file name");
            }
            if (headers is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": headers");
            }
            if (rows is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": rows");
            }

            using FileStream stream = File.Create(fileName);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
            WriteEntry(archive, "[Content_Types].xml", ContentTypesXml);
            WriteEntry(archive, "_rels/.rels", RootRelationshipsXml);
            WriteEntry(archive, "xl/workbook.xml", BuildWorkbookXml(sheetName));
            WriteEntry(archive, "xl/_rels/workbook.xml.rels", WorkbookRelationshipsXml);
            WriteEntry(archive, "xl/styles.xml", StylesXml);
            WriteEntry(archive, "xl/worksheets/sheet1.xml", BuildSheetXml(headers, rows));
        }

        private static string BuildSheetXml(
            IReadOnlyList<string> headers,
            IEnumerable<IReadOnlyList<object?>> rows)
        {
            var builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            builder.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><sheetData>");
            builder.Append("<row r=\"1\">");
            for (int column = 0; column < headers.Count; column++)
            {
                AppendInlineString(builder, 1, column, headers[column]);
            }
            builder.Append("</row>");

            int rowNumber = 2;
            foreach (IReadOnlyList<object?> row in rows)
            {
                builder.Append("<row r=\"").Append(rowNumber).Append("\">");
                for (int column = 0; column < row.Count; column++)
                {
                    AppendCell(builder, rowNumber, column, row[column]);
                }
                builder.Append("</row>");
                rowNumber++;
            }

            builder.Append("</sheetData></worksheet>");
            return builder.ToString();
        }

        private static void AppendCell(StringBuilder builder, int rowNumber, int column, object? value)
        {
            switch (value)
            {
                case null:
                    builder.Append("<c r=\"").Append(GetReference(rowNumber, column)).Append("\"/>");
                    break;
                case string text:
                    AppendInlineString(builder, rowNumber, column, text);
                    break;
                case bool boolean:
                    AppendInlineString(builder, rowNumber, column, boolean ? "TRUE" : "FALSE");
                    break;
                case double doubleValue:
                    AppendNumber(builder, rowNumber, column, doubleValue);
                    break;
                case float floatValue:
                    AppendNumber(builder, rowNumber, column, floatValue);
                    break;
                case decimal decimalValue:
                    AppendNumber(builder, rowNumber, column, (double)decimalValue);
                    break;
                case int intValue:
                    AppendNumber(builder, rowNumber, column, intValue);
                    break;
                case long longValue:
                    AppendNumber(builder, rowNumber, column, longValue);
                    break;
                case short shortValue:
                    AppendNumber(builder, rowNumber, column, shortValue);
                    break;
                case byte byteValue:
                    AppendNumber(builder, rowNumber, column, byteValue);
                    break;
                default:
                    AppendInlineString(builder, rowNumber, column, value.ToString() ?? string.Empty);
                    break;
            }
        }

        private static void AppendNumber(StringBuilder builder, int rowNumber, int column, double value)
        {
            builder.Append("<c r=\"").Append(GetReference(rowNumber, column)).Append("\"><v>")
                .Append(value.ToString("G17", CultureInfo.InvariantCulture))
                .Append("</v></c>");
        }

        private static void AppendInlineString(StringBuilder builder, int rowNumber, int column, string text)
        {
            builder.Append("<c r=\"").Append(GetReference(rowNumber, column)).Append("\" t=\"inlineStr\"><is><t>")
                .Append(Escape(text))
                .Append("</t></is></c>");
        }

        private static string GetReference(int rowNumber, int columnIndex)
            => GetColumnName(columnIndex) + rowNumber.ToString(CultureInfo.InvariantCulture);

        private static string GetColumnName(int columnIndex)
        {
            var name = string.Empty;
            int index = columnIndex + 1;
            while (index > 0)
            {
                int remainder = (index - 1) % 26;
                name = (char)('A' + remainder) + name;
                index = (index - 1) / 26;
            }
            return name;
        }

        private static string Escape(string text)
        {
            return text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;");
        }

        private static void WriteEntry(ZipArchive archive, string entryName, string content)
        {
            ZipArchiveEntry entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
            using Stream entryStream = entry.Open();
            using var writer = new StreamWriter(entryStream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            writer.Write(content);
        }

        private static string BuildWorkbookXml(string sheetName)
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
                + "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" "
                + "xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">"
                + "<sheets><sheet name=\"" + Escape(sheetName) + "\" sheetId=\"1\" r:id=\"rId1\"/></sheets>"
                + "</workbook>";
        }

        private const string ContentTypesXml = """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
              <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
              <Default Extension="xml" ContentType="application/xml"/>
              <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
              <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
              <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
            </Types>
            """;

        private const string RootRelationshipsXml = """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
              <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
            </Relationships>
            """;

        private const string WorkbookRelationshipsXml = """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
              <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
              <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
            </Relationships>
            """;

        private const string StylesXml = """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
              <fonts count="1">
                <font><sz val="11"/><name val="Calibri"/></font>
              </fonts>
              <fills count="2">
                <fill><patternFill patternType="none"/></fill>
                <fill><patternFill patternType="gray125"/></fill>
              </fills>
              <borders count="1"><border/></borders>
              <cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>
              <cellXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/></cellXfs>
              <cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles>
            </styleSheet>
            """;
    }
}
