using System.Diagnostics;
using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\input.txt");

var stopWatch = new Stopwatch();
stopWatch.Start();

//var sum = Process(input, 1);
var sum = Process(input, 2);

stopWatch.Stop();

Console.WriteLine($"Sum: {sum}");
Console.WriteLine($"Time: {stopWatch.ElapsedMilliseconds}ms"); //12ms | 14ms

int Process(string input, int part)
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
            if (part == 1)
            {
                if (!skip) sum += game;
                skip = false;
            }
            else
            {
                sum += minimumCubes.Values.Aggregate((p, n) => p * n);

                minimumCubes = new Dictionary<string, int>
                {
                    { "red", 0 },
                    { "green", 0 },
                    { "blue", 0 },
                };
            }
            
            game = int.Parse(match.Groups["game"].Value);
            
        }
        
        if (!skip)
        {
            var count = int.Parse(match.Groups["count"].Value);
            var colour = match.Groups["colour"].Value;

            if (part == 1)
            {
                if (count > rules[colour]) skip = true;
            }
            else
            {
                minimumCubes[colour] = Math.Max(minimumCubes[colour], count);
            }
        }
    }

    if (!skip && part == 1)
    {
        sum += game;
    }
    else
    {
        sum += minimumCubes.Values.Aggregate((p, n) => p * n);
    }

    return sum;
}