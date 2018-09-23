using System;
using System.Collections.Generic;

namespace Benchmark.Formatters
{
  internal class CsvReportFormatter : AbstractReportTableFormatter
  {
    protected override bool HasRowSeparators { get; } = false;

    protected override string FormatContextRowSeparator(Column column)
    {
      throw new NotSupportedException();
    }

    protected override string FormatHeader(Column column)
    {
      return column.Header;
    }

    protected override string FormatHeaderRowSeparator(Column column)
    {
      throw new NotSupportedException();
    }

    protected override string FormatValue(Column column, int rowGroupIndex, int rowIndex)
    {
      return string.Concat("\"", column.RowGroups[rowGroupIndex][rowIndex], "\"");
    }

    protected override string BuildLine(IEnumerable<string> columnValues)
    {
      return string.Join(",", columnValues);
    }

    public string Format(
      IEnumerable<BenchmarkContextMetrics> contextMetrics,
      RankColumn order)
    {
      return DoFormat(contextMetrics, order, false);
    }
  }
}
