using System.Diagnostics;
using System.Text.RegularExpressions;

var lines = await File.ReadAllLinesAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\input.txt");

var stopWatch = new Stopwatch();
stopWatch.Start();

//var result = PartOne(lines);
var result = PartTwo(lines);

stopWatch.Stop();

Console.WriteLine($"Result: {result}");
Console.WriteLine($"Time: {stopWatch.ElapsedMilliseconds}ms"); //27ms

long PartOne(string[] lines)
{
    var mappingDefinitions = new Dictionary<string, List<MappingDefinition>>();
    var mappings = new Dictionary<long, List<long>>();
    var mode = string.Empty;

    foreach (var line in lines)
    {
        if (line.StartsWith("seeds:"))
        {
            var seeds = NumberRegex().Matches(line);
            foreach (Match seed in seeds)
            {
                mappings.Add(long.Parse(seed.Value), new() { long.Parse(seed.Value) });
            }
        }
        else
        {
            var mappingMatch = MapRegex().Match(line);

            if (string.IsNullOrEmpty(line)) continue;

            if (mappingMatch.Success)
            {
                mode = mappingMatch.Value;
                mappingDefinitions.Add(mode, new());
            }
            else
            {
                var numberMatches = NumberRegex().Matches(line).Cast<Match>().ToArray();
                mappingDefinitions[mode].Add(new(long.Parse(numberMatches[1].Value), long.Parse(numberMatches[0].Value), long.Parse(numberMatches[2].Value)));
            }
        }
    }

    foreach (var seed in mappings.Keys)
    {
        var seedMapping = mappings[seed];
        foreach (var mappingDefinition in mappingDefinitions)
        {
            var source = seedMapping.Last();

            var definitions = mappingDefinition.Value;
            var found = false;
            foreach (var definition in definitions)
            {
                if (source >= definition.Source && source <= definition.Source + definition.Range)
                {
                    var difference = source - definition.Source;
                    seedMapping.Add(definition.Destination + difference);

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                seedMapping.Add(source);
            }
        }
    }

    return mappings.Values.Select(m => m.Last()).Min();
}

long PartTwo(string[] lines)
{
    var mappingDefinitions = new Dictionary<long, LinkedList<long>>();
    var seedsToCheck = new List<long>();

    var mappings = new Dictionary<long, List<long>>();
    var mode = string.Empty;

    var times = 0;
    foreach (var line in lines)
    {
        if (line.StartsWith("seeds:"))
        {
            var seeds = SeedRegex().Matches(line);
            foreach (Match seed in seeds)
            {
                var source = long.Parse(seed.Groups["source"].Value);
                var range = long.Parse(seed.Groups["range"].Value);

                for (var i = source; i <= source + range; i++)
                {
                    seedsToCheck.Add(i);
                }
                
            }
        }
        else
        {
            var mappingMatch = MapRegex().Match(line);

            if (string.IsNullOrEmpty(line)) continue;

            if (mappingMatch.Success)
            {
                times += 1;
            }
            else
            {
                var numberMatches = NumberRegex().Matches(line).Cast<Match>().ToArray();
                var source = long.Parse(numberMatches[1].Value);
                var destination = long.Parse(numberMatches[0].Value);
                var range = long.Parse(numberMatches[2].Value);

                //var newList = new LinkedList<long>();
                //newList.AddLast(source);
                //newList.AddLast(destination);
                //mappingDefinitions.Add(source, newList);

                var count = 0;
                for (var i = source; i < source + range; i++)
                {
                    var newList = new LinkedList<long>();
                    newList.AddLast(i);
                    newList.AddLast(destination + count);
                    mappingDefinitions.Add(i, newList);

                    count++;
                }
            }
        }
    }

    //foreach (var seed in mappings.Keys)
    //{
    //    var seedMapping = mappings[seed];
    //    foreach (var mappingDefinition in mappingDefinitions)
    //    {
    //        var source = seedMapping.Last();

    //        var definitions = mappingDefinition.Value;
    //        var found = false;
    //        foreach (var definition in definitions)
    //        {
    //            if (source >= definition.Source && source <= definition.Source + definition.Range)
    //            {
    //                var difference = source - definition.Source;
    //                seedMapping.Add(definition.Destination + difference);

    //                found = true;
    //                break;
    //            }
    //        }

    //        if (!found)
    //        {
    //            seedMapping.Add(source);
    //        }
    //    }
    //}

    return mappings.Values.Select(m => m.Last()).Min();
}

record MappingDefinition(long Source, long Destination, long Range);

partial class Program
{
    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();
}

partial class Program
{
    [GeneratedRegex(".+-?map")]
    private static partial Regex MapRegex();
}

partial class Program
{
    [GeneratedRegex("(?<source>\\d+) (?<range>\\d+)")]
    private static partial Regex SeedRegex();
}