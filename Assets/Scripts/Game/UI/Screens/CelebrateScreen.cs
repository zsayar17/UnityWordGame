using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CelebrateScreen : IScreen
{
    private Canvas _canvas;
    private GameObject _background;
    private GameObject _basePanel;
    private GameObject _celebreateScreen;
    private TextMeshProUGUI _infoText;
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _continueText;

    private bool _isTextAnimateDone;
    private int _score;
    private float _explosionElapsedTime;
    private float _highScoreElapsedTime;
    private const float _explosionDuration = 1;
    private const float _highScoreDuration = 0.005f;

    public CelebrateScreen(Canvas canvas)
    {
        _canvas = canvas;
        LoadPanel();
    }
    public void Invoke()
    {
        UIManager.Instance.EndLevel();

        _basePanel.SetActive(true);
        _background.SetActive(true);
        _celebreateScreen.SetActive(true);
        _continueText.text = "";

        _infoText.text = "New High Score!";
        _scoreText.text = "0";
        _isTextAnimateDone = false;
        _score = 0;
        _explosionElapsedTime = 0;
        _highScoreElapsedTime = 0;
    }

    public void Action()
    {
        AnimateScore();
        AnimateReload();
        AnimateExplosion();
        CheckExit();
    }

    public void DeActivate()
    {
        _background.SetActive(false);
        _basePanel.SetActive(false);
        _celebreateScreen.SetActive(false);
        _infoText.text = "";
    }

    private void LoadPanel()
    {

        _basePanel = _canvas.transform.GetChild(1).Find("EndLevel").gameObject;
        _continueText = _basePanel.transform.Find("ContinueText").GetComponent<TextMeshProUGUI>();

        _celebreateScreen = _basePanel.transform.Find("CelebreteScreen").gameObject;
        _scoreText = _celebreateScreen.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        _infoText = _celebreateScreen.transform.Find("InfoText").GetComponent<TextMeshProUGUI>();

        _background = _canvas.transform.GetChild(0).gameObject;
    }

    private void AnimateScore()
    {
        if (_isTextAnimateDone) return;

        _highScoreElapsedTime += Time.deltaTime;
        if (_highScoreElapsedTime >= _highScoreDuration)
        {
            _score ++;
            _highScoreElapsedTime = 0;
        }

        if (_score >= UIManager.Instance.BoardInfos()().TotalScore())
        {
            _score = UIManager.Instance.BoardInfos()().TotalScore();
            _isTextAnimateDone = true;
            _continueText.text = "Tab to continue";
        }

        _scoreText.text = _score.ToString();
    }

    private void AnimateReload()
    {
        if (!_isTextAnimateDone) return;
        _continueText.color = new Color(1, 1, 1, Mathf.PingPong(Time.time, 1));
    }

    private void CheckExit()
    {
        if (Input.GetMouseButtonDown(0) && _isTextAnimateDone)
            UIManager.Instance.InvokeScreen(UIState.LevelScreen);
    }

    private void AnimateExplosion()
    {
        Vector3 explosionPosition;
        Vector3 randomPosition;

        if (!_isTextAnimateDone) return;

        _explosionElapsedTime += Time.deltaTime;
        if (_explosionElapsedTime >= _explosionDuration)
        {
            _explosionElapsedTime = 0;
            randomPosition = new Vector3(Random.Range(Screen.width /  4, Screen.width - Screen.width / 4)
                , Random.Range(Screen.height /  4, Screen.height - Screen.height / 4), 0);

            explosionPosition = Camera.main.ScreenToWorldPoint(randomPosition);
            Object.Instantiate(GameManager.Instance.explosionPrefab, explosionPosition, Quaternion.identity);
        }
    }
}
