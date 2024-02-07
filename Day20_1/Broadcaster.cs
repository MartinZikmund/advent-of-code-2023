namespace PulsePropagation;

internal class Broadcaster : Module
{
    public Broadcaster(Machine machine, string name, string[] outputs) : base(machine, name, outputs)
    {
    }

    public override void ProcessPulse(string sourceModuleName, PulseType pulse)
    {
        foreach (var output in Outputs)
        {
            SendPulse(Name, output, pulse);
        }
    }
}
