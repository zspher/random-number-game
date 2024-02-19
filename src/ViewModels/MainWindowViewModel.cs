using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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

    [RelayCommand]
    private void TryGuess()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        int input = int.Parse((string)UserInput!);

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
