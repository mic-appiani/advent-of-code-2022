/// the start of a packet is indicated by a sequence of four characters that are all different.
/// How many characters need to be processed before the first start-of-packet marker is detected?
using (var sr = new StreamReader("input.txt"))
{
    var data = sr.ReadToEnd();

    // part one
    for (int i = 1; i < data.Length; i++)
    {
        if (i + 4 > data.Length)
        {
            break;
        }

        var marker = data.Substring(i, 4);

        var differentCount = marker.Distinct().Count();

        if (differentCount== 4)
        {
            Console.WriteLine($"Characters processed before finding a  packet marker marker: {i + 4}");
            break;
        }

    }

    // part two
    for (int i = 1; i < data.Length; i++)
    {
        if (i + 14 > data.Length)
        {
            break;
        }

        var marker = data.Substring(i, 14);

        var differentCount = marker.Distinct().Count();

        if (differentCount == 14)
        {
            Console.WriteLine($"Characters processed before finding a  packet message marker: {i + 14}");
            break;
        }

    }
}