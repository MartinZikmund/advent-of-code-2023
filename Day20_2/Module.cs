namespace PulsePropagation;

internal abstract class Module
{

    protected readonly List<string> _inputs = new();
    protected readonly Machine _machine;

    public Module(Machine machine, string name, string[] outputs)
    {
        _machine = machine;
        Name = name;
        Outputs = outputs;
    }

    protected void SendPulse(string sourceModuleName, string target, PulseType pulse)
    {
        _machine.EnqueuePulse(sourceModuleName, target, pulse);
    }

    public string Name { get; }

    public string[] Outputs { get; }

    public virtual void AddInput(string inputModule) => _inputs.Add(inputModule);

    public abstract void ProcessPulse(string sourceModuleName, PulseType pulse);
}
