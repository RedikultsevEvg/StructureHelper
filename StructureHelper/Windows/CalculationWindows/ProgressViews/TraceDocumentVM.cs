using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.CalculationWindows.ProgressViews
{
    public class TraceDocumentVM : ViewModelBase
    {
        const double tabFactor = 500d;
        private readonly IEnumerable<ITraceLoggerEntry> loggerEntries;
        private IEnumerable<ITraceLoggerEntry> selectedLoggerEntries;
        private FlowDocument document;
        private ICommand rebuildCommand;
        private ICommand printDocumentCommand;
        private int maxPriority;
        private int tabGap;

        public FlowDocumentReader DocumentReader { get; set; }
        public int MaxPriority
        {
            get => maxPriority; set
            {
                var oldValue = maxPriority;
                try
                {
                    maxPriority = Math.Max(value, 0);
                    OnPropertyChanged(nameof(MaxPriority));
                }
                catch (Exception)
                {
                    maxPriority = oldValue;
                }
            }
        }
        public int TabGap
        {
            get => tabGap; set
            {
                var oldValue = tabGap;
                try
                {
                    tabGap = Math.Max(value, 0);
                    OnPropertyChanged(nameof(TabGap));
                }
                catch (Exception)
                {
                    tabGap = oldValue;
                }
            }
        }
        public TraceDocumentVM(IEnumerable<ITraceLoggerEntry> loggerEntries)
        {
            this.loggerEntries = loggerEntries;
            maxPriority = 350;
            tabGap = 30;
        }

        public ICommand RebuildCommand =>
            rebuildCommand ??= new RelayCommand(o =>
            {
                Show();
            });

        public ICommand PrintDocumentCommand =>
            printDocumentCommand ??= new RelayCommand(o =>
            {
                SafetyProcessor.RunSafeProcess(DocumentReader.Print, "Error of printing document");
            });


        public void Prepare()
        {
            document = new();
            selectedLoggerEntries = loggerEntries.Where(x => x.Priority <= MaxPriority);
            var blocks = selectedLoggerEntries.Select(x => GetBlockByEntry(x));
            document.Blocks.AddRange(blocks);
        }
        public void ShowPrepared()
        {
            DocumentReader.Document = document;
        }
        public void Show()
        {
            Prepare();
            ShowPrepared();
        }

        private Block GetBlockByEntry(ITraceLoggerEntry traceEntry)
        {
            Block block;
            if (traceEntry is StringLogEntry stringEntry)
            {
                block = GetBlockByStringEntry(stringEntry);
            }
            else if (traceEntry is TableLogEntry tableEntry)
            {
                block = GetBlockByTableEntry(tableEntry);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(traceEntry));
            }
            block.Margin = new Thickness(traceEntry.Priority / tabFactor * tabGap, 7, 0, 7);
            return block;
        }

        private Block GetBlockByStringEntry(StringLogEntry stringEntry)
        {
            var paragraph = new Paragraph(new Run(stringEntry.Message));
            if (stringEntry.Priority <= LoggerService.GetPriorityByStatus(TraceLogStatuses.Fatal))
            {
                paragraph.FontSize = 14;
                paragraph.Background = Brushes.Red;
                paragraph.Foreground = Brushes.Black;
                paragraph.FontStyle = FontStyles.Italic;
            }
            else if (stringEntry.Priority <= LoggerService.GetPriorityByStatus(TraceLogStatuses.Error))
            {
                paragraph.FontSize = 14;
                paragraph.Background = Brushes.Pink;
                paragraph.Foreground = Brushes.Black;
            }
            else if (stringEntry.Priority <= LoggerService.GetPriorityByStatus(TraceLogStatuses.Warning))
            {
                paragraph.FontSize = 14;
                paragraph.Background = Brushes.Yellow;
                paragraph.Foreground = Brushes.Black;
            }
            else if (stringEntry.Priority <= LoggerService.GetPriorityByStatus(TraceLogStatuses.Debug))
            {
                paragraph.FontSize = 12;
                paragraph.Foreground = Brushes.Black;
            }
            else
            {
                paragraph.FontSize = 10;
                paragraph.Foreground = Brushes.Gray;
            }

            return paragraph;
        }

        private Table GetBlockByTableEntry(TableLogEntry tableEntry)
        {
            const int columnWidth = 150;
            var rows = tableEntry.Table.GetAllRows();
            int rowCount = rows.Count();
            int columnCount = tableEntry.Table.RowSize;
            var table = new Table();
            for (int x = 0; x < columnCount; x++)
            {
                var tableColumn = new TableColumn();
                tableColumn.Width = new GridLength(columnWidth);
                table.Columns.Add(tableColumn);
            }
            foreach (var row in rows)
            {
                TableRow newRow = GetTableRow(row);
                table.RowGroups.Add(new TableRowGroup());
                table.RowGroups[0].Rows.Add(newRow);
            }
            return table;
        }

        private TableRow GetTableRow(IShTableRow<ITraceLoggerEntry> row)
        {
            var newRow = new TableRow();
            foreach (var cell in row.Elements)
            {
                TableCell tableCell;
                if (cell is null)
                {
                    tableCell = new TableCell(new Paragraph(new Run(string.Empty)));
                }
                else
                {
                    var cellvalue = GetBlockByEntry(cell.Value);
                    tableCell = new TableCell(cellvalue);
                    AdjustTableCell(cell, tableCell);
                }
                newRow.Cells.Add(tableCell);
            }

            return newRow;
        }

        private static void AdjustTableCell(IShTableCell<ITraceLoggerEntry>? cell, TableCell tableCell)
        {
            tableCell.ColumnSpan = cell.ColumnSpan;
            if (cell.Role == CellRole.Regular)
            {
                tableCell.TextAlignment = TextAlignment.Left;
                tableCell.Background = Brushes.LightYellow;
            }
            else if (cell.Role == CellRole.Header)
            {
                tableCell.TextAlignment = TextAlignment.Center;
                tableCell.Background = Brushes.AliceBlue;
            }
        }
    }
}
