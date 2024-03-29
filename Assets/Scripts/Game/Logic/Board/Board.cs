using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Board
{
    // Shared data between the board and the UI
    public BoardInfo BoardInfo;
    private string word;
    private bool _canSubmit;
    private bool _isCompleted;
    private int _wordScore;
    private int _totalScore;

    ReadOnlyCollection<Letter> _Letters;
    List<Letter> _SelectedLetters;

    ReadOnlyCollection<StackObject> _stackObjects;
    int _stackIndex;



    public Board(LetterData[] letterDatas, string hint, int stackSize)
    {
        SetBoardRequirements();
        CreateBoardInfo(hint);

        Allocate(ref _Letters, letterDatas.Length);
        InitilazeLetters(letterDatas);

        Allocate(ref _stackObjects, stackSize);
        InitilazeStackObjects(stackSize);

        WordManager.Instance.ClearWords();
    }

    public void DestroyBoard()
    {
        Deallocate(ref _Letters);
        Deallocate(ref _stackObjects);

        WordManager.Instance.ClearWords();
    }

    private void SetBoardRequirements()
    {
        word = "";
        _SelectedLetters = new List<Letter>();

        Letter.movingLetterCount = 0;
    }

    private void CreateBoardInfo(string hint)
    {
        BoardInfo = new BoardInfo()
        {
            CanSubmit = () => _canSubmit && Letter.movingLetterCount == 0,
            IsCompleted = () => _isCompleted,
            WordScore = () => _wordScore,
            TotalScore = () => _totalScore,
            Hint = hint,
            HighScore = () => LevelManager.Instance.CurrentLevelHighestScore,
            Words = () => WordManager.Instance.CurentWords
        };
    }

    private void Allocate<T>(ref ReadOnlyCollection<T> collection, int size) where T : BaseObject
        => collection = PoolSystem.Instance.Allocate<T>(size);

    private void Deallocate<T>(ref ReadOnlyCollection<T> collection) where T : BaseObject
    {
        foreach (T tObject in collection) tObject.TurnOffObject();
        PoolSystem.Instance.Deallocate<T>();
    }

    private void InitilazeLetters(LetterData[] letterDatas)
    {
        List<LetterContent>[] childrens = new List<LetterContent>[letterDatas.Length];
        List<LetterContent>[] parents = new List<LetterContent>[letterDatas.Length];

        SetLettersHierarchy(childrens, parents, letterDatas);
        for (int i = 0; i < letterDatas.Length; i++)
            _Letters[i].TurnOnObject(this, letterDatas[i], parents[i], childrens[i]);
    }

    private void SetLettersHierarchy(List<LetterContent>[] childrens, List<LetterContent>[] parents, LetterData[] letterDatas)
    {
        int[] childrenIds;

        for (int i = 0; i < letterDatas.Length; i++) {
            childrens[i] = new List<LetterContent>();
            parents[i] = new List<LetterContent>();
        }

        for (int i = 0; i < letterDatas.Length; i++)
        {
            childrenIds = letterDatas[i].children;
            foreach (int id in childrenIds)
            {
                childrens[i].Add(_Letters[id].content);
                parents[id].Add(_Letters[i].content);
            }
        }
    }

    public void Submit()
    {
        WordManager.Instance.AddWord(word);
        _wordScore = Utils.ObjectInfo.GetScore(_SelectedLetters);
        _totalScore += _wordScore;
        foreach (Letter letter in _SelectedLetters)
        {
            letter.TurnOffObject();
            PoolSystem.Instance.Deallocate<Letter>(letter);

            _stackObjects[--_stackIndex].PlayParticle();
        }
        _Letters = PoolSystem.Instance.GetSafeAllocatedObjects<Letter>();
        _SelectedLetters.Clear();

        word = "";
        _canSubmit = false;
        _isCompleted = !Utils.Algorithm.CheckValidLetterCombinations(_Letters, _stackObjects.Count);
    }

    public void UndoLastLetter()
    {
        if (_SelectedLetters.Count == 0) return;

        word = word.Remove(word.Length - 1);
        _SelectedLetters[^1].Undo();
        _SelectedLetters.RemoveAt(_SelectedLetters.Count - 1);
        _stackIndex--;


        _canSubmit = WordManager.Instance.IsWordExist(word);
    }

    public void UndoAllLetters()
    {
        foreach (Letter letter in _SelectedLetters) letter.Undo();
        _SelectedLetters.Clear();
        _stackIndex = 0;

        word = "";
        _canSubmit = false;
    }

    public void InitilazeStackObjects(int stackSize)
    {
       Vector3 BeginCord;

        BeginCord = Utils.ObjectInfo.StackCenterPosition;
        BeginCord.x -= Utils.ObjectInfo.StackObjectScale.x * (stackSize / 2);

        foreach (StackObject stackObject in _stackObjects)
        {
            stackObject.TurnOnObject(BeginCord);
            BeginCord.x += Utils.ObjectInfo.StackObjectScale.x;
        }
    }

    public void AddToSelectedLetters(Letter letter) {
        _SelectedLetters.Add(letter);
        word += (char)(letter.Character + 32);

        _canSubmit = WordManager.Instance.IsWordExist(word);
    }

    public StackObject GetPlaceableStackObject()
    {
        if (_stackIndex < _stackObjects.Count && !_stackObjects[_stackIndex].IsBusy())
            return _stackObjects[_stackIndex++];
        return null;
    }
}
