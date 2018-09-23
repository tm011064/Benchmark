using System;
using System.Linq;

namespace Benchmark.Formatters
{
  internal class Column
  {
    public Column(
      string header,
      string[][] rowGroups,
      HorizontalAlignment horizontalAlignment)
    {
      Header = header;
      RowGroups = rowGroups;
      HorizontalAlignment = horizontalAlignment;

      Width = Math.Max(
        header.Count(),
        rowGroups.SelectMany(x => x).Max(x => x.Count()));
    }

    public string Header { get; }

    public string[][] RowGroups { get; }

    public int Width { get; }

    public HorizontalAlignment HorizontalAlignment { get; }
  }
}
