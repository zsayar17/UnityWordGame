using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject letterPrefab, stackObjectPrefab, explosionPrefab;


    private void Awake()
    {
        Instance = this;
        LogicManager.Instance.LoadGame(letterPrefab, stackObjectPrefab);
    }

    private void Start()
    {
        UIManager.Instance.Invoke();
        LogicManager.Instance.Invoke();
    }

    private void Update()
    {
        UIManager.Instance.Action();
        LogicManager.Instance.Action();
    }


    // Logic Methods

    public void StartLevel(int level) => LogicManager.Instance.StartLevel(level);
    public void EndLevel() => LogicManager.Instance.EndLevel();


    public void Undo() => LogicManager.Instance.Undo();
    public void Submit() => LogicManager.Instance.Submit();
    public void UndoAll() => LogicManager.Instance.UndoAll();


    public ListToupleDelegate LevelInfos => LogicManager.Instance.LevelInfos;
    public BoardInfoDelegate BoardInfos() => LogicManager.Instance.BoardInfos();



    // UI Methods
}
