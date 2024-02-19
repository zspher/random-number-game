using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace random_number_game.ViewModels;

public partial class MainWindowViewModel : ObservableValidator
{
    [ObservableProperty]
    private string _atitle = "Random Number Game";

    [ObservableProperty]
    private int _attemptsLeft = 10;

    private static readonly Random rnd = new Random();

    [ObservableProperty]
    private int _randomNumber = rnd.Next(0, 100);

    [ObservableProperty]
    [Range(1, 100)]
    private object? _userInput;

    [ObservableProperty]
    private ObservableCollection<Guess> _attemptsList = new ObservableCollection<Guess>(
        new List<Guess> { }
    );

    [ObservableProperty]
    private bool _gameFinished = false;

    [RelayCommand]
    private void QuitGame(Window window)
    {
        window.Close("Bye");
    }

    [RelayCommand]
    private void ResetGame()
    {
        GameFinished = false;
        RandomNumber = rnd.Next(0, 100);
        AttemptsList = new ObservableCollection<Guess>(new List<Guess> { });
        AttemptsLeft = 10;
    }

    [RelayCommand]
    private void TryGuess()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        if (UserInput is null)
            return;

        int input = int.Parse((string)UserInput);

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

    private string result = "";
    public string? Result
    {
        get => result;
    }

    public Guess(int guess, int actualNum)
    {
        Num = guess;
        ActualNum = actualNum;

        switch (guess)
        {
            case int n when n > actualNum:
                Success = false;
                result = "higher than";
                break;
            case int n when n < actualNum:
                Success = false;
                result = "lower than";
                break;
            case int n when n == actualNum:
                Success = true;
                break;
        }
    }
}
