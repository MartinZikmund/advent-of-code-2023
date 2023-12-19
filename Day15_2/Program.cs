var input = File.ReadAllText("input.txt");

var parts = input.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

var boxes = new LinkedList<Lens>[256];
for (int i = 0; i < boxes.Length; i++)
{
    boxes[i] = new();
}

foreach (var part in parts)
{
    if (part.EndsWith('-'))
    {
        // Removal
        var label = part.Substring(0, part.Length - 1);
        var boxId = Hash(label);
        var box = boxes[boxId];
        if (FindLens(box, label) is { } lens)
        {
            box.Remove(lens);
        }
    }
    else
    {
        // Insertion
        var parts2 = part.Split("=", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var label = parts2[0];
        var focalLength = int.Parse(parts2[1]);

        var boxId = Hash(label);
        var box = boxes[boxId];
        if (FindLens(box, label) is { } lens)
        {
            lens.Value.FocalLength = focalLength;
        }
        else
        {
            box.AddLast(new Lens(label, focalLength));
        }
    }
}

var runningTotal = 0;
for (int i = 0; i < boxes.Length; i++)
{
    var boxNumber = i + 1;
    var lensNumber = 1;
    foreach(var lens in boxes[i])
    {
        runningTotal += boxNumber * lensNumber * lens.FocalLength;
        lensNumber++;
    }
}

Console.WriteLine(runningTotal);

LinkedListNode<Lens>? FindLens(LinkedList<Lens> box, string label)
{
    var current = box.First;
    while (current is not null)
    {
        if (current.Value.Label == label)
        {
            return current;
        }

        current = current.Next;
    }

    return null;
}

int Hash(string input)
{
    var value = 0;
    foreach (var c in input)
    {
        value += c;
        value *= 17;
        value %= 256;
    }

    return value;
}

class Lens
{
    public Lens(string label, int focalLength)
    {
        Label = label;
        FocalLength = focalLength;
    }

    public string Label { get; }

    public int FocalLength { get; set; }
}