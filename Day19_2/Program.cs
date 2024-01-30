using System.Data;

var inputSteam = File.OpenRead("input.txt");
var reader = new StreamReader(inputSteam);

Dictionary<string,Workflow> workflows = new Dictionary<string, Workflow>();
string line = "";
while ((line = reader.ReadLine()!) != "")
{
    var parts = line.TrimEnd('}').Split('{');
    var name = parts[0].Trim();
    var rules = parts[1].Split(',');

    var ruleList = new List<Rule>();
    for (var i = 0; i < rules.Length - 1; i++)
    {
        var ruleParts = rules[i].Trim().Split(':');
        var condition = ruleParts[0].Split(new[] { '<', '>' });
        var property = condition[0][0];
        var lessThan = ruleParts[0].IndexOf('<') > -1 ? true : false;
        var value = int.Parse(condition[1]);
        var rule = new Rule(property, lessThan, value, ruleParts[1].Trim());
        ruleList.Add(rule);
    }
    var fallback = rules[rules.Length - 1].Trim();
    var workflow = new Workflow(name, ruleList.ToArray(), fallback);
    workflows.Add(name, workflow);
}

var fullInterval = new PartIntervals(
    new Interval(1, 4000),
    new Interval(1, 4000),
    new Interval(1, 4000),
    new Interval(1, 4000));

var inWorkflow = workflows["in"];
var totalCount = inWorkflow.CountAccepted(fullInterval, workflows);

Console.WriteLine(totalCount);

record Workflow(string Name, Rule[] Rules, string Fallback)
{
    public ulong CountAccepted(PartIntervals part, Dictionary<string, Workflow> workflows)
    {
        var currentIntervals = part;
        ulong totalAccepted = 0;
        foreach (var rule in Rules)
        {
            var (accepted, rejected) = rule.Process(currentIntervals);
            if (!accepted.IsEmpty)
            {
                ProcessNextWorkflow(rule.Target, accepted);
            }

            currentIntervals = rejected;
            if (currentIntervals.IsEmpty)
            {
                break;
            }
        }

        if (!currentIntervals.IsEmpty)
        {
            ProcessNextWorkflow(Fallback, currentIntervals);
        }

        return totalAccepted;

        void ProcessNextWorkflow(string nextWorkflowName, PartIntervals part)
        {
            if (nextWorkflowName == "A")
            {
                checked
                {
                    totalAccepted += ((ulong)part.x.Length * (ulong)part.m.Length * (ulong)part.a.Length * (ulong)part.s.Length);
                }
            }
            else if (nextWorkflowName == "R")
            {
                return;
            }
            else
            {
                var nextWorkflow = workflows![nextWorkflowName];
                totalAccepted += nextWorkflow.CountAccepted(part, workflows);
            }
        }
    }
}

record PartIntervals(Interval x, Interval m, Interval a, Interval s)
{
    public bool IsEmpty => x.Length <= 0 && m.Length <= 0 && a.Length <= 0 && s.Length <= 0;

    public Interval this[char property]
    {
        get => property switch
        {
            'x' => x,
            'm' => m,
            'a' => a,
            's' => s,
            _ => throw new Exception("Unknown property")
        };
    }

    public PartIntervals With(char property, Interval interval)
    {
        return property switch
        {
            'x' => this with { x = interval },
            'm' => this with { m = interval },
            'a' => this with { a = interval },
            's' => this with { s = interval },
            _ => throw new Exception("Unknown property")
        };
    }
}

public record Interval(int Min, int Max)
{
    public int Length => Max - Min + 1;
}

record Rule(char Property, bool LessThan, int Value, string Target)
{
    public (PartIntervals accepted, PartIntervals rejected) Process(PartIntervals part)
    {
        var value = part[Property];

        if (LessThan)
        {
            var newMax = Math.Min(value.Max, Value - 1);
            var accepted = part.With(Property, new Interval(value.Min, newMax));
            var rejected = part.With(Property, new Interval(newMax + 1, value.Max));
            return (accepted, rejected);
        }
        else
        {
            var newMin = Math.Max(value.Min, Value + 1);
            var accepted = part.With(Property, new Interval(newMin, value.Max));
            var rejected = part.With(Property, new Interval(value.Min, newMin - 1));
            return (accepted, rejected);
        }
    }
}