using System.Diagnostics;
using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\input.txt");

var stopWatch = new Stopwatch();
stopWatch.Start();

//var total = PartOne(input);
var total = PartTwo(input);

stopWatch.Stop();

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Time: {stopWatch.ElapsedMilliseconds}ms"); //12ms | 15ms

static double PartOne(string input)
{
    var cards = new List<Card>();
    var currentCard = new Card(0, new List<int>(), new List<int>());
    
    var matches = CardRegex().Matches(input);
    foreach (Match match in matches)
    {
        var groups = match.Groups;

        if (groups["card"].Success)
        {
            cards.Add(currentCard);

            currentCard = new(int.Parse(match.Groups["card"].Value), new List<int>(), new List<int>());
        }

        if (groups["winners"].Success)
        {
            var numbers = NumberRegex().Matches(groups["winners"].Value);
            currentCard.Winners.AddRange(numbers.Select(number => int.Parse(number.Value)));
        }

        if (groups["owned"].Success)
        {
            var numbers = NumberRegex().Matches(groups["owned"].Value);
            currentCard.Owned.AddRange(numbers.Select(number => int.Parse(number.Value)));
        }
    }

    cards.RemoveAt(0);
    cards.Add(currentCard);

    var total = 0d;
    foreach (var card in cards)
    {
        var intersects = card.Winners.Intersect(card.Owned);
        if (intersects != null && intersects.Any())
        {
            total += Math.Pow(2, intersects.Count() - 1);
        }
    }

    return total;
}

static double PartTwo(string input)
{
    var cards = new List<Card>();
    var currentCard = new Card(0, new List<int>(), new List<int>());
    
    var matches = CardRegex().Matches(input);
    foreach (Match match in matches)
    {
        var groups = match.Groups;

        if (groups["card"].Success)
        {
            cards.Add(currentCard);

            currentCard = new(int.Parse(match.Groups["card"].Value), new List<int>(), new List<int>());
        }

        if (groups["winners"].Success)
        {
            var numbers = NumberRegex().Matches(groups["winners"].Value);
            currentCard.Winners.AddRange(numbers.Select(number => int.Parse(number.Value)));
        }

        if (groups["owned"].Success)
        {
            var numbers = NumberRegex().Matches(groups["owned"].Value);
            currentCard.Owned.AddRange(numbers.Select(number => int.Parse(number.Value)));
        }
    }

    cards.RemoveAt(0);
    cards.Add(currentCard);

    var cardCopies = new Dictionary<int, int>();
    foreach (var card in cards)
    {
        cardCopies.Add(card.Number, 1);
    }

    foreach (var card in cards)
    {
        var intersects = card.Winners.Intersect(card.Owned);
        for (var i = card.Number + 1; i <= card.Number + intersects.Count() && i <= cards.Count; i++)
        {
            cardCopies[i] += cardCopies[card.Number];
        }
    }

    return cardCopies.Sum(cardCopy => cardCopy.Value); 
}

record Card(int Number, List<int> Winners, List<int> Owned);

partial class Program
{
    [GeneratedRegex("(?<card>\\d+): (?<winners>.+)\\|(?<owned>.+)")]
    private static partial Regex CardRegex();
}

partial class Program
{
    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();
}