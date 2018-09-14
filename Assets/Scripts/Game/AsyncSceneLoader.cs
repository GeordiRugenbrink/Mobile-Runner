using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncSceneLoader : MonoBehaviour {

    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private Slider loadingBar;
    [SerializeField]
    private Text percentageText;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        loadingScreen.SetActive(false);
        Utility.sceneLoader = this;
    }

    public void LoadScene(int sceneIndex) {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    /// <summary>
    /// Loads a scene Asynchronously and sets a loadingscreen with a loadingbar
    /// that goes to the right when the progress value increases
    /// </summary>
    /// <param name="sceneIndex">The Scene it needs to load</param>
    /// <returns></returns>
    private IEnumerator LoadAsynchronously(int sceneIndex) {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);

        while (!async.isDone) {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            loadingBar.value = progress;
            percentageText.text = progress * 100f + "%";
            yield return null;
        }
        if (async.isDone) {
            loadingScreen.SetActive(false);
        }
    }
}
