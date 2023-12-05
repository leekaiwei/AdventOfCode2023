using System.Diagnostics;
using System.Text.RegularExpressions;

var lines = await File.ReadAllLinesAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\input.txt");

var stopWatch = new Stopwatch();
stopWatch.Start();

var linkedList = new LinkedList<int>();

foreach (var line in lines)
{
    if (line.StartsWith("seeds:"))
    {
        var seeds = NumberRegex().Matches(line);
    }
}

partial class Program
{
    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();
}