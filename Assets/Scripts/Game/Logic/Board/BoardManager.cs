
public class BoardManager
{
    private static BoardManager _instance;

    public static BoardManager Instance => _instance == null ? _instance = new BoardManager() : _instance;

    private Board _currentBoard;

    public BoardInfoDelegate BoardInfo => () => _currentBoard != null ?_currentBoard.BoardInfo : null;

    public void CreateBoard(LetterData[] letterDatas, string hint, int stackSize) => _currentBoard = new Board(letterDatas, hint, stackSize);

    public void Submit() => _currentBoard.Submit();

    public void DestroyBoard() => _currentBoard.DestroyBoard();

    public void Undo() => _currentBoard.UndoLastLetter();

    public void UndoAll() => _currentBoard.UndoAllLetters();
}
