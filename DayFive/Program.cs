using System.Diagnostics;
using System.Text.RegularExpressions;

var lines = await File.ReadAllLinesAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\input.txt");

var stopWatch = new Stopwatch();
stopWatch.Start();

var mappingDefinitions = new List<MappingDefinition>();
var mappings = new Dictionary<int, List<int>>();
var mode = string.Empty;

foreach (var line in lines)
{
    if (line.StartsWith("seeds:"))
    {
        var seeds = NumberRegex().Matches(line);
        foreach (Match seed in seeds)
        {
            mappings.Add(int.Parse(seed.Value), new List<int>());
        }
    }
    else if (line.StartsWith("seed-")) mode = "se";
    else if (line.StartsWith("so")) mode = "so";
    else if (line.StartsWith("f")) mode = "f";
    else if (line.StartsWith("w")) mode = "w";
}

record MappingDefinition(int Source, int Destination, int Range);

partial class Program
{
    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();
}

partial class Program
{
    [GeneratedRegex("[a-z]+")]
    private static partial Regex WordRegex();
}