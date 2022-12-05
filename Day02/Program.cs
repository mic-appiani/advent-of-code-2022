using Day02;
using System.Text;

string? input;
int totalScorePart1 = 0;
int totalScorePart2 = 0;

using (var sr = new StreamReader("input.txt"))
{
    while (!sr.EndOfStream)
    {
        input = sr.ReadLine();
        var choices = input.ToCharArray();
        var firstInput = choices[0];
        var secondInput = choices[2];

        totalScorePart1 += CalculateGameOutcome(firstInput, secondInput);

        // Part two: pick shape to have the outcome
        totalScorePart2 += CalculatePart2(firstInput, secondInput);
    }

    Console.WriteLine($"Total score for part 1: {totalScorePart1}");
    Console.WriteLine($"Total score for part 2: {totalScorePart2}");
}

int CalculatePart2(char opponentPick, char desiredResult)
{
    // DesiredResult
    char playerPick = CalculatePlayerPick(opponentPick, desiredResult);

    int outcomePoints = CalculateGameOutcome(opponentPick, playerPick);
    
    return outcomePoints;
}

char CalculatePlayerPick(char opponentPick, char desiredResult)
{
    if (desiredResult == DesiredResult.Loss ) 
    {
        return opponentPick switch
        {
            OpponentInput.Rock => PlayerInput.Scissors,
            OpponentInput.Paper => PlayerInput.Rock,
            OpponentInput.Scissors => PlayerInput.Paper,
            _ => throw new ArgumentException("Invalid input.")
        };
    }

    if (desiredResult == DesiredResult.Draw) 
    {
        return opponentPick switch
        {
            OpponentInput.Rock => PlayerInput.Rock,
            OpponentInput.Paper => PlayerInput.Paper,
            OpponentInput.Scissors => PlayerInput.Scissors,
            _ => throw new ArgumentException("Invalid input.")
        };
    }

    if (desiredResult == DesiredResult.Win) 
    {
        return opponentPick switch
        {
            OpponentInput.Rock => PlayerInput.Paper,
            OpponentInput.Paper => PlayerInput.Scissors,
            OpponentInput.Scissors => PlayerInput.Rock,
            _ => throw new ArgumentException("Invalid input.")
        };
    }

    throw new ArgumentException("Invalid input.");
}

int CalculateGameOutcome(char opponentPick, char playerPick)
{
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
