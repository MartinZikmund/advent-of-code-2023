using PulsePropagation;

Machine machine = new Machine();
var input = File.ReadAllLines("input.txt");
foreach (var line in input)
{
    var parts = line.Split(" -> ");
    var designation = parts[0];
    var outputs = parts[1].Split(", ");
    if (designation == "broadcaster")
    {
        Broadcaster broadcaster = new Broadcaster(machine, designation, outputs);
        machine.AddModule(broadcaster);
    }
    else
    {
        var name = designation.Substring(1);
        Module module = designation[0] switch
        {
            '%' => new FlipFlop(machine,name, outputs),
            '&' => new Conjunction(machine, name, outputs),
            _ => throw new InvalidOperationException()
        };
        machine.AddModule(module);
    }
}

machine.AddInputs();
var mgModule = (Conjunction)machine["mg"];

while (true)
{
    machine.PressButton();

    if (mgModule.HighPulses.Count == mgModule.LastInputPulses.Count)
    {
        break;
    }
}

// Get Lowest Common Multiple of all the high pulses
long Lcm(long a, long b) => a * b / Gcd(a, b);
long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

long lcm = 1;
foreach (var highPulse in mgModule.HighPulses.Values)
{
    lcm = Lcm(lcm, highPulse);
}

Console.WriteLine(lcm);