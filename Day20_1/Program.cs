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

for (int i = 0; i < 1000; i++)
{
    machine.EnqueuePulse("button", "broadcaster", PulseType.Low);
    machine.ProcessPulses();

}

Console.WriteLine(machine.LowPulseCount * machine.HighPulseCount);