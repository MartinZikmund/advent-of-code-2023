var input = File.ReadAllLines("input.txt");

long runningTotal = 0;
foreach (var line in input)
{
    var parts = line.Split(' ');
    var pattern = parts[0];
    var groups = parts[1].Split(',').Select(int.Parse).ToArray();
    runningTotal += GetArrangmentsCount(pattern, groups);
}

Console.WriteLine(runningTotal);

int GetArrangmentsCount(string pattern, int[] groups)
{
    var arrangements = new int[pattern.Length + 1, groups.Length + 1];
    arrangements[0, 0] = 1;

    for (int patternLength = 1; patternLength <= pattern.Length; patternLength++)
    {
        var patternIndex = patternLength - 1;
        for (int groupCount = 0; groupCount <= groups.Length; groupCount++)
        {
            var character = pattern[patternIndex];
            if (character == '.' || character == '?')
            {
                arrangements[patternLength, groupCount] += arrangements[patternLength - 1, groupCount];
            }
            
            if (character == '#' || character == '?')
            {
                if (groupCount == 0)
                {
                    continue;
                }

                // The last group must end on this character
                var groupSize = groups[groupCount - 1];
                if (patternLength < groupSize)
                {
                    continue;
                }

                bool canPlaceGroup = true;
                for (int endIndex = patternIndex; endIndex >= patternIndex - groupSize + 1; endIndex--)
                {
                    if (pattern[endIndex] == '.')
                    {
                        canPlaceGroup = false;
                        break;
                    }
                }

                // There must be a non-# before the group
                if (patternIndex - groupSize >= 0 && pattern[patternIndex - groupSize] == '#')
                {
                    canPlaceGroup = false;
                }

                if (canPlaceGroup)
                {
                    if (patternLength == groupSize)
                    {
                        if (groupCount == 1)
                        {
                            arrangements[patternLength, groupCount] += 1;
                        }
                    }
                    else
                    {
                        arrangements[patternLength, groupCount] +=
                            arrangements[patternLength - groupSize - 1, groupCount - 1];
                    }
                }
            }
        }
    }

    return arrangements[pattern.Length, groups.Length];
}