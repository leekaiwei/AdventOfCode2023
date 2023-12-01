using System.Diagnostics;
using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\input.txt");

var stopWatch = new Stopwatch();
stopWatch.Start();

var stringNumbersMapping = new Dictionary<string, string>
{
    { "one", "1" },
    { "two", "2" },
    { "three", "3" },
    { "four", "4" },
    { "five", "5" },
    { "six", "6" },
    { "seven", "7" },
    { "eight", "8" },
    { "nine", "9" }
};

var stringNumbersRegex = string.Join("|", stringNumbersMapping.Keys);

var regex = "(?<first>\\d{1}|" + stringNumbersRegex + ").*(?<last>\\d|" + stringNumbersRegex + ")|(?<single>\\d|" + stringNumbersRegex + ")";

var matches = Regex.Matches(input, regex);

var sum = 0;
for (var i = 0; i < matches.Count; i++)
{
    var groups = matches[i].Groups;
    var number = groups["single"].Success
        ? NumberValue(groups["single"].Value) + NumberValue(groups["single"].Value)
        : NumberValue(groups["first"].Value) + NumberValue(groups["last"].Value);

    Console.WriteLine(number);
    sum += int.Parse(number);
}

stopWatch.Stop();

Console.WriteLine();
Console.WriteLine($"Sum: {sum}");
Console.WriteLine($"Time: {stopWatch.ElapsedMilliseconds}ms"); //13ms without logging

string NumberValue(string input)
{
    if (!stringNumbersMapping!.TryGetValue(input, out var number))
    {
        return input;
    }

    else return number;
}