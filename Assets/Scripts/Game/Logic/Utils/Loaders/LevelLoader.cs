using System.IO;
using UnityEngine;

namespace Utils.Loaders
{
    public class LevelLoader
    {
        public static int MaxLetterCountPerLevel { get; private set; }
        public static Level[] LoadLevels()
        {
            int levelCount;
            LevelData[] levelDatas;
            Level[] levels;

            levelCount = GetLevelCount();
            levelDatas = new LevelData[levelCount];
            levels = new Level[levelCount];

            for (int i = 0; i < levels.Length; i++)
            {
                levelDatas[i] = LoadLevel(i + 1);
                levels[i] = new Level(levelDatas[i]);

                if (levelDatas[i].tiles.Length > MaxLetterCountPerLevel)
                    MaxLetterCountPerLevel = levelDatas[i].tiles.Length ;
            }
            return levels;
        }

        private static LevelData LoadLevel(int levelIndex)
            => JsonUtility.FromJson<LevelData>(Resources.Load<TextAsset>(Utils.Constants.LevelPath + Utils.Constants.LevelPrefix + levelIndex).text);

        // 2 because of .meta files
        private static int GetLevelCount() => Directory.GetFiles(Utils.Constants.ResourcesPath + Utils.Constants.LevelPath).Length / 2;
    }
}


