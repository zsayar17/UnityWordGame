using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Utils
{
    public class Algorithm
    {
        public static bool CheckValidLetterCombinations(ReadOnlyCollection<Letter> letters, int size)
        {
            List<int> selectableIndexes = new List<int>();
            List<int> selectedIndexes = new List<int>();
            string foundWord;

            foreach (Letter letter in letters)
                if (letter.content.Situation == LetterSituation.SELECTABLE) selectableIndexes.Add(letter.ID);

            for (int i = 0; i <= size; i++)
            {
                foundWord = FindValidWord(letters, selectableIndexes, selectedIndexes, "", i);
                if (foundWord != "")
                {
                    //Debug.Log("Found Word: " + foundWord);
                    return true;
                }

            }
            return false;
        }

        private static string FindValidWord(ReadOnlyCollection<Letter> letters, List<int> selectableIndexes, List<int> selectedIndexes, string word, int limit)
        {
            Letter currentLetter;
            List<int> newSelectables;
            List<int> newSelectedIndexes;
            string newWord;
            string foundWord;

           if (word.Length == limit)
           {
               if (WordManager.Instance.IsWordExist(word)) return word;
               return "";
           }

            for (int i = 0; i < selectableIndexes.Count; i++)
            {
                currentLetter = GetLetterByID(letters, selectableIndexes[i]);
                newWord = word + (char)(currentLetter.Character + 32);

                newSelectables = new List<int>(selectableIndexes);
                newSelectedIndexes = new List<int>(selectedIndexes);
                if (newWord.Length < limit)
                {
                    newSelectables.RemoveAt(i);
                    newSelectedIndexes.Add(currentLetter.ID);
                    newSelectables.AddRange(AddChildrenToIndexes(newSelectedIndexes, currentLetter.content.Children));
                }
                foundWord = FindValidWord(letters, newSelectables, newSelectedIndexes, newWord, limit);
                if (foundWord != "") return foundWord;
            }
            return "";
        }

        private static List<int> AddChildrenToIndexes(List<int> selectedIndexes, List<LetterContent> children)
        {
            List<LetterContent> parents;
            List<int> newSelectables = new List<int>();

            for (int i = 0; i < children.Count; i++)
            {
                parents = children[i].Parents;
                for (int j = 0; j < parents.Count; j++)
                {
                    if (!selectedIndexes.Contains(parents[j].BaseLetter.ID)) break;
                    if (j  == parents.Count - 1) newSelectables.Add(children[i].BaseLetter.ID);
                }
            }

            return newSelectables;
        }

        private static Letter GetLetterByID(ReadOnlyCollection<Letter> letters, int id)
        {
            foreach (Letter letter in letters)
                if (letter.ID == id) return letter;
            return null;
        }
    }
}
