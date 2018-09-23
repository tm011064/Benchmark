using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benchmark.Formatters
{
  internal abstract class AbstractReportTableFormatter
  {
    protected abstract bool HasRowSeparators { get; }

    protected abstract string FormatHeader(Column column);

    protected abstract string FormatHeaderRowSeparator(Column column);

    protected abstract string FormatValue(Column column, int rowGroupIndex, int rowIndex);

    protected abstract string FormatContextRowSeparator(Column column);

    protected abstract string BuildLine(IEnumerable<string> columnValues);

    protected string DoFormat(
      IEnumerable<BenchmarkContextMetrics> contextMetrics,
      RankColumn rankColumn,
      bool allowNesting)
    {
      var columnFactory = new ColumnFactory();

      var columns = columnFactory.Create(contextMetrics, rankColumn, allowNesting).ToArray();

      var output = new StringBuilder();

      output.AppendLine(
        BuildLine(columns.Select(FormatHeader)));

      if (HasRowSeparators)
      {
        output.AppendLine(
          BuildLine(columns.Select(FormatHeaderRowSeparator)));
      }

      var primeColumn = columns.First();
      var numberOfRowGroups = primeColumn.RowGroups.Count();
      for (var rowGroupIndex = 0; rowGroupIndex < numberOfRowGroups; rowGroupIndex++)
      {
        var numberOfRows = primeColumn.RowGroups[rowGroupIndex].Count();
        for (var rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
        {
          output.AppendLine(
            BuildLine(columns.Select(column => FormatValue(column, rowGroupIndex, rowIndex))));
        }

        if (HasRowSeparators)
        {
          output.AppendLine(
          BuildLine(columns.Select(FormatContextRowSeparator)));
        }
      }

      return output.ToString();
    }
  }
}
