using System.Collections.Generic;

namespace Benchmark.Formatters
{
  internal class MarkupReportFormatter : AbstractReportTableFormatter
  {
    protected override bool HasRowSeparators { get; } = true;

    protected override string FormatContextRowSeparator(Column column)
    {
      return " ";
    }

    protected override string FormatHeader(Column column)
    {
      return column.Header;
    }

    protected override string FormatHeaderRowSeparator(Column column)
    {
      return "---";
    }

    protected override string FormatValue(Column column, int rowGroupIndex, int rowIndex)
    {
      return column.RowGroups[rowGroupIndex][rowIndex];
    }

    protected override string BuildLine(IEnumerable<string> columnValues)
    {
      return string
        .Concat(
          " | ",
          string.Join(" | ", columnValues),
          " | ")
        .Trim();
    }

    public string Format(
      IEnumerable<BenchmarkContextMetrics> contextMetrics,
      RankColumn order)
    {
      return DoFormat(contextMetrics, order, true);
    }
  }
}
