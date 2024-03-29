using System.IO;
using WordContainer = System.Collections.Generic.SortedSet<string>;
using WordsContainer = System.Collections.Generic.List<System.Collections.Generic.SortedSet<string>>;

namespace Utils.Loaders
{
    public class WordLoader
    {
        public static WordsContainer LoadWords()
        {
            StreamReader reader;
            WordsContainer words;
            string line;

            reader = new StreamReader(Utils.Constants.WordsPath);
            words = new WordsContainer();
            while ((line = reader.ReadLine()) != null) LoadWord(words, line);

            return words;
        }

        public static void LoadWord(WordsContainer wordsContainer, string word)
        {
            int length = word.Length;

            if (length > wordsContainer.Count)
                for (int i = wordsContainer.Count; i < length; i++) wordsContainer.Add(new WordContainer());

            wordsContainer[length - 1].Add(word);
        }
    }
}

