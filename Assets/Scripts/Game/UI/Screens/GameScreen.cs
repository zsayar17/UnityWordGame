using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameScreen : IScreen
{
    private Canvas _canvas;
    private GameObject _gamePanel;
    private TextMeshProUGUI _words, _highestScore, _title;
    private Button _undoButton, _activeSubmitButton, _passiveSubmitButton;
    BoardInfoDelegate _boardInfo;

    public GameScreen(Canvas canvas)
    {
        _canvas = canvas;
        InitUIElements();
    }

    public void Invoke()
    {
        _gamePanel.SetActive(true);
        _activeSubmitButton.onClick.AddListener(Submit);
        _activeSubmitButton.gameObject.SetActive(false);

        RefreshInfo();
    }

    public void Action()
    {
        SetButtons();

        if (_boardInfo().IsCompleted() == true)
        {
            if(_boardInfo().TotalScore() > _boardInfo().HighScore())
                UIManager.Instance.InvokeScreen(UIState.CelebrateScreen);
            else
                UIManager.Instance.InvokeScreen(UIState.EndScreen);
        }

    }

    public void DeActivate()
    {
        _gamePanel.SetActive(false);
        _activeSubmitButton.onClick.RemoveAllListeners();
    }

    private void InitUIElements()
    {
        _gamePanel = _canvas.transform.GetChild(1).transform.Find("GamePanel").gameObject;
        _words = _gamePanel.transform.Find("WordPanel").GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _highestScore = _gamePanel.transform.Find("HighestScore").GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _title = _gamePanel.transform.Find("Title").GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _undoButton = _gamePanel.transform.Find("UndoButton").GetComponent<Button>();
        _activeSubmitButton = _gamePanel.transform.Find("ActiveSubmitButton").GetComponent<Button>();
        _passiveSubmitButton = _gamePanel.transform.Find("PassiveSubmitButton").GetComponent<Button>();
    }

    private void SetButtons()
    {
        if (_boardInfo().CanSubmit() && !_activeSubmitButton.IsActive())
        {
            _activeSubmitButton.gameObject.SetActive(true);
            _passiveSubmitButton.gameObject.SetActive(false);
        }
        else if (!_boardInfo().CanSubmit() && !_passiveSubmitButton.IsActive())
        {
            _activeSubmitButton.gameObject.SetActive(false);
            _passiveSubmitButton.gameObject.SetActive(true);
        }
    }

    private void Submit()
    {
        UIManager.Instance.Submit();
        _words.text += (_words.text.Length != 0 ? "\n" + _boardInfo().Words()[^1] : _boardInfo().Words()[^1]) + ": " + _boardInfo().WordScore();
        _highestScore.text = "Total Score: " + _boardInfo().TotalScore();
    }

    private void RefreshInfo()
    {
        _boardInfo = BoardManager.Instance.BoardInfo;
        _words.text = "";
        _highestScore.text = "";
        _title.text = _boardInfo().Hint;
    }
}
