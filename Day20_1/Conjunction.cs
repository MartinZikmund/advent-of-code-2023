namespace PulsePropagation;

internal class Conjunction : Module
{
    public Conjunction(Machine machine, string name, string[] outputs) : base(machine, name, outputs)
    {
    }

    public Dictionary<string, PulseType> LastInputPulses { get; } = new();

    public override void AddInput(string inputModule)
    {
        LastInputPulses.Add(inputModule, PulseType.Low);
        base.AddInput(inputModule);
    }

    public override void ProcessPulse(string sourceModuleName, PulseType pulse)
    {
        LastInputPulses[sourceModuleName] = pulse;
        var pulseToSend = LastInputPulses.All(p => p.Value == PulseType.High) ? PulseType.Low : PulseType.High;
        foreach (var output in Outputs)
        {
            SendPulse(Name, output, pulseToSend);
        }
    }
}
