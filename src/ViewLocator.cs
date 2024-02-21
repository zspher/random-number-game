using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using RandomNumberGame.ViewModels;

namespace RandomNumberGame;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        string name = param
            .GetType()
            .FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        Type? type = Type.GetType($"{name}");

        if (type != null)
        {
            Control control = (Control)Activator.CreateInstance(type)!;
            control.DataContext = param;
            return control;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
