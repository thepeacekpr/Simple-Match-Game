using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public int difficultyIndex, tilesMatched, tilesTurned;

    [Header("<----- Lists ----->")]
    public List<AudioSource> audioSources = new();
    public List<Sprite> tileSprites = new();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        GetReference();
        LoadScene(1);
    }

    private void GetReference()
    {
        loadingScreen = GetComponentInChildren<Canvas>(true).gameObject;
        loadingSlider = GetComponentInChildren<Slider>(true);

        loadingScreen.SetActive(false);
    }

    public void OnButtonClickSound()
    {
        audioSources[0].Play();
    }

    public void OnToggleAudioSources(bool canPlaySound)
    {
        if (audioSources.Count > 0) {
            foreach (var audioSource in audioSources)
            {
                audioSource.enabled = canPlaySound;
            }
        }
    }

    #region LOAD_SCENE
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;
            yield return null;
        }

        if (asyncOperation.isDone)
        {
            loadingSlider.value = loadingSlider.maxValue;
            loadingScreen.SetActive(false);
        }
    }
    #endregion
}