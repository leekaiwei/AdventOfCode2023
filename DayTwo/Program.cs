using System.Diagnostics;
using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\input.txt");

var stopWatch = new Stopwatch();
stopWatch.Start();

var sum1 = Part1(input);

Console.WriteLine($"Sum 1: {sum1}");
Console.WriteLine($"Time: {stopWatch.ElapsedMilliseconds}ms");

var sum2 = Part2(input);

stopWatch.Stop();

Console.WriteLine($"Sum 2: {sum2}");
Console.WriteLine($"Time: {stopWatch.ElapsedMilliseconds}ms");

int Part1(string input)
{
    var rules = new Dictionary<string, int>
    {
        { "red", 12 },
        { "green", 13 },
        { "blue", 14 },
    };

    var game = 0;
    var sum = 0;
    var skip = false;

    var matches = Regex.Matches(input, "(Game (?<game>\\d+):)? (?<set>((?<count>\\d+) (?<colour>([a-z]+))))");
    foreach (Match match in matches)
    {
        if (match.Groups["game"].Success)
        {
            if (!skip) sum += game;
            
            game = int.Parse(match.Groups["game"].Value);
            skip = false;
        }
        
        if (!skip)
        {
            var count = int.Parse(match.Groups["count"].Value);
            var colour = match.Groups["colour"].Value;

            if (count > rules[colour]) skip = true;
        }
    }

    if (!skip) sum += game;

    return sum;
}

int Part2(string input)
{
    var sum = 0;

    var minimumCubes = new Dictionary<string, int>
    {
        { "red", 0 },
        { "green", 0 },
        { "blue", 0 },
    };

    var matches = Regex.Matches(input, "(Game (?<game>\\d+):)? (?<set>((?<count>\\d+) (?<colour>([a-z]+))))");
    foreach (Match match in matches)
    {
        if (match.Groups["game"].Success)
        {
            sum += minimumCubes.Values.Aggregate((p, n) => p * n);

            minimumCubes = new Dictionary<string, int>
            {
                { "red", 0 },
                { "green", 0 },
                { "blue", 0 },
            };
        }

       
        var count = int.Parse(match.Groups["count"].Value);
        var colour = match.Groups["colour"].Value;

        minimumCubes[colour] = Math.Max(minimumCubes[colour], count);
    }

    sum += minimumCubes.Values.Aggregate((p, n) => p * n);

    return sum;
}