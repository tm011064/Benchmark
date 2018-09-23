using System.Collections.Generic;
using System.Linq;

namespace Benchmark.Formatters
{
  internal class DefaultReportFormatter : AbstractReportTableFormatter
  {
    protected override bool HasRowSeparators { get; } = true;

    protected override string FormatContextRowSeparator(Column column)
    {
      return new string(Enumerable.Range(0, column.Width).Select(_ => '-').ToArray());
    }

    protected override string FormatHeader(Column column)
    {
      return column.Header.PadRight(column.Width);
    }

    protected override string FormatHeaderRowSeparator(Column column)
    {
      return new string(Enumerable.Range(0, column.Width).Select(_ => '-').ToArray());
    }

    protected override string FormatValue(Column column, int rowGroupIndex, int rowIndex)
    {
      var value = column.RowGroups[rowGroupIndex][rowIndex];

      return column.HorizontalAlignment == HorizontalAlignment.Left
        ? value.PadRight(column.Width)
        : value.PadLeft(column.Width);
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
