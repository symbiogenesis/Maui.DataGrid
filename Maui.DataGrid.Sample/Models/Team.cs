namespace Maui.DataGrid.Sample.Models;

using System.Windows.Input;
using System.ComponentModel;

public class Team : INotifyPropertyChanged
{
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
    public int Won { get; set; }
    public int Lost { get; set; }
    public double Percentage { get; set; }
    public string Conf { get; set; }
    public string Div { get; set; }
    public string Home { get; set; }
    public string Road { get; set; }
    public string Last10 { get; set; }
    public Streak Streak { get; set; }
    public string Logo { get; set; }

    #region INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

    #endregion INotifyPropertyChanged implementation
}

public class Streak : IComparable
{
    public Result Result { get; set; }
    public int NumStreak { get; set; }

    public int CompareTo(object obj)
    {
        var score = Result == Result.Won ? NumStreak : -NumStreak;
        if (obj is Streak s)
        {
            var otherScore = s.Result == Result.Won ? s.NumStreak : -s.NumStreak;
            return score - otherScore;
        }

        return score;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Enum.GetName(typeof(Result), Result)} {NumStreak}";
    }
}

public enum Result
{
    Lost = 0,
    Won = 1
}
