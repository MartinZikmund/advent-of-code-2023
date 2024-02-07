namespace PulsePropagation;

internal class FlipFlop : Module
{
    private bool _isOn = false;

    public FlipFlop(Machine machine, string name, string[] outputs) : base(machine, name, outputs)
    {
    }

    public override void ProcessPulse(string sourceModuleName, PulseType pulse)
    {
        if (pulse == PulseType.High) 
        {
            return;
        }

        _isOn = !_isOn;
        foreach (var output in Outputs)
        {
            SendPulse(Name, output, _isOn ? PulseType.High : PulseType.Low);
        }
    }
}
