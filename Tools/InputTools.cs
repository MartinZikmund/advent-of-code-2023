namespace Tools;

public static class InputTools
{
    public static string[] ReadAllLines()
    {
        var lines = new List<string>();
        while (Console.ReadLine() is string line)
        {
            lines.Add(line);
        }

        return lines.ToArray();
    }
}