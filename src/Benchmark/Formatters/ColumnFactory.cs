using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchmark.Formatters
{
  internal class ColumnFactory
  {
    public IEnumerable<Column> Create(
      IEnumerable<BenchmarkResult> results,
      RankColumn rankColumn,
      bool allowNesting)
    {
      var metricsByContext = CreateOrderedMetrics(results, rankColumn).ToArray();

      yield return new Column(
        "Context",
        allowNesting
          ? BuildNestedBenchmarkTestContextRows(metricsByContext).ToArray()
          : BuildBenchmarkTestContextRows(metricsByContext).ToArray(),
        HorizontalAlignment.Left);

      yield return new Column(
        "Candidate",
        BuildCandidateRows(metricsByContext.Select(x => x.OrderedMetrics)).ToArray(),
        HorizontalAlignment.Left);

      yield return new Column(
        "Rank",
        BuildRankRows(metricsByContext.Select(x => x.OrderedMetrics)).ToArray(),
        HorizontalAlignment.Right);

      yield return new Column(
        CreatePlusMinusPercentageHeader(rankColumn),
        BuildPlusMinusPercentageRows(metricsByContext.Select(x => x.OrderedMetrics), rankColumn).ToArray(),
        HorizontalAlignment.Right);

      yield return new Column(
        "Total",
        BuildElapsedColumn(metricsByContext.Select(x => x.OrderedMetrics), RankColumn.Total).ToArray(),
        HorizontalAlignment.Right);

      yield return new Column(
        "Average",
        BuildElapsedColumn(metricsByContext.Select(x => x.OrderedMetrics), RankColumn.Average).ToArray(),
        HorizontalAlignment.Right);

      yield return new Column(
        "Median",
        BuildElapsedColumn(metricsByContext.Select(x => x.OrderedMetrics), RankColumn.Median).ToArray(),
        HorizontalAlignment.Right);
    }

    private IEnumerable<(IBenchmarkContext Context, CandidateMetrics[] OrderedMetrics)> CreateOrderedMetrics(
      IEnumerable<BenchmarkResult> results,
      RankColumn rankOrder)
    {
      var sortKey = new Func<CandidateMetrics, object>(metrics => GetElapsed(metrics, rankOrder));

      return results
        .Select(result => (result.Context, result.CandidateMetrics.OrderBy(sortKey).ToArray()));
    }

    private IEnumerable<string[]> BuildElapsedColumn(
      IEnumerable<CandidateMetrics[]> orderedMetricGroups,
      RankColumn mode)
    {
      return orderedMetricGroups.Select(
        orderedMetrics =>
        {
          var durations = orderedMetrics.Select(metrics => GetElapsed(metrics, mode)).ToArray();

          var format = CreateTimeSpanFormatter(durations.Min());

          return orderedMetrics
            .Select(metrics => format(GetElapsed(metrics, mode)))
            .ToArray();
        });
    }

    private IEnumerable<string[]> BuildPlusMinusPercentageRows(
      IEnumerable<CandidateMetrics[]> orderedMetricGroups,
      RankColumn rankColumn)
    {
      return orderedMetricGroups.Select(
        orderedMetrics =>
        {
          var prime = orderedMetrics.First();

          return new[] { string.Empty }
            .Concat(
              orderedMetrics.Skip(1)
                .Select(metric =>
                {
                  var primeElapsedTicks = GetElapsed(prime, rankColumn).Ticks;

                  if (primeElapsedTicks == 0)
                  {
                    return "NaN";
                  }

                  var percentage = ((decimal)GetElapsed(metric, rankColumn).Ticks / primeElapsedTicks) - 1;

                  return percentage.ToString("+ 0.00 %");
                }))
            .ToArray();
        });
    }

    private IEnumerable<string[]> BuildRankRows(
      IEnumerable<CandidateMetrics[]> orderedMetricGroups)
    {
      return orderedMetricGroups
        .Select(orderedMetrics => Enumerable
          .Range(1, orderedMetrics.Count())
          .Select(rank => rank.ToString())
          .ToArray());
    }

    private IEnumerable<string[]> BuildCandidateRows(
      IEnumerable<CandidateMetrics[]> orderedMetricGroups)
    {
      return orderedMetricGroups
        .Select(orderedMetrics => orderedMetrics
          .Select(metrics => metrics.CandidateName)
          .ToArray());
    }

    private IEnumerable<string[]> BuildBenchmarkTestContextRows(
      (IBenchmarkContext Context, CandidateMetrics[] OrderedMetrics)[] orderedMetricsByContext)
    {
      return orderedMetricsByContext
        .Select(x => Enumerable
          .Range(0, x.OrderedMetrics.Count())
          .Select(_ => x.Context.Description)
          .ToArray());
    }

    private IEnumerable<string[]> BuildNestedBenchmarkTestContextRows(
      (IBenchmarkContext Context, CandidateMetrics[] OrderedMetrics)[] orderedMetricsByContext)
    {
      return orderedMetricsByContext
        .Select(x => new[] { x.Context.Description }
          .Concat(
            Enumerable.Range(0, x.OrderedMetrics.Count() - 1).Select(_ => string.Empty))
          .ToArray());
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
        return timeSpan => timeSpan.TotalSeconds.ToString("0.000 sec");
      }

      return timeSpan => timeSpan.TotalMilliseconds.ToString("0.0000 ms");
    }

    private string CreatePlusMinusPercentageHeader(RankColumn rankColumn)
    {
      switch (rankColumn)
      {
        case RankColumn.Total:
          return "+/- Total";

        case RankColumn.Median:
          return "+/- Median";

        case RankColumn.Average:
          return "+/- Average";

        default:
          throw new NotSupportedException(rankColumn.ToString());
      }
    }

    private TimeSpan GetElapsed(CandidateMetrics metrics, RankColumn rankColumn)
    {
      switch (rankColumn)
      {
        case RankColumn.Total:
          return metrics.TotalElapsed;

        case RankColumn.Median:
          return metrics.MedianElapsed;

        case RankColumn.Average:
          return metrics.AverageElapsed;

        default:
          throw new NotSupportedException(rankColumn.ToString());
      }
    }
  }
}
