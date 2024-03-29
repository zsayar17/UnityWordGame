using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EndScreen : IScreen
{
    private Canvas _canvas;
    private GameObject _background;
    private GameObject _basePanel;
    private TextMeshProUGUI _infoText;
    private TextMeshProUGUI _continueText;

    public EndScreen(Canvas canvas)
    {
        _canvas = canvas;
        LoadPanel();
    }
    public void Invoke()
    {

        _basePanel.SetActive(true);
        _background.SetActive(true);

        _continueText.text = "Tab to continue";
        _infoText.text = "Your score: " + UIManager.Instance.BoardInfos()().TotalScore()
            + "\n" + "High score: " + UIManager.Instance.BoardInfos()().HighScore();

        UIManager.Instance.EndLevel();
    }

    public void Action()
    {
        _continueText.color = new Color(1, 1, 1, Mathf.PingPong(Time.time, 1));
        if (Input.GetMouseButtonDown(0))
        {
            UIManager.Instance.InvokeScreen(UIState.LevelScreen);
        }
    }

    public void DeActivate()
    {
        _background.SetActive(false);
        _basePanel.SetActive(false);
        _infoText.text = "";
    }

    private void LoadPanel()
    {
        _basePanel = _canvas.transform.GetChild(1).Find("EndLevel").gameObject;
        _infoText = _basePanel.transform.Find("EndInfoText").GetComponent<TextMeshProUGUI>();
        _continueText = _basePanel.transform.Find("ContinueText").GetComponent<TextMeshProUGUI>();
        _background = _canvas.transform.GetChild(0).gameObject;
    }
}
