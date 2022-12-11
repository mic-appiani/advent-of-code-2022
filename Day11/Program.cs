Dictionary<string, Monkey> monkeys = new();

using (var sr = new StreamReader("input.txt"))
{
    while (!sr.EndOfStream)
    {

        var monkeyData = new string[7];
        // 7 lines
        for (int i = 0; i < 7; i++)
        {
            monkeyData[i] = sr.ReadLine();
        }

        var monkey = CreateMonkey(monkeyData);

        monkeys.Add(monkey.Key, monkey);
    }

    for (int round = 1; round <= 20; round++)
    {
        Console.WriteLine($"Round {round} starts.");
        var totItems = monkeys.Values.Select(x => x.StartingItems.Count).Sum();
        Console.WriteLine($"Total items = {totItems}");
        for (int i = 0; i < monkeys.Keys.Count; i++)
        {
            var monkey = monkeys[i.ToString()];
            monkey.PlayRound(monkeys);
        }

        totItems = monkeys.Values.Select(x => x.StartingItems.Count).Sum();
        Console.WriteLine($"Round {round} ends.");
    }

    var ordered = monkeys.Values.OrderByDescending(x => x.InspectedItems).ToList();
        var top2 = ordered.Take(2).Select(x => x.InspectedItems).ToArray();
    var monkeyBusiness = top2[0] * top2[1];
    Console.WriteLine($"monkey business is: {monkeyBusiness}");

}

Monkey CreateMonkey(string[] monkeyData)
{
    var monkey = new Monkey();

    // Assuming monkey id is single char
    monkey.Key = monkeyData[0][7].ToString();

    var items = monkeyData[1].Substring(18).Split(",").Select(x => x.Trim()).ToList();
    monkey.StartingItems = new Queue<string>(items);

    monkey.Operation = monkeyData[2].Substring(19);

    monkey.TestDivider = int.Parse(monkeyData[3].Substring(21));

    // Assuming monkey id is single char
    monkey.TargetOnSuccessfulTest = monkeyData[4].Last().ToString();
    monkey.TargetOnFailedTest = monkeyData[5].Last().ToString();

    return monkey;
}

class Monkey
{
    public string Key { get; set; } = string.Empty;
    public Queue<string> StartingItems { get; set; } = new Queue<string>();

    public int TestDivider { get; set; } = 0;
    public string TargetOnSuccessfulTest = string.Empty;
    public string TargetOnFailedTest = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public int InspectedItems { get; set; } = 0;

    private void Throw(string item, string targetKey, Dictionary<string, Monkey> monkeys)
    {
        monkeys[targetKey].StartingItems.Enqueue(item);
    }

    internal void PlayRound(Dictionary<string, Monkey> monkeys)
    {
        while (StartingItems.Count > 0)
        {
            // Monkey inspects an item with a worry level of 79.
            var item = StartingItems.Dequeue();

            // Worry level is multiplied by 19 to 1501.
            var worry = DoOperation(item);

            // Monkey gets bored with item.Worry level is divided by 3 to 500.
            worry = worry / 3;

            if (worry % TestDivider == 0)
            {
                Throw(worry.ToString(), TargetOnSuccessfulTest, monkeys);
            }
            else
            {
                Throw(worry.ToString(), TargetOnFailedTest, monkeys);
            }

            InspectedItems++;
        }

    }

    private int DoOperation(string item)
    {
        // left operand is always "old"
        int leftOperand = int.Parse(item);

        var split = Operation.Split(' ');
        var rightOperandString = split[2];

        var rightOperand = rightOperandString switch
        {
            "old" => leftOperand,
            _ => int.Parse(rightOperandString),
        };

        var operatorSymbol = split[1];

        if (operatorSymbol == "+")
        {
            return leftOperand + rightOperand;
        }
        else if (operatorSymbol == "*")
        {
            return leftOperand * rightOperand;
        }

        throw new Exception("Unexpected path");
    }
}