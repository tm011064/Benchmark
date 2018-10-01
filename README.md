# Benchmark
Simple library that allows you to compare the performance of algorithms and output benchmark results as text, markdown or json. The following example shows how to benchmark two algorithms for concatenating strings:

``` c#
public void StringBuilder()
{
  var text = new StringBuilder();

  for (var i = 0; i < 5; i++)
  {
	text.Append(' ');
  }

  ObservableObject.Observe(text.ToString());
}

public void Concatenate()
{
  var text = string.Empty;

  for (var i = 0; i < 5; i++)
  {
	text += ' ';
  }

  ObservableObject.Observe(text);
}

var report = Measure
  .Candidates(
    ("Five Concatenations", Concatenate),
    ("Five String Builder Appends", StringBuilder))
  .WithNumberOfRuns(1000)
  .WithNumberOfDryRuns(10)
  .Go();

Console.Write(report);
```

## Examples

All example code can be found [here](https://github.com/tm011064/Benchmark/tree/master/src/Benchmark.Examples)

#### Lambda Actions

Lambda actions are a quick way to evaluate an algorithm. Just create any number of `void` methods and compare their execution times:

``` c#
var report = Measure
  .Candidates(
    ("First Algo", RunFirstAlgo),
    ("Second Algo", RunSecondAlgo))
  .WithNumberOfRuns(1000)
  .Go();
```

#### Benchmark Candidates With Contexts

Often it is useful to compare algorithms against different data scenarios. An algorithm which performs well against a few data records may become inefficient when run against large datasets. You can define classes implementing the [IBenchmarkContext](https://github.com/tm011064/Benchmark/blob/master/src/Benchmark/IBenchmarkContext.cs) interface to pass parameters to your test methods:

``` c#
var items = Enumerable.Range(0, 1000)
  .Select(_ => new ObservableObject())
  .ToArray();

var report = Measure<LoopContext>
  .Candidates<WhileLoopCandidate, ForLoopCandidate, ForEachLoopCandidate, ForLoopInlineRangeEvaluationCandidate>()
  .WithContexts(
    new LoopContext(items.Take(10).ToArray(), 1),
    new LoopContext(items, 0),
    new LoopContext(items, 1),
    new LoopContext(items, 10))
  .WithNumberOfRuns(100)
  .WithNumberOfDryRuns(10, new LoopContext(items.Take(1).ToArray(), 1))
  .Go();  
```

#### Benchmark Candidates Without Context

This is similar to lambda actions but written in a more formalized way (see LINK for complete example):

``` c#
var report = Measure
  .Candidates<ConcatenateStringsCandidate, StringBuilderCandidate>()
  .WithNumberOfRuns(300)
  .WithNumberOfDryRuns(10)
  .Go();
```

## Output Options

#### ToString(), ToString(RankColumn column)

Good for console output or Visual Studio debugging:

``` text
| Context        | Candidate     | Rank | +/- Median    | Total        | Average    | Median      |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| first Context  | candidate one |    1 |               |    0.0001 ms |  0.001 sec |   1.000 sec |
|                | candidate two |    2 |  + 11900.00 % | 1000.0000 ms | 10.000 sec | 120.000 sec |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| second Context | candidate two |    1 |               |    10.00 sec | 10.000 sec |   0.0001 ms |
|                | candidate one |    2 | + 999900.00 % |    60.00 sec |  0.001 sec |   1.0000 ms |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
| third Context  | candidate one |    1 |               |     00:01:00 |  2.000 sec |   1.000 sec |
|                | candidate two |    2 |    + 100.00 % |     00:02:00 |  1.000 sec |   2.000 sec |
| -------------- | ------------- | ---- | ------------- | ------------ | ---------- | ----------- |
```

### ToMarkdown(), ToMarkdown(RankColumn column)

Good for posting your results on github:

| Context | Candidate | Rank | +/- Average | Total | Average | Median |
| --- | --- | --- | --- | --- | --- | --- |
| first Context | candidate one | 1 |  | 0.0001 ms | 0.001 sec | 1.000 sec |
|  | candidate two | 2 | + 999900.00 % | 1000.0000 ms | 10.000 sec | 120.000 sec |
|   |   |   |   |   |   |   |
| second Context | candidate one | 1 |  | 60.00 sec | 0.001 sec | 1.0000 ms |
|  | candidate two | 2 | + 999900.00 % | 10.00 sec | 10.000 sec | 0.0001 ms |
|   |   |   |   |   |   |   |
| third Context | candidate two | 1 |  | 00:02:00 | 1.000 sec | 2.000 sec |
|  | candidate one | 2 | + 100.00 % | 00:01:00 | 2.000 sec | 1.000 sec |
|   |   |   |   |   |   |   |

#### ToJson()

``` json
{
  "contexts": [{
    "name": "first Context",
    "candidates": [{
      "name": "candidate one",
      "totalMilliseconds": "0.0001",
      "averageMilliseconds": "1",
      "medianMilliseconds": "1000"
    }, {
      "name": "candidate two",
      "totalMilliseconds": "1000",
      "averageMilliseconds": "10000",
      "medianMilliseconds": "120000"
    }]
  }, {
    "name": "second Context",
    "candidates": [{
      "name": "candidate one",
      "totalMilliseconds": "60000",
      "averageMilliseconds": "1",
      "medianMilliseconds": "1"
    }, {
      "name": "candidate two",
      "totalMilliseconds": "10000",
      "averageMilliseconds": "10000",
      "medianMilliseconds": "0.0001"
    }]
  }]
}
```

## Dry Runs
The [Measure](https://github.com/tm011064/Benchmark/blob/master/src/Benchmark/Measure.cs) builder allows you to specify a number of dry runs for your algorithm to counter JIT compilation influencing your results. If you have a good number of runs and are only interested in the median runtime, you won't need this feature. If overall/average execution time of the runs is important, dry runs will remove any distortions caused by JIT compilation.
