using Tools;

var input = File.ReadAllLines("input.txt");

var width = input[0].Length;
var height = input.Length;

var map = new char[width, height];
var visited = new bool[width, height];

var startingPosition = new Point();

for (int x = 0; x < width; x++)
{
    for (int y = 0; y < height; y++)
    {
        map[x, y] = input[y][x];
        if (map[x, y] == 'S')
        {
            startingPosition = new Point(x, y);
        }
    }
}

var stack = new Stack<(Point previous, Point current, int steps)>();
stack.Push((startingPosition, startingPosition, 0));
while (stack.Count > 0)
{
    var (previousPosition, currentPosition, steps) = stack.Pop();
    if (map[currentPosition.X, currentPosition.Y] == 'S' &&
        visited[currentPosition.X, currentPosition.Y])
    {
        Console.WriteLine(steps / 2);
        return;
    }

    visited[currentPosition.X, currentPosition.Y] = true;
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

        if (map[nextPosition.X, nextPosition.Y] != 'S' && visited[nextPosition.X, nextPosition.Y])
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