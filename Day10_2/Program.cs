using Tools;

var input = File.ReadAllLines("input.txt");

var width = input[0].Length;
var height = input.Length;

var map = new char[width, height];
var distance = new int[width, height];
var mainLoop = new bool[width, height];

var startingPosition = new Point();

for (int x = 0; x < width; x++)
{
    for (int y = 0; y < height; y++)
    {
        distance[x, y] = -1;
        map[x, y] = input[y][x];
        if (map[x, y] == 'S')
        {
            startingPosition = new Point(x, y);
        }
    }
}

var loopLength = 0;

var stack = new Stack<(Point previous, Point current, int steps)>();
stack.Push((startingPosition, startingPosition, 0));
while (stack.Count > 0)
{
    var (previousPosition, currentPosition, steps) = stack.Pop();
    if (map[currentPosition.X, currentPosition.Y] == 'S' &&
        distance[currentPosition.X, currentPosition.Y] != -1)
    {
        loopLength = steps;
        break;
    }

    distance[currentPosition.X, currentPosition.Y] = steps;
    steps++;

    foreach (var direction in Directions.WithoutDiagonals)
    {
        var nextPosition = currentPosition + direction;
        if (nextPosition.X < 0 || nextPosition.X >= width || nextPosition.Y < 0 || nextPosition.Y >= height)
        {
            continue;
        }

        if (nextPosition == previousPosition)
        {
            continue;
        }

        if (map[nextPosition.X, nextPosition.Y] == '.')
        {
            continue;
        }

        if (map[nextPosition.X, nextPosition.Y] != 'S' && distance[nextPosition.X, nextPosition.Y] != -1)
        {
            continue;
        }

        var currentPipe = map[currentPosition.X, currentPosition.Y];
        var nextPipe = map[nextPosition.X, nextPosition.Y];

        bool isValid = false;
        if (direction == new Point(1, 0))
        {
            var nextPipeIsValid = (nextPipe == '7' || nextPipe == 'J' || nextPipe == '-' || nextPipe == 'S');
            var currentPipeIsValid = (currentPipe == 'L' || currentPipe == '-' || currentPipe == 'S' || currentPipe == 'F');
            isValid = nextPipeIsValid && currentPipeIsValid;
        }
        else if (direction == new Point(-1, 0))
        {
            var nextPipeIsValid = (nextPipe == 'F' || nextPipe == 'L' || nextPipe == '-' || nextPipe == 'S');
            var currentPipeIsValid = (currentPipe == 'J' || currentPipe == '7' || currentPipe == 'S' || currentPipe == '-');
            isValid = nextPipeIsValid && currentPipeIsValid;
        }
        else if (direction == new Point(0, 1))
        {
            var nextPipeIsValid = (nextPipe == 'L' || nextPipe == 'J' || nextPipe == '|' || nextPipe == 'S');
            var currentPipeIsValid = (currentPipe == 'F' || currentPipe == '|' || currentPipe == 'S' || currentPipe == '7');
            isValid = nextPipeIsValid && currentPipeIsValid;
        }
        else if (direction == new Point(0, -1))
        {
            var nextPipeIsValid = (nextPipe == 'F' || nextPipe == '7' || nextPipe == '|' || nextPipe == 'S');
            var currentPipeIsValid = (currentPipe == 'L' || currentPipe == 'J' || currentPipe == 'S' || currentPipe == '|');
            isValid = nextPipeIsValid && currentPipeIsValid;
        }

        if (isValid)
        {
            stack.Push((currentPosition, nextPosition, steps));
        }
    }
}

var originalLoopLength = loopLength;
mainLoop[startingPosition.X, startingPosition.Y] = true;
var loopPosition = startingPosition;
while (loopLength > 0)
{
    loopLength--;
    foreach (var direction in Directions.WithoutDiagonals)
    {
        var nextPosition = loopPosition + direction;
        if (nextPosition.X < 0 || nextPosition.X >= width || nextPosition.Y < 0 || nextPosition.Y >= height)
        {
            continue;
        }

        if (distance[nextPosition.X, nextPosition.Y] == loopLength)
        {
            mainLoop[nextPosition.X, nextPosition.Y] = true;
            loopPosition = nextPosition;
            break;
        }
    }
}

long counter = 0;
for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {
        if (!mainLoop[x, y])
        {
            if (InMainLoop(x, y))
            {
                counter++;
                Console.Write('I');
            }
            else
            {
                Console.Write('O');
            }
        }
        else
        {
            Console.Write('X');
        }
    }
    Console.WriteLine();
}

Console.WriteLine(counter);

bool InMainLoop(int x, int y)
{
    var crossings = 0;
    x--;
    for (; x >= 0; x--)
    {
        if (mainLoop[x, y] && (map[x, y] == '|' || map[x, y] == 'J' || map[x, y] == 'L' ))
        {
            crossings++;
        }
    }

    return crossings % 2 != 0;
}