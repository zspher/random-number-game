using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RandomNumberGame.ViewModels;

public partial class MainWindowViewModel : ObservableValidator
{
    [ObservableProperty]
    private string _atitle = "Random Number Game";

    [ObservableProperty]
    private int _attemptsLeft = 10;

    private static readonly Random rnd = new();

    [ObservableProperty]
    private int _randomNumber = rnd.Next(0, 100);

    [ObservableProperty]
    [Range(1, 100)]
    private object? _userInput;

    [ObservableProperty]
    private ObservableCollection<Guess> _attemptsList = new([]);

    [ObservableProperty]
    private bool _gameFinished = false;

    [RelayCommand]
    private static void QuitGame(Window window)
    {
        window.Close("Bye");
    }

    [RelayCommand]
    private void ResetGame()
    {
        GameFinished = false;
        RandomNumber = rnd.Next(0, 100);
        AttemptsList = new ObservableCollection<Guess>([]);
        AttemptsLeft = 10;
    }

    [RelayCommand]
    [RequiresUnreferencedCode(
        "Calls CommunityToolkit.Mvvm.ComponentModel.ObservableValidator.ValidateAllProperties()"
    )]
    private void TryGuess()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        if (UserInput is null)
            return;

        int input = int.Parse($"{UserInput}", CultureInfo.CurrentCulture);

        if (AttemptsLeft <= 0)
            GameFinished = true;

        if (input == RandomNumber)
            GameFinished = true;

        if (GameFinished)
            return;

        AttemptsLeft -= 1;
        AttemptsList.Insert(0, new Guess(input, RandomNumber));
    }
}

public class Guess
{
    public int ActualNum { get; set; }
    public int Num { get; set; }
    public bool Success { get; set; }

    public string? Result { get; set; }

    public Guess(int guess, int actualNum)
    {
        Num = guess;
        ActualNum = actualNum;

        switch (guess)
        {
            case int n when n > actualNum:
                Success = false;
                Result = "higher than";
                break;
            case int n when n < actualNum:
                Success = false;
                Result = "lower than";
                break;
            case int n when n == actualNum:
                Success = true;
                break;
            default:
                break;
        }
    }
}
