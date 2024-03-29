using System.Collections.Generic;

public delegate List<string> StringsDelegate();
public delegate int IntDelegate();
public delegate bool BoolDelegate();
public delegate BoardInfo BoardInfoDelegate();

public class BoardInfo
{
    public BoolDelegate CanSubmit;
    public BoolDelegate IsCompleted;
    public IntDelegate WordScore;
    public IntDelegate TotalScore;
    public IntDelegate HighScore;
    public StringsDelegate Words;
    public string Hint;
}
