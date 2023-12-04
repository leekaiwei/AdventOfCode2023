using System.Diagnostics;
using System.Text.RegularExpressions;

var lines = await File.ReadAllLinesAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\input.txt");

var stopWatch = new Stopwatch();

stopWatch.Start();

//var sum = PartOne(lines);
var sum = PartTwo(lines);

stopWatch.Stop();

Console.WriteLine($"Sum: {sum}");
Console.WriteLine($"Time: {stopWatch.ElapsedMilliseconds}ms"); //28ms | 20ms

static int PartOne(string[] lines)
{
    var partNumberMappings = new List<PartNumber>();
    var symbolPositions = new Dictionary<int[], char>();
    var yIndex = 0;

    // parsing
    foreach (var line in lines)
    {
        var matches = Regex().Matches(line);
        foreach (Match match in matches)
        {
            if (match.Groups["number"].Success)
            {
                var numberMatch = match.Groups["number"];
                var number = numberMatch.Value;
                var x = numberMatch.Index;

                partNumberMappings.Add(new(int.Parse(number), x - 1, x + number.Length, yIndex - 1, yIndex + 1));
            }
            else if (match.Groups["symbol"].Success)
            {
                var symbolMatch = match.Groups["symbol"];
                var symbol = symbolMatch.Value;
                var x = symbolMatch.Index;

                int[] position = { x, yIndex };
                symbolPositions.Add(position, symbol[0]);
            }
        }

        yIndex += 1;
    }

    var partNumbersToCheck = partNumberMappings.ToArray();
    var sum = 0;
    foreach (var symbolPosition in symbolPositions)
    {
        var x = symbolPosition.Key[0];
        var y = symbolPosition.Key[1];

        var index = 0;
        var added = 0;
        var nextPartNumbersToCheck = new List<PartNumber>();

        do
        {
            var partNumberMapping = partNumbersToCheck[index];

            if (x >= partNumberMapping.MinX && x <= partNumberMapping.MaxX && y >= partNumberMapping.MinY && y <= partNumberMapping.MaxY)
            {
                sum += partNumberMapping.Number;
                added += 1;

                //Console.WriteLine(partNumberMapping.Number);
            }

            // new collection of remaining numbers to check
            // no need to check previously added numbers
            nextPartNumbersToCheck.Add(partNumberMapping);

            index += 1;
        }
        // max 6 parts can surround a symbol
        while (added < 6 && index < partNumberMappings.Count);

        partNumbersToCheck = nextPartNumbersToCheck.ToArray();
    }

    return sum;
}

static int PartTwo(string[] lines)
{
    var partNumberMappings = new List<PartNumber>();
    var gearPositions = new List<int[]>();
    var yIndex = 0;

    // parsing
    foreach (var line in lines)
    {
        var matches = Regex().Matches(line);
        foreach (Match match in matches)
        {
            if (match.Groups["number"].Success)
            {
                var numberMatch = match.Groups["number"];
                var number = numberMatch.Value;
                var x = numberMatch.Index;

                partNumberMappings.Add(new(int.Parse(number), x - 1, x + number.Length, yIndex - 1, yIndex + 1));
            }
            else if (match.Groups["symbol"].Success)
            {
                var symbolMatch = match.Groups["symbol"];
                var symbol = symbolMatch.Value;

                if (symbol == "*")
                {
                    var x = symbolMatch.Index;

                    int[] position = { x, yIndex };
                    gearPositions.Add(position);
                }
            }
        }

        yIndex += 1;
    }

    var sum = 0;
    foreach (var gearPosition in gearPositions)
    {
        var x = gearPosition[0];
        var y = gearPosition[1];
        var adjacentPartNumbers = partNumberMappings.Where(partNumberMapping =>
            x >= partNumberMapping.MinX && x <= partNumberMapping.MaxX && y >= partNumberMapping.MinY && y <= partNumberMapping.MaxY);

        if (adjacentPartNumbers.Count() == 2)
        {
            sum += adjacentPartNumbers.First().Number * adjacentPartNumbers.Last().Number;
        }
    }

    return sum;
}
record PartNumber(int Number, int MinX, int MaxX, int MinY, int MaxY);

partial class Program
{
    [GeneratedRegex("(?<number>\\d+)|(?<symbol>[^.\\d]+)")]
    private static partial Regex Regex();
}