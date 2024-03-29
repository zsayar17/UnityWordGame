using System.Data.Common;

public class Level
{
    public LevelData Data {get; private set;}
    public int stackSize = Utils.Constants.StackSize;

    public LevelInfo levelInfo;


    public Level(LevelData data)
    {
        Data = data;

        levelInfo = new LevelInfo
        {
            LevelName = data.title.Split(' ')[^1],
            HighestScore = () => Data.highestScore,
        };
    }

    public void SaveLevel(int MaxScore)
    {
        Data.highestScore = MaxScore > Data.highestScore ? MaxScore : Data.highestScore;
        Data.isCompleted = true;
        //LevelLoader.SaveLevel(Data);
    }

    public void StartLevel() => BoardManager.Instance.CreateBoard(Data.tiles, Data.title, stackSize);

    public void EndLevel()
    {
        SaveLevel(BoardManager.Instance.BoardInfo().TotalScore());
        BoardManager.Instance.DestroyBoard();
    }

}
