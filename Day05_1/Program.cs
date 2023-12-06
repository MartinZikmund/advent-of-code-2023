const long MapGroupCount = 7;

using var inputStream = File.OpenRead("input.txt");
using var reader = new StreamReader(inputStream);

var seedsLine = reader.ReadLine()!.Substring("seeds:".Length);
var seeds = seedsLine.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(long.Parse)
    .ToArray();

reader.ReadLine(); // empty line

for (long i = 0; i < MapGroupCount; i++)
{
    reader.ReadLine(); // header

    var seedRanges = new List<SeedRange>();
    string? line = reader.ReadLine();
    while (!string.IsNullOrEmpty(line))
    {
        var parts = line.Split(' ').Select(long.Parse).ToArray();
        seedRanges.Add(new SeedRange(parts[0], parts[1], parts[2]));
        line = reader.ReadLine();
    }

    var seedRangeGroup = new SeedRangeGroup(seedRanges.ToArray());

    for (long j = 0; j < seeds.Length; j++)
    {
        seeds[j] = seedRangeGroup.Map(seeds[j]);
    }
}

Console.WriteLine(seeds.Min());

class SeedRangeGroup
{
    private readonly SeedRange[] _seedRanges;

    public SeedRangeGroup(SeedRange[] seedRanges)
    {
        _seedRanges = seedRanges;
    }

    public long Map(long value)
    {
        foreach(var range in _seedRanges)
        {
            if (range.IsInSourceRange(value))
            {
                return range.MapSource(value);
            }
        }

        return value;
    }
}

class SeedRange
{
    private readonly long _destinationStart;
    private readonly long _sourceStart;
    private readonly long _rangeLength;

    public SeedRange(long destinationStart, long sourceStart, long rangeLength)
    {
        _destinationStart = destinationStart;
        _sourceStart = sourceStart;
        _rangeLength = rangeLength;
    }

    public bool IsInSourceRange(long value) =>
        value >= _sourceStart &&
        value < (_sourceStart + _rangeLength);

    public long MapSource(long value) =>
        _destinationStart + (value - _sourceStart);
}