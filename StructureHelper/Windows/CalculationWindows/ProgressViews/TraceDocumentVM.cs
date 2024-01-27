using StructureHelper.Infrastructure;
using StructureHelper.Windows.AddMaterialWindow;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.CalculationWindows.ProgressViews
{
    public class TraceDocumentVM : ViewModelBase
    {
        IEnumerable<ITraceLoggerEntry> loggerEntries;
        IEnumerable<ITraceLoggerEntry> selectedLoggerEntries;
        FlowDocument document;
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
            tabGap = 50;
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

        public void Show()
        {
            Prepare();
            ShowPrepared();
        }

        public void Prepare()
        {
            document = new();
            selectedLoggerEntries = loggerEntries.Where(x => x.Priority <= MaxPriority);
            foreach (var item in selectedLoggerEntries)
            {
                ProcessLoggerEntries(item);
            }
        }

        private void ProcessLoggerEntries(ITraceLoggerEntry item)
        {
            if (item is StringLoggerEntry stringEntry)
            {
                ProcessStringEntry(stringEntry);
            }
            else if (item is TableLoggerEntry tableEntry)
            {
                ProcessTableEntry(tableEntry);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(item));
            }
        }

        private void ProcessTableEntry(TableLoggerEntry tableEntry)
        {
            var rows = tableEntry.Table.GetAllRows();
            int rowCount = rows.Count();
            int columnCount = tableEntry.Table.RowSize;
            var table = new Table();
            for (int x = 0; x < columnCount; x++)
            {
                var tableColumn = new TableColumn();
                tableColumn.Width = new GridLength(150);
                table.Columns.Add(tableColumn);
            }
            foreach (var row in rows)
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
                        if (cell.Value is StringLoggerEntry stringEntry)
                        {
                            tableCell = new TableCell(GetParagraphByStringEntry(stringEntry));
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
                        else
                        {
                            throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cell));
                        }
                    }
                    newRow.Cells.Add(tableCell);
                }
                table.RowGroups.Add(new TableRowGroup());
                table.RowGroups[0].Rows.Add(newRow);
            }
            document.Blocks.Add(table);
        }

        private void ProcessStringEntry(StringLoggerEntry stringEntry)
        {
            var paragraph = GetParagraphByStringEntry(stringEntry);
            document.Blocks.Add(paragraph);
        }

        private Paragraph GetParagraphByStringEntry(StringLoggerEntry stringEntry)
        {
            var paragraph = new Paragraph(new Run(stringEntry.Message));
            paragraph.Margin = new Thickness(stringEntry.Priority / tabGap);
            if (stringEntry.Priority <= LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Fatal))
            {
                paragraph.FontSize = 14;
                paragraph.Background = Brushes.Red;
                paragraph.Foreground = Brushes.Black;
                paragraph.FontStyle = FontStyles.Italic;
            }
            else if (stringEntry.Priority <= LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Error))
            {
                paragraph.FontSize = 14;
                paragraph.Background = Brushes.Pink;
                paragraph.Foreground = Brushes.Black;
            }
            else if (stringEntry.Priority <= LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Warning))
            {
                paragraph.FontSize = 14;
                paragraph.Background = Brushes.Yellow;
                paragraph.Foreground = Brushes.Black;
            }
            else if (stringEntry.Priority <= LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Debug))
            {
                paragraph.FontSize = 12;
                paragraph.Foreground = Brushes.Black;
            }
            else
            {
                paragraph.FontSize = 8;
                paragraph.Foreground = Brushes.Gray;

            }

            return paragraph;
        }

        public void ShowPrepared()
        {
            DocumentReader.Document = document;
        }
    }
}
