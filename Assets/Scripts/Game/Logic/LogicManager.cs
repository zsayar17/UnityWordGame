using UnityEngine;

public class LogicManager
{
    static private LogicManager _instance;
    static public LogicManager Instance => _instance == null ? _instance = new LogicManager() : _instance;

    public void Invoke() { }

    public void Action() => SelectionSystem.Instance.Action();

    public void LoadGame(GameObject letterPrefab, GameObject stackObjectPrefab)
    {
        WordManager.Instance.LoadWords();
        LevelManager.Instance.LoadLevels();
        PoolSystem.Instance.InitilazePool(letterPrefab, stackObjectPrefab);
    }

    public void StartLevel(int level) => LevelManager.Instance.StartLevel(level);
    public void EndLevel() => LevelManager.Instance.EndLevel();


    public void Undo() => BoardManager.Instance.Undo();
    public void Submit() => BoardManager.Instance.Submit();
    public void UndoAll() => BoardManager.Instance.UndoAll();


    public ListToupleDelegate LevelInfos => LevelManager.Instance.LevelInfosDelegate;
    public BoardInfoDelegate BoardInfos() => BoardManager.Instance.BoardInfo;
}
