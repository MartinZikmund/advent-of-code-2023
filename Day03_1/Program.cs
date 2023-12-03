using Tools;

var input = InputTools.ReadAllLines();

var width = input[0].Length;
var height = input.Length;

var map = new char[width, height];
for (var x = 0; x < width; x++)
{
    for (var y = 0; y < height; y++)
    {
        map[x, y] = input[y][x];
    }
}

var runningTotal = 0;
var currentNumber = 0;
var hasNeighboringSymbol = false;

for (var y = 0; y < height; y++)
{
    void EndCurrentNumber()
    {
        if (currentNumber != 0 && hasNeighboringSymbol)
        {
            runningTotal += currentNumber;
        }
        currentNumber = 0;
        hasNeighboringSymbol = false;
    }

    for (var x = 0; x < height; x++)
    {
        var character = map[x, y];
        // check if we are reading a number
        if (char.IsDigit(character))
        {
            var value = character - '0';
            currentNumber = currentNumber * 10 + value;
            foreach (var direction in Directions.WithDiagonals)
            {
                var neigbhorX = x + direction.X;
                var neigbhorY = y + direction.Y;
                if (neigbhorX < 0 || neigbhorX >= width || neigbhorY < 0 || neigbhorY >= height)
                {
                    continue;
                }

                var neighborCharacter = map[neigbhorX, neigbhorY];
                if (!char.IsDigit(neighborCharacter) && neighborCharacter != '.')
                {
                    hasNeighboringSymbol = true;
                }
            }
        }
        else 
        {
            EndCurrentNumber();
        }
    }

    EndCurrentNumber();
}

Console.WriteLine(runningTotal);