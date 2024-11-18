using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

static char[] StringToArray(string input)
{
    if (input == null)
    {
        throw new ArgumentNullException(nameof(input), "Can be null.");
    }
    return input.ToCharArray();
}

int gamesPlayed = 0;
int gamesWon = 0;

void DisplaySummary()
{
    Console.WriteLine("\nMy Wordle Summary");
    Console.WriteLine("=================");
    Console.WriteLine($"You played {gamesPlayed} games:");
    Console.WriteLine($" |--> Number of wordles solved: {gamesWon}");
    Console.WriteLine($" |--> Number of wordles unsolved: {gamesPlayed - gamesWon}");
    Console.WriteLine("Thanks for playing!");
}

void PlayGame()
{
    gamesPlayed++;

    string jsonText = File.ReadAllText("words.json");
    JArray jsonArray = JArray.Parse(jsonText);
    List<string> words = jsonArray.Select(item => item["word"].ToString()).ToList();

    Random random = new Random();
    string randomWord = words[random.Next(words.Count)];
    //string randomWord = "glass";
    Console.WriteLine($"Correct word (for testing): {randomWord}");
    char[] wordrandom = StringToArray(randomWord);

    List<string> attemptsHistory = new List<string>();
    int maxAttempts = 6;
    bool isSolved = false;

    for (int attempt = 1; attempt <= maxAttempts; attempt++)
    {
        Console.Write($"Please enter your guess - attempt {attempt}: ");
        string inputGuess = Console.ReadLine();

        if (inputGuess.Length != 5)
        {
            Console.WriteLine("You must enter a word with 5 letters! nothing more, nothing less.");
            attempt--;
            continue;
        }

        bool isWordExist = words.Contains(inputGuess);
        if (!isWordExist)
        {
            Console.WriteLine("This word doesn't exist in the list, please enter a valid word!");
            attempt--;
            continue;
        }

        char[] guessArray = StringToArray(inputGuess);
        char[] resultArray = new char[5];
        bool[] letterUsed = new bool[5]; 

      
        for (int i = 0; i < 5; i++)
        {
            if (guessArray[i] == wordrandom[i])
            {
                resultArray[i] = '^';
                letterUsed[i] = true; 
            }
            else
            {
                resultArray[i] = '-';
            }
        }

        
        for (int i = 0; i < 5; i++)
        {
            if (resultArray[i] != '^')
            {
                for (int j = 0; j < 5; j++)
                {
                    if (!letterUsed[j] && guessArray[i] == wordrandom[j])
                    {
                        resultArray[i] = '*';
                        letterUsed[j] = true; 
                        break;
                    }
                }
            }
        }

        string guessResult = $"| {string.Join(" ", guessArray)} |";
        string feedbackResult = $"| {string.Join(" ", resultArray)} |";
        attemptsHistory.Add(guessResult);
        attemptsHistory.Add(feedbackResult);

        Console.WriteLine("-------------");
        foreach (var attemptLine in attemptsHistory)
        {
            Console.WriteLine(attemptLine);
        }

        if (new string(guessArray) == randomWord)
        {
            if (attempt == 1)
            {
                Console.WriteLine("Amazing! You guessed the word on your first try!");
            }
            else
            {
                Console.WriteLine($"Congratulations! You've guessed the word correctly in {attempt} tries!");
            }
            gamesWon++;
            isSolved = true;
            break;
        }
        else if (attempt == maxAttempts)
        {
            Console.WriteLine($"Sorry, you've used all {maxAttempts} attempts. The correct word was: {randomWord}");
        }
    }

    while (true)
    {
        Console.WriteLine("Would you like to play again [y|n]?");
        string playAgain = Console.ReadLine().ToLower();
        if (playAgain == "y")
        {
            PlayGame();
            return;
        }
        else if (playAgain == "n")
        {
            DisplaySummary();
            return;
        }
        else
        {
            Console.WriteLine("Invalid input, please enter 'y' or 'n'.");

        }
    }
}
void Game()
{
    Console.WriteLine("--------------WELCOME TO WORDLE GAME--------------");
    Console.WriteLine("\n------------------------------------------------");
    Console.WriteLine("--             Hai_l3ui Wordle!               --");
    Console.WriteLine("--        Guess the Wordle in 6 tries         --");
    Console.WriteLine("------------------------------------------------");

    Console.WriteLine();
    Console.WriteLine("Would you like to play Hai_l3ui Wordle? [y/n]?");

    Console.Write("Your answer: ");
    string startInput = Console.ReadLine().ToLower();
    if (startInput == "y")
    {
        PlayGame();
    }
    else if (startInput == "n")
    {
        DisplaySummary();
    }
    else
    {
        Console.WriteLine("Invalid input, please enter 'y' or 'n'.");
        Console.WriteLine();
        Game();
    }
}
Game();


