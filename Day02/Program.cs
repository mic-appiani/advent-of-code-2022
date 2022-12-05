




using Day02;
using System.Data;
/// The score for a single round is the score for the shape you selected (1 for Rock, 2 for Paper,
/// and 3 for Scissors) plus the score for the outcome of the round (0 if you lost, 3 if the round 
/// was a draw, and 6 if you won). 
/// 
/// What would your total score be if everything goes exactly according to your strategy guide?
/// Opponent:
string? input;

int sum = 0;
using (var sr = new StreamReader("input.txt"))
{
    while (!sr.EndOfStream)
    {
        input = sr.ReadLine();
        sum += CalculateMatch(input);
    }

    Console.WriteLine($"Total score: {sum}");

    // Part two: pick shape to have the outcome
}

int CalculateMatch(string input)
{
    var choices = input.ToCharArray();
    var opponentPick = choices[0];
    var playerPick = choices[2];

    // calculate value of my choice
    var choicePoints = GetChoicePoints(playerPick);
    // calculate result of match
    var matchResult = GetMatchOutcome(opponentPick, playerPick);

    return choicePoints + matchResult;
}

int GetChoicePoints(char choice)
{
    return choice switch
    {
        PlayerInput.Paper => 2, 
        PlayerInput.Rock => 1,
        PlayerInput.Scissors => 3,
        _ => throw new ArgumentException("Invalid input"),
    };
}

int GetMatchOutcome(char opponentInput, char playerInput)
{
    if (opponentInput == OpponentInput.Rock) 
    {
        return playerInput switch
        {
            PlayerInput.Paper => GameOutcomes.Win,
            PlayerInput.Rock => GameOutcomes.Draw,
            PlayerInput.Scissors => GameOutcomes.Loss,
        };
    }

    if (opponentInput == OpponentInput.Paper) 
    {
        return playerInput switch
        {
            PlayerInput.Paper => GameOutcomes.Draw,
            PlayerInput.Rock => GameOutcomes.Loss,
            PlayerInput.Scissors => GameOutcomes.Win,
        };
    }

    if (opponentInput == OpponentInput.Scissors) 
    {
        return playerInput switch
        {
            PlayerInput.Paper => GameOutcomes.Loss,
            PlayerInput.Rock => GameOutcomes.Win,
            PlayerInput.Scissors => GameOutcomes.Draw,
        };
    }

    throw new ArgumentException("Invalid input");
}
