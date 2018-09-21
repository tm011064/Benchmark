using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benchmark.Writers
{
  public class BenchmarkResultConsoleWriter
  {
    private static readonly string LineBreak = Environment.NewLine;

    private readonly string columnSeparator;

    private readonly char headerRowSeparater;

    private readonly char testCaseRowSeparater;

    private readonly RankMode rankMode;

    private readonly (ICandidateTestCase TestCase, CandidateMetrics[] OrderedMetrics)[] results;

    public BenchmarkResultConsoleWriter(
      IEnumerable<BenchmarkResult> results,
      RankMode rankMode = RankMode.Median,
      string columnSeparator = " | ",
      char headerRowSeparater = '-',
      char testCaseRowSeparater = '-')
    {
      this.columnSeparator = columnSeparator;
      this.headerRowSeparater = headerRowSeparater;
      this.testCaseRowSeparater = testCaseRowSeparater;
      this.rankMode = rankMode;

      var sortKey = new Func<CandidateMetrics, object>(metrics => GetElapsed(metrics, rankMode));
      this.results = results
        .Select(result => (result.TestCase, result.Metrics.OrderBy(sortKey).ToArray()))
        .ToArray();
    }

    public string Write()
    {
      var columns = new[]
      {
        CreateColumnRows(string.Empty, BuildTestCaseRows().ToArray()).ToArray(),
        CreateColumnRows("Candidate", BuildCandidateRows().ToArray()).ToArray(),
        CreateColumnRows("Rank", BuildRankRows().ToArray(), false).ToArray(),
        CreateColumnRows(CreatePlusMinusPercentageHeader(), BuildPlusMinusPercentageRows().ToArray(), false).ToArray(),
        CreateColumnRows("Total", BuildElapsedColumn(RankMode.Total).ToArray(), false).ToArray(),
        CreateColumnRows("Average", BuildElapsedColumn(RankMode.Average).ToArray(), false).ToArray(),
        CreateColumnRows("Median", BuildElapsedColumn(RankMode.Median).ToArray(), false).ToArray()
      };

      var output = new StringBuilder();

      var numberOfRows = columns.First().Count();
      for (var rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
      {
        var line = string.Join(columnSeparator, columns.Select(column => column[rowIndex]));

        output.AppendLine(string.Concat(columnSeparator, line, columnSeparator).Trim());
      }

      return output.ToString();
    }

    private IEnumerable<string> BuildElapsedColumn(RankMode mode)
    {
      foreach (var orderedMetrics in results.Select(x => x.OrderedMetrics))
      {
        var durations = orderedMetrics.Select(metrics => GetElapsed(metrics, mode)).ToArray();

        var format = CreateTimeSpanFormatter(durations.Min());

        foreach (var metric in orderedMetrics)
        {
          var elapsed = GetElapsed(metric, mode);

          yield return format(elapsed);
        }

        yield return LineBreak;
      }
    }

    private Func<TimeSpan, string> CreateTimeSpanFormatter(TimeSpan duration)
    {
      if (duration >= TimeSpan.FromMinutes(1))
      {
        return timeSpan => timeSpan.ToString("c");
      }

      if (duration >= TimeSpan.FromSeconds(10))
      {
        return timeSpan => timeSpan.TotalSeconds.ToString("0.00 sec");
      }

      if (duration >= TimeSpan.FromMilliseconds(1))
      {
        return timeSpan => timeSpan.TotalSeconds.ToString("0.0000 sec");
      }

      return timeSpan => timeSpan.TotalMilliseconds.ToString("0.0000 ms");
    }

    private string CreatePlusMinusPercentageHeader()
    {
      switch (rankMode)
      {
        case RankMode.Total:
          return "+/- Total";

        case RankMode.Median:
          return "+/- Median";

        case RankMode.Average:
          return "+/- Average";

        default:
          throw new NotSupportedException(rankMode.ToString());
      }
    }

    private TimeSpan GetElapsed(CandidateMetrics metrics, RankMode mode)
    {
      switch (mode)
      {
        case RankMode.Total:
          return metrics.TotalElapsed;

        case RankMode.Median:
          return metrics.MedianElapsed;

        case RankMode.Average:
          return metrics.AverageElapsed;

        default:
          throw new NotSupportedException(rankMode.ToString());
      }
    }

    private IEnumerable<string> BuildPlusMinusPercentageRows()
    {
      foreach (var orderedMetrics in results.Select(x => x.OrderedMetrics))
      {
        var prime = orderedMetrics.First();

        yield return string.Empty;

        foreach (var metric in orderedMetrics.Skip(1))
        {
          var percentage = ((decimal)GetElapsed(metric, rankMode).Ticks / GetElapsed(prime, rankMode).Ticks) - 1;
          yield return percentage.ToString("+ 0.00 %");
        }

        yield return LineBreak;
      }
    }

    private IEnumerable<string> BuildRankRows()
    {
      foreach (var orderedMetrics in results.Select(x => x.OrderedMetrics))
      {
        var rank = 1;

        foreach (var metric in orderedMetrics)
        {
          yield return rank.ToString();
          rank++;
        }

        yield return LineBreak;
      }
    }

    private IEnumerable<string> BuildCandidateRows()
    {
      foreach (var orderedMetrics in results.Select(x => x.OrderedMetrics))
      {
        foreach (var metric in orderedMetrics)
        {
          yield return metric.CandidateName;
        }

        yield return LineBreak;
      }
    }

    private IEnumerable<string> BuildTestCaseRows()
    {
      foreach (var (testCase, orderedMetrics) in results)
      {
        yield return testCase.Description;

        var numberOfEmptyLines = orderedMetrics.Count() - 1;
        for (var i = 0; i < numberOfEmptyLines; i++)
        {
          yield return string.Empty;
        }

        yield return LineBreak;
      }
    }

    private IEnumerable<string> CreateColumnRows(string header, string[] values, bool leftAlign = true)
    {
      var width = values.Concat(new[] { header }).Max(x => x.Length);

      var rowSeparator = new string(Enumerable.Range(0, width).Select(_ => testCaseRowSeparater).ToArray());

      yield return header.PadRight(width);
      yield return new string(Enumerable.Range(0, width).Select(_ => headerRowSeparater).ToArray());

      var rows = values
        .Select(value => FormatColumnValue(value, width, rowSeparator, leftAlign))
        .ToArray();

      foreach (var row in rows)
      {
        yield return row;
      }
    }

    private string FormatColumnValue(string value, int maxLength, string rowSeparator, bool leftAlign = true)
    {
      if (string.Equals(value, LineBreak, StringComparison.Ordinal))
      {
        return rowSeparator;
      }

      return leftAlign
        ? value.PadRight(maxLength)
        : value.PadLeft(maxLength);
    }
  }
}
