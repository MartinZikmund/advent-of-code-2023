var input = File.ReadAllLines("input.txt");

long runningTotal = 0;

foreach (var line in input)
{
    var sequence = line.Split(' ').Select(int.Parse).ToArray();
    runningTotal += GetExtrapolatedValue(sequence);
}

Console.WriteLine(runningTotal);

long GetExtrapolatedValue(int[] sequence)
{
    int[,] chart = new int[sequence.Length + 1, sequence.Length + 1];

    var allZeroes = false;
    for (int i = 0; i < sequence.Length; i++)
    {
        chart[0, i] = sequence[i];
    }

    int chartRow = 0;
    while (!allZeroes)
    {
        allZeroes = true;
        for (int i = 0; i < sequence.Length - chartRow - 1; i++)
        {
            var leftValue = chart[chartRow, i];
            var rightValue = chart[chartRow, i + 1];
            var diff = rightValue - leftValue;
            chart[chartRow + 1, i] = diff;
            if (diff != 0)
            {
                allZeroes = false;
            }
        }

        chartRow++;
    }

    int currentValue = 0;
    for (int row = chartRow; row > 0; row--)
    {
        var difference = currentValue;
        var valueAbove = chart[row - 1, 0];
        currentValue = valueAbove - difference;
    }

    return currentValue;
}