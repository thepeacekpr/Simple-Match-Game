using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public int difficultyIndex, tilesMatched, tilesTurned, score, scoreMultiplier = 1;
    public GridController currentGridController;
    public bool canAudioBePlayed = true;

    [Header("<----- Lists ----->")]
    public List<AudioSource> audioSources = new();
    public List<AudioClip> audioClips = new();
    public List<Sprite> tileSprites = new();
    public List<TileController> selectedTileControllers = new();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        GetReference();
        LoadScene(1);
    }

    #region LOAD_SCENE
    public void LoadScene(int sceneIndex)
    {
        scoreMultiplier = 1;
        tilesMatched = tilesTurned = score = 0;
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

    private void GetReference()
    {
        loadingScreen = GetComponentInChildren<Canvas>(true).gameObject;
        loadingSlider = GetComponentInChildren<Slider>(true);

        loadingScreen.SetActive(false);
    }

    public void OnButtonClickSound() => audioSources[0].Play();

    public void PlaySoundEffects(int index)
    {
        audioSources[1].clip = audioClips[index];
        audioSources[1].Play();
    }

    public void OnToggleAudioSources(bool canPlaySound)
    {
        if (audioSources.Count > 0)
        {
            foreach (var audioSource in audioSources)
            {
                audioSource.enabled = canPlaySound;
            }
        }
    }

    public void CheckTileSelection()
    {
        StartCoroutine(CheckTileSelectionRoutine());
    }

    private TileController GetTileController(TileController tileController)
    {
        int index = currentGridController.tileControllers.IndexOf(tileController);
        return currentGridController.tileControllers[index];
    }

    private IEnumerator CheckTileSelectionRoutine()
    {
        if (selectedTileControllers.Count > 1)
        {
            tilesTurned++;
            currentGridController.CanButtonsInteract(false);
            yield return new WaitForSecondsRealtime(1.0f);

            var tileControllerOne = GetTileController(selectedTileControllers[0]);
            var tileControllerTwo = GetTileController(selectedTileControllers[1]);

            if (tileControllerOne.fruitVariety.Equals(tileControllerTwo.fruitVariety))
            {
                scoreMultiplier++;
                score += 20 * scoreMultiplier;

                PlaySoundEffects(1);

                tileControllerOne.ReverseFlip();
                tileControllerTwo.ReverseFlip();

                tileControllerOne.gameObject.SetActive(false);
                tileControllerTwo.gameObject.SetActive(false);

                tilesMatched++;
                if (currentGridController.CheckGridIsEmpty())
                {
                    PlaySoundEffects(0);
                    LevelManager.Instance.endMessage.SetActive(true);
                    if (PlayerPrefs.GetInt($"Score {difficultyIndex}") < score)
                    {
                        PlayerPrefs.SetInt($"Score {difficultyIndex}", score);
                        PlayerPrefs.SetInt($"Tiles Turned {difficultyIndex}", tilesTurned);
                    }
                }
            }
            else
            {
                scoreMultiplier = 1;
                score -= score > 0 ? 3 : 0;

                PlaySoundEffects(2);

                tileControllerOne.ReverseFlip();
                tileControllerTwo.ReverseFlip();
            }

            LevelManager.Instance.tilesMatchedText.text = tilesMatched.ToString();
            LevelManager.Instance.totalTurnsText.text = tilesTurned.ToString();
            LevelManager.Instance.scoreText.text = score.ToString();

            selectedTileControllers.Clear();
            currentGridController.CanButtonsInteract(true);
        }
    }
}