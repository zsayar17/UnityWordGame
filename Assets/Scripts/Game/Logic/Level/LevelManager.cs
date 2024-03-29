
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Utils.Loaders;


public delegate List<Tuple<bool, LevelInfo>> ListToupleDelegate();
public class LevelManager
{
    private static LevelManager _instance;
    public static LevelManager Instance => _instance == null ? _instance = new LevelManager() : _instance;

    private List<Tuple<bool, LevelInfo>> _levelInfos;
    public ListToupleDelegate LevelInfosDelegate => () => _levelInfos;

    private Level[] _levels;
    private Level _currentLevel;
    public int MaxLetterCountPerLevel => LevelLoader.MaxLetterCountPerLevel;
    public int CurrentLevelHighestScore => _currentLevel.Data.highestScore;


    public void LoadLevels()
    {
        bool isPlayable = true;

        _levels = LevelLoader.LoadLevels();
        _levelInfos = new List<Tuple<bool, LevelInfo>>();
        foreach (Level level in _levels)
        {
            _levelInfos.Add(new Tuple<bool, LevelInfo>(isPlayable, level.levelInfo));
            if (!level.Data.isCompleted) isPlayable = false;
        }
    }

    public void StartLevel(int level) => (_currentLevel = _levels[level - 1]).StartLevel();

    public void EndLevel()
    {
        int levelIndex = Array.IndexOf(_levels, _currentLevel);

        if (levelIndex + 1 < _levelInfos.Count && _levelInfos[levelIndex + 1].Item1 == false)
        {
            _levelInfos[levelIndex + 1] = new Tuple<bool, LevelInfo>(true, _levelInfos[levelIndex + 1].Item2); // unlock next level
            _levels[levelIndex + 1].SaveLevel(0);
        }

        _currentLevel.EndLevel();
        _currentLevel = null;
    }
}
