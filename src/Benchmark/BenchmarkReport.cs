using Benchmark.Formatters;
using System.Collections.Generic;

namespace Benchmark
{
  public class BenchmarkReport
  {
    public BenchmarkReport(BenchmarkResult[] results)
    {
      Results = results;
    }

    public IEnumerable<BenchmarkResult> Results { get; }

    public override string ToString()
    {
      return ToString(RankColumn.Median);
    }

    public string ToString(RankColumn order = RankColumn.Median)
    {
      return new DefaultReportFormatter().Format(Results, order);
    }

    public string ToMarkdown(RankColumn order = RankColumn.Median)
    {
      return new MarkupReportFormatter().Format(Results, order);
    }

    public string ToJson()
    {
      return new JsonFormatter().Format(Results);
    }
  }
}
