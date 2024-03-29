using System.Collections.Generic;
using WordsContainer = System.Collections.Generic.List<System.Collections.Generic.SortedSet<string>>;

class WordManager
{
    private static WordManager _instance;
    public static WordManager Instance => _instance == null ? _instance = new WordManager() : _instance;

    private WordsContainer _allWords;
    public List<string> CurentWords;
    public bool IsCurrentWordValid { get; private set; }
    public int LongestWordLength { get => _allWords.Count;}

    WordManager()
    {
        CurentWords = new List<string>();
    }

    public void LoadWords() => _allWords = Utils.Loaders.WordLoader.LoadWords();

    public void ClearWords() => CurentWords.Clear();

    public void AddWord(string word) => CurentWords.Add(word);


    public bool IsWordExist(string word) => word.Length != 0 && _allWords[word.Length - 1].Contains(word) && !CurentWords.Contains(word);
}
