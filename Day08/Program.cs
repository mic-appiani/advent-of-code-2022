
var rowList = new List<string>();

using (var sr = new StreamReader("input.txt"))
{

    while (!sr.EndOfStream)
    {
        rowList.Add(sr.ReadLine());
    }
}

//  convert to array
var forest = rowList.Select(row => row.ToCharArray()
        .Select(c => (int)char.GetNumericValue(c))
        .ToArray())
    .ToArray();

var width = forest[0].Length;
var height = forest.Length;

//for (int row = 0; row < width; row++)
//{
//    for (int col = 0; col < height; col++)
//    {
//        Console.Write($"{forest[row][col]} ");
//    }
//    Console.WriteLine();
//}

// external perimeter calculation
var perimeter = (width * 2) + (height - 2) * 2;
Console.WriteLine($"Trees on the outside edge: {perimeter}");

var visibleInside = 0;
var topScore = 0;
// internal tree check
for (int row = 1; row < width - 1; row++)
{
    for (int col = 1; col < height - 1; col++)
    {
        var evaluation = EvaluateTree(row, col);
        if (evaluation.IsVisible)
        {
            visibleInside++;
        }

        if (evaluation.Score > topScore)
        {
            topScore = evaluation.Score;
        }
    }
}

Console.WriteLine($"Visible trees inside: {visibleInside}");
Console.WriteLine($"Total visible trees: {visibleInside + perimeter}");
Console.WriteLine($"Top score: {topScore}");


CheckResult EvaluateTree(int row, int col)
{
    var results = new List<CheckResult>
    {
        EvaluateNorth(row, col),
        EvaluateEast(row, col),
        EvaluateSouth(row, col),
        EvaluateWest(row, col)
    };

    var isVisible = results.Any(x => x.IsVisible);
    var score = results.Select(x => x.Score)
        .Aggregate((a, x) => a * x);

    return new CheckResult { IsVisible = isVisible, Score = score  };
}

CheckResult EvaluateNorth(int row, int col)
{
    var tree = forest[row][col];
    var eval = new CheckResult();

    for (int search = row - 1; search >= 0; search--)
    {
        eval.Score++;

        if (forest[search][col] >= tree)
        {
            eval.IsVisible = false;
            return eval;
        }
    }

    eval.IsVisible = true;
    return eval;
}
CheckResult EvaluateSouth(int row, int col)
{
    var tree = forest[row][col];
    var eval = new CheckResult();

    for (int search = row + 1; search < forest.Length; search++)
    {
        eval.Score++;

        if (forest[search][col] >= tree)
        {
            eval.IsVisible = false;
            return eval;
        }
    }

    eval.IsVisible = true;
    return eval;
}
CheckResult EvaluateEast(int row, int col)
{
    var tree = forest[row][col];
    var eval = new CheckResult();

    for (int search = col + 1; search < width; search++)
    {
        eval.Score++;

        if (forest[row][search] >= tree)
        {
            eval.IsVisible = false;
            return eval;
        }
    }

    eval.IsVisible = true;
    return eval;
}
CheckResult EvaluateWest(int row, int col)
{
    var tree = forest[row][col];
    var eval = new CheckResult();

    for (int search = col -1; search >= 0; search--)
    {
        eval.Score++;

        if (forest[row][search] >= tree)
        {
            eval.IsVisible = false;
            return eval;
        }
    }

    eval.IsVisible = true;
    return eval;
}

record CheckResult
{
    public bool IsVisible { get; set; }
    public int Score { get; set; }
}