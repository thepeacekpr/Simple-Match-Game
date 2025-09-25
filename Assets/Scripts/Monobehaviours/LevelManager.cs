using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridHolder;
    public GridLayoutGroup gridLayoutGroup;
    public Button homeButton;

    [Header("<----- List ----->")]
    public List<GameObject> tiles = new();

    private GameController m_GameController;

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
        Vector2 spacing = Vector2.zero;

        switch (m_GameController.difficultyIndex)
        {
            case 0:
                spacing = new Vector2(25,25);
                PopulateTiles(9, 300, spacing);
                break;
            case 1:
                spacing = new Vector2(40,40);
                PopulateTiles(12, 250, spacing);
                break;
            case 2:
                spacing = new Vector2(30,30);
                PopulateTiles(15, 200, spacing);
                break;
            case 3:
                spacing = new Vector2(174, 10);
                PopulateTiles(18, 150, spacing);
                break;
            case 4:
                spacing = new Vector2(10, 100);
                PopulateTiles(21, 150, spacing);
                break;
        }
    }

    private void PopulateTiles(int tileCount, float cellSize, Vector2 spacing)
    {
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        gridLayoutGroup.spacing = spacing;

        for (int i = 0; i < tileCount; i++)
        {
            var tile = Instantiate(tilePrefab, gridHolder);
            tile.name = $"Tile ({i})";
            var tileController = tile.GetComponent<TileController>();
            tileController.SetFruitIcon(m_GameController.tileSprites[0]);
            tiles.Add(tile);
        }
    }
}
