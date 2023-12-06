const long MapGroupCount = 7;

using var inputStream = File.OpenRead("input.txt");
using var reader = new StreamReader(inputStream);

var seedsLine = reader.ReadLine()!.Substring("seeds:".Length);
var seedPairs = seedsLine.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(long.Parse)
    .ToArray();

reader.ReadLine(); // empty line

var mapGroups = new List<RangeMapGroup>();

for (long i = 0; i < MapGroupCount; i++)
{
    reader.ReadLine(); // header

    var maps = new List<RangeMap>();
    string? line = reader.ReadLine();
    while (!string.IsNullOrEmpty(line))
    {
        var parts = line.Split(' ').Select(long.Parse).ToArray();
        maps.Add(new RangeMap(parts[0], parts[1], parts[2]));
        line = reader.ReadLine();
    }

    var seedRangeGroup = new RangeMapGroup(maps.ToArray());
    mapGroups.Add(seedRangeGroup);
}

var seeds = new List<SeedRange>();
for (int seedPairId = 0; seedPairId < seedPairs.Length / 2; seedPairId++)
{
    var startingSeed = seedPairs[seedPairId * 2];
    var length = seedPairs[seedPairId * 2 + 1];
    seeds.Add(new(startingSeed, length));
}

long best = long.MaxValue;


var seedRanges = seeds;
foreach (var group in mapGroups)
{
    var newSeedRanges = new List<SeedRange>();

    foreach (var seedRange in seedRanges)
    {
        var mappedRanges = group.Map(seedRange);
        newSeedRanges.AddRange(mappedRanges);
    }

    seedRanges = newSeedRanges;
}

Console.WriteLine(seedRanges.Select(s => s.Start).Min());

class RangeMapGroup
{
    private readonly RangeMap[] _maps;

    public RangeMapGroup(RangeMap[] maps)
    {
        _maps = maps.OrderBy(s => s.SourceStart).ToArray();
    }

    public SeedRange[] Map(SeedRange range)
    {
        var results = new List<SeedRange>();

        var remainingRange = range;

        foreach (var map in _maps)
        {
            // The a part or whole remaining range is before the map starts
            //    identity map for this part
            if (remainingRange.Start < map.SourceStart)
            {
                var cutOffLength = Math.Min(
                    remainingRange.Length,
                    map.SourceStart - remainingRange.Start);

                var cutOff = new SeedRange(remainingRange.Start, cutOffLength);
                results.Add(cutOff);

                remainingRange = new SeedRange(
                    remainingRange.Start + cutOffLength,
                    remainingRange.Length - cutOffLength);
            }

            if (remainingRange.Length <= 0)
            {
                break;
            }

            // check for intersection with current map
            if (remainingRange.Start >= map.SourceStart &&
                remainingRange.Start < (map.SourceStart + map.RangeLength))
            {
                var intersectionLength = Math.Min(
                    remainingRange.Length,
                    (map.SourceStart + map.RangeLength) - remainingRange.Start);
                var intersection = new SeedRange(remainingRange.Start, intersectionLength);
                var transformedRange = map.Transform(intersection);
                results.Add(transformedRange);

                remainingRange = new SeedRange(
                    remainingRange.Start + intersectionLength,
                    remainingRange.Length - intersectionLength);
            }

            if (remainingRange.Length <= 0)
            {
                break;
            }
        }

        if (remainingRange.Length > 0)
        {
            results.Add(remainingRange);
        }

        return results.ToArray();
    }
}

record RangeMap(long DestinationStart, long SourceStart, long RangeLength)
{
    public bool IsInSourceRange(long value) =>
        value >= SourceStart &&
        value < (SourceStart + RangeLength);

    public long MapSource(long value) =>
        DestinationStart + (value - SourceStart);

    internal SeedRange Transform(SeedRange intersection) =>
        new SeedRange(MapSource(intersection.Start), intersection.Length);
}

record struct SeedRange(long Start, long Length)
{
    public long End => Start + Length - 1;
}