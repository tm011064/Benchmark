using Benchmark.Formatters;
using System.Collections.Generic;

namespace Benchmark
{
  public class BenchmarkReport
  {
    public BenchmarkReport(BenchmarkContextMetrics[] metrics)
    {
      Metrics = metrics;
    }

    public IEnumerable<BenchmarkContextMetrics> Metrics { get; }

    public override string ToString()
    {
      return ToString(RankColumn.Median);
    }

    public string ToString(RankColumn order = RankColumn.Median)
    {
      return new DefaultReportFormatter().Format(Metrics, order);
    }

    public string ToMarkupTable(RankColumn order = RankColumn.Median)
    {
      return new MarkupReportFormatter().Format(Metrics, order);
    }

    public string ToCsv(RankColumn order = RankColumn.Median)
    {
      return new CsvReportFormatter().Format(Metrics, order);
    }
  }
}
