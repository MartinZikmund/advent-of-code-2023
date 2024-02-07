

namespace PulsePropagation;

internal class Machine
{
    private readonly Dictionary<string, Module> _modules = new();

    private readonly Queue<(string sourceModuleName, string target, PulseType pulse)> _pulseQueue = new();

    public long LowPulseCount { get; private set; }

    public long HighPulseCount { get; private set; }

    public Module this[string moduleName] => _modules[moduleName];

    public void AddModule(Module module)
    {
        _modules.Add(module.Name, module);
    }

    public void SendPulse(string sourceModuleName, string target, PulseType pulse)
    {
        if (_modules.TryGetValue(target, out var module))
        {
            module.ProcessPulse(sourceModuleName, pulse);
        }
    }

    internal void AddInputs()
    {
        foreach (var module in _modules.Values)
        {
            var outputs = module.Outputs;
            foreach (var output in outputs)
            {
                if (_modules.TryGetValue(output, out var targetModule))
                {
                    targetModule.AddInput(module.Name);
                }
            }
        }
    }

    internal void EnqueuePulse(string sourceModuleName, string target, PulseType pulse)
    {
        if (pulse == PulseType.Low)
        {
            LowPulseCount++;
        }
        else
        {
            HighPulseCount++;
        }

        _pulseQueue.Enqueue((sourceModuleName, target, pulse));
    }

    internal void ProcessPulses()
    {
        while (_pulseQueue.Count > 0)
        {
            var (sourceModuleName, target, pulse) = _pulseQueue.Dequeue();
            SendPulse(sourceModuleName, target, pulse);
        }
    }
}
