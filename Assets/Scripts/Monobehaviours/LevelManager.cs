using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Transform gridHolder;
    public Button homeButton;
    public GameObject endMessage;
    public TextMeshProUGUI totalTurnsText, tilesMatchedText, scoreText;

    [Header("<----- List ----->")]
    public List<GameObject> grids = new();

    private GameController m_GameController;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_GameController = GameController.Instance;
        SubscribeToButton();
        SetGridBasedOnDifficulty();
    }

    public void SubscribeToButton()
    {
        homeButton.onClick.AddListener(delegate { OnHomeButtonClicked(); });
    }

    private void OnHomeButtonClicked()
    {
        m_GameController.OnButtonClickSound();
        m_GameController.LoadScene(1);
    }

    public void SetGridBasedOnDifficulty()
    {
        SpawnGrid(m_GameController.difficultyIndex);
    }

    private void SpawnGrid(int gridIndex)
    {
        var grid = Instantiate(grids[gridIndex], gridHolder);
        m_GameController.currentGridController = grid.GetComponent<GridController>();
    }
}
