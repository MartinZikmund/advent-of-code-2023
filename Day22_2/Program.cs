using Day22_1;

var input = File.ReadAllLines("input.txt");

var bricks = new List<Brick>();

var id = 0;
foreach (var line in input)
{
    var parts = line.Split(new[] { ',', '~' }, StringSplitOptions.RemoveEmptyEntries);
    var x1 = int.Parse(parts[0]);
    var y1 = int.Parse(parts[1]);
    var z1 = int.Parse(parts[2]);
    var x2 = int.Parse(parts[3]);
    var y2 = int.Parse(parts[4]);
    var z2 = int.Parse(parts[5]);
    var brick = new Brick(id, x1, y1, z1, x2, y2, z2);
    bricks.Add(brick);
    id++;
}

var sortedBricks = bricks.OrderBy(b => b.MinZ).ToList();
for (var currentBrickId = 0; currentBrickId < sortedBricks.Count; currentBrickId++)
{
    var currentBrick = sortedBricks[currentBrickId];
    var currentMinZ = 1;
    for (int brickToCheckId = 0; brickToCheckId < currentBrickId; brickToCheckId++)
    {
        var brickToCheck = sortedBricks[brickToCheckId];
        if (brickToCheck.Intersects(currentBrick))
        {
            currentMinZ = Math.Max(currentMinZ, brickToCheck.MaxZ + 1);
        }
    }

    var deltaZ = currentMinZ - currentBrick.MinZ;

    currentBrick.Z1 += deltaZ;
    currentBrick.Z2 += deltaZ;
}

var supportedBy = new Dictionary<Brick, HashSet<Brick>>();
var supports = new Dictionary<Brick, HashSet<Brick>>();
foreach (var brick in bricks)
{
    supportedBy.Add(brick, new());
    supports.Add(brick, new());
}
sortedBricks = bricks.OrderBy(b => b.MinZ).ToList();

for (var currentBrickId = 0; currentBrickId < sortedBricks.Count; currentBrickId++)
{
    var currentBrick = sortedBricks[currentBrickId];
    for (int brickToCheckId = 0; brickToCheckId < currentBrickId; brickToCheckId++)
    {
        var brickToCheck = sortedBricks[brickToCheckId];
        if (brickToCheck.Intersects(currentBrick) &&
            brickToCheck.MaxZ + 1 == currentBrick.MinZ)
        {
            supportedBy[currentBrick].Add(brickToCheck);
            supports[brickToCheck].Add(currentBrick);
        }
    }
}

var disintegrationTotal = 0;
for (var currentBrickId = 0; currentBrickId < bricks.Count; currentBrickId++)
{
    var currentBrick = bricks[currentBrickId];
    var isSafe = true;
    foreach (var supportedBrick in supports[currentBrick])
    {
        if (supportedBy[supportedBrick].Count == 1)
        {
            isSafe = false;
        }
    }

    if (!isSafe)
    {
        var wouldFallDown = new HashSet<Brick>();
        wouldFallDown.Add(currentBrick);
        CheckFellDown(currentBrick);
        void CheckFellDown(Brick brick)
        {
            foreach (var supportedBrick in supports[brick])
            {
                if (!wouldFallDown.Contains(supportedBrick) &&
                    supportedBy[supportedBrick].All(b => wouldFallDown.Contains(b)))
                {
                    wouldFallDown.Add(supportedBrick);
                    CheckFellDown(supportedBrick);
                }
            }
        }

        disintegrationTotal += wouldFallDown.Count - 1;
    }
}

for (var currentBrickId = 0; currentBrickId < bricks.Count; currentBrickId++)
{
    var currentBrick = bricks[currentBrickId];
    Console.WriteLine($"{currentBrick.Id} is supporting {string.Join(", ", supports[currentBrick].Select(b => b.Id.ToString()))}");
}

Console.WriteLine(disintegrationTotal);