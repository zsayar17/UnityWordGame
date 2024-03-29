using UnityEngine;

public enum UIState
{
    LevelScreen,
    GameScreen,
    EndScreen,
    CelebrateScreen
}


public class UIManager
{
    static private UIManager _instance;
    static public UIManager Instance => _instance == null ? _instance = new UIManager() : _instance;

    private IScreen[] _screens;

    public Canvas canvas;
    public UIState currentState {get; private set;}


    public void Invoke()
    {
        canvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();

        _screens = new IScreen[]
        {
            new LevelScreen(canvas),
            new GameScreen(canvas),
            new EndScreen(canvas),
            new CelebrateScreen(canvas)
        };

        foreach (var screen in _screens) screen.DeActivate();

        currentState = UIState.LevelScreen;
        _screens[(int)currentState].Invoke();
    }

    public void Action()
    {
        _screens[(int)currentState].Action();
    }

    public void InvokeScreen(UIState state)
    {
        _screens[(int)currentState].DeActivate();
        currentState = state;
        _screens[(int)currentState].Invoke();
    }

    public void StartLevel(int level) => GameManager.Instance.StartLevel(level);
    public void EndLevel() => GameManager.Instance.EndLevel();


    public void Undo() => GameManager.Instance.Undo();
    public void Submit() => GameManager.Instance.Submit();
    public void UndoAll() => GameManager.Instance.UndoAll();


    public ListToupleDelegate LevelInfos => GameManager.Instance.LevelInfos;
    public BoardInfoDelegate BoardInfos() => GameManager.Instance.BoardInfos();

    public GameObject FireWorkPrefab => GameManager.Instance.explosionPrefab;
}
