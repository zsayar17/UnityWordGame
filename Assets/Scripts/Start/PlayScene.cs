using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections;
using Unity.Mathematics;
using TMPro;
public class PlayScene : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] Slider _slider;

    void Start()
    {
        _slider.transform.gameObject.SetActive(false);

        _button.onClick.AddListener(() =>
        {
            _button.transform.gameObject.SetActive(false);
            StartCoroutine(LoadSceneAsync());
        });
    }


    IEnumerator LoadSceneAsync()
    {
        _slider.transform.gameObject.SetActive(true);
        _slider.value = 0;
        float progress = 0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncLoad.progress, Time.deltaTime / 10);
            _slider.value = progress;
            if (progress >= 0.9f)
            {
                _slider.value = 1;
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
