using System.Diagnostics;
using System.Text.RegularExpressions;

var lines = await File.ReadAllLinesAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\input.txt");

var stopWatch = new Stopwatch();
stopWatch.Start();

//var sum = Process(lines, 1);
var sum = Process(lines, 2);

stopWatch.Stop();

Console.WriteLine($"Sum: {sum}");
Console.WriteLine($"Time: {stopWatch.ElapsedMilliseconds}ms"); //12ms without logging

static int Process(string[] lines, int part)
{
    var rules = new Dictionary<string, int>
    {
        { "red", 12 },
        { "green", 13 },
        { "blue", 14 },
    };

    const string numberRegex = "\\d+";
    const string setRegex = "[^;]+";
    const string groupRegex = "(?<count>\\d+) (?<colour>[a-z]+)";

    var sum = 0;

    var nextLine = false;
    foreach (var line in lines)
    {
        var minimumCubes = new Dictionary<string, int>
        {
            { "red", 0 },
            { "green", 0 },
            { "blue", 0 },
        };

        nextLine = false;

        var gameSplit = line.Split(':');
        var numberMatch = Regex.Match(gameSplit[0], numberRegex);
        var setMatches = Regex.Matches(gameSplit[1], setRegex);

        foreach (Match setMatch in setMatches)
        {
            var groups = Regex.Matches(setMatch.Value, groupRegex);
            foreach (Match groupMatch in groups)
            {
                var count = int.Parse(groupMatch.Groups["count"].Value);
                var colour = groupMatch.Groups["colour"].Value;
                
                if (part == 1)
                {
                    if (count > rules[colour])
                    {
                        nextLine = true; break;
                    }
                }
                else
                {
                    minimumCubes[colour] = Math.Max(minimumCubes[colour], count);
                }
                
            }

            if (nextLine) break;
        }

        if (!nextLine && part == 1)
        {
            sum += int.Parse(numberMatch.Value);
        }
        else
        {
            sum += minimumCubes.Values.Aggregate((p, n) => p * n);
        }
    }

    return sum;
}