using Tools;

var input = InputTools.ReadAllLines();
int[] cardCount = new int[input.Length];
for (int i = 0; i < cardCount.Length; i++)
{
    cardCount[i] = 1;
}

for (int cardId = 0; cardId < input.Length; cardId++)
{
    string? line = input[cardId];
    var parts = line.Split(':');
    var numbers = parts[1].Split('|');
    var pickedNumbers = numbers[0]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .ToArray();
    var ourNumbers = numbers[1]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .ToArray();

    var matchCount = pickedNumbers.Intersect(ourNumbers).Count();

    for (int i = 0; i < matchCount; i++)
    {
        cardCount[cardId + 1 + i] += cardCount[cardId];
    }
}

Console.WriteLine(cardCount.Sum());