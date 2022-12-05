string input;

using (var sr = new StreamReader("input.txt"))
{
    // Read the stream as a string, and write the string to the console.
    input = sr.ReadToEnd();
}

var groups = input.Split("\r\n\r\n");

var cals = new List<int>();

try
{
    foreach (var group in groups)
    {
        var singleElfCals = group.Trim()
            .Split("\r\n")
            .Where(x => x.Length > 0)
            .Select(x => Int32.Parse(x))
            .Sum();

        cals.Add(singleElfCals);
    }
}
catch (Exception)
{

	throw;
}


var highest = cals.Max();
Console.WriteLine($"Highest: {highest}");

// Part two
var top3Cals = cals.OrderByDescending(x => x)
                   .Take(3)
                   .Sum();
Console.WriteLine($"Top 3 sum: {top3Cals}");