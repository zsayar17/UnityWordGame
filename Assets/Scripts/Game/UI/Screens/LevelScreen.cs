using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelScreenContent
{
    public Button ActiveButton;
    public Button PassiveButton;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TitleText;
    public GameObject Container;
}

public class LevelScreen : IScreen
{
    private Canvas _canvas;
    private GameObject _scrollView;
    private GameObject _content;
    private GameObject _prefab;
    private List<Tuple<bool, LevelInfo>> _infoList;
    private List<LevelScreenContent> _contents;

    public LevelScreen(Canvas canvasObj)
    {
        _canvas = canvasObj;

        _infoList = UIManager.Instance.LevelInfos();
        _scrollView = _canvas.transform.GetChild(1).transform.GetComponentInChildren<ScrollRect>().gameObject;
        CreateContents();
    }
    public void Invoke()
    {
        _scrollView.SetActive(true);
        SetContents();
    }

    public void Action()
    {
        // Implementation for 'IScreen.Action()'
    }

    public void DeActivate()
    {
        _scrollView.SetActive(false);
    }

    private void SetContents()
    {

        _infoList = UIManager.Instance.LevelInfos();

        for (int i = 0; i < _contents.Count; i++)
        {
            _contents[i].TitleText.text = _infoList[i].Item2.LevelName;

            if (_infoList[i].Item1)
            {
                int level = i + 1;

                _contents[i].ActiveButton.gameObject.SetActive(true);
                _contents[i].PassiveButton.gameObject.SetActive(false);
                _contents[i].ActiveButton.onClick.RemoveAllListeners();
                _contents[i].ActiveButton.onClick.AddListener(() => StartLevel(level));
                if (_infoList[i].Item2.HighestScore() == 0) _contents[i].ScoreText.text = "Not Played Yet";
                else _contents[i].ScoreText.text = "Highest Score: " + _infoList[i].Item2.HighestScore().ToString();
            }

            else
            {
                _contents[i].PassiveButton.gameObject.SetActive(true);
                _contents[i].ActiveButton.gameObject.SetActive(false);
                _contents[i].ScoreText.text = "Locked";
            }
        }
    }

    public void StartLevel(int level)
    {
        UIManager.Instance.StartLevel(level);
        UIManager.Instance.InvokeScreen(UIState.GameScreen);
    }

    private void CreateContents()
    {
        GameObject newContainer;

        _content = _scrollView.transform.GetChild(0).GetChild(0).gameObject;
        _prefab = _content.transform.GetChild(0).gameObject;

        _contents = new List<LevelScreenContent>();

        CreateContent(_prefab);
        for (int i = 1; i < _infoList.Count; i++)
        {
            newContainer = GameObject.Instantiate(_prefab, _content.transform);
            newContainer.transform.SetParent(_content.transform, false);
            CreateContent(newContainer);
        }
    }

    private void CreateContent(GameObject gameObject)
    {
        LevelScreenContent content = new LevelScreenContent();

        content.ActiveButton = gameObject.transform.Find("ActiveButton").GetComponent<Button>();
        content.PassiveButton = gameObject.transform.Find("PassiveButton").GetComponent<Button>();
        content.ScoreText = gameObject.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        content.TitleText = gameObject.transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
        content.Container = gameObject;
        _contents.Add(content);
    }
}
