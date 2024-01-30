var inputSteam = File.OpenRead("input.txt");
var reader = new StreamReader(inputSteam);

var workflows = new Dictionary<string, Workflow>();
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
        var value = long.Parse(condition[1]);
        var rule = new Rule(property, lessThan, value, ruleParts[1].Trim());
        ruleList.Add(rule);
    }
    var fallback = rules[rules.Length - 1].Trim();
    var workflow = new Workflow(name, ruleList.ToArray(), fallback);
    workflows.Add(name, workflow);
}

var total = 0L;
while ((line = reader.ReadLine()!) != null)
{
    var parts = line.Substring(1, line.Length - 2).Split(',');
    var part = new Part(
        long.Parse(parts[0].Substring(2)),
        long.Parse(parts[1].Substring(2)),
        long.Parse(parts[2].Substring(2)),
        long.Parse(parts[3].Substring(2)));

    var currentWorkflow = workflows["in"];
    while (true)
    {
        var nextWorkflowName = currentWorkflow.Process(part);
        if (nextWorkflowName == "A")
        {
            total += part.x + part.m + part.a + part.s;
            break;
        }
        else if (nextWorkflowName == "R")
        {
            break;
        }
        else
        {
            currentWorkflow = workflows[nextWorkflowName];
        }
    }
}

Console.WriteLine(total);

record Workflow(string Name, Rule[] Rules, string Fallback)
{
    public string Process(Part part)
    {
        foreach (var rule in Rules)
        {
            if (rule.Process(part))
            {
                return rule.Target;
            }
        }

        return Fallback;
    }
}

record Part(long x, long m, long a, long s);

record Rule(char Property, bool LessThan, long Value, string Target)
{
    public bool Process(Part part)
    {
        var value = Property switch
        {
            'x' => part.x,
            'm' => part.m,
            'a' => part.a,
            's' => part.s,
            _ => throw new Exception("Unknown property")
        };

        return LessThan ? value < Value : value > Value;
    }
}