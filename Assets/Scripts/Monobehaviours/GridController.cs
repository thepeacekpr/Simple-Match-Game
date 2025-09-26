using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    public List<TileController> tileControllers = new();

    private GridLayoutGroup m_GridLayoutGroup;
    private GameController m_GameController;

    private void Awake()
    {
        m_GridLayoutGroup = GetComponent<GridLayoutGroup>();
        GetChildren();
    }

    private void Start()
    {
        m_GameController = GameController.Instance;
        ShuffleGrid();
    }

    private void GetChildren()
    {
        TileController[] tileControllers = GetComponentsInChildren<TileController>();
        if (tileControllers.Length > 0)
        {
            foreach (var tileController in tileControllers)
            {
                this.tileControllers.Add(tileController);
            }
        }
    }

    private void ShuffleGrid()
    {
        if (tileControllers.Count > 0)
        {
            foreach (var tile in tileControllers)
            {
                tile.transform.SetSiblingIndex(Random.Range(0, tileControllers.Count - 1));
            }
        }

        StartCoroutine(DisableGridLayout());
    }

    private IEnumerator DisableGridLayout()
    {
        if (tileControllers.Count > 0)
        {
            foreach (var tile in tileControllers)
            {
                CanButtonsInteract(false);
                m_GameController.PlaySoundEffects(3);
                yield return StartCoroutine(tile.FlipTile(true, (isDone) =>
                    {
                        tile.farmerIconHolder.SetActive(false);
                        tile.fruitIconHolder.SetActive(true);
                    }));
            }

            yield return new WaitForSecondsRealtime(1.5f);

            foreach (var tile in tileControllers)
            {
                m_GameController.PlaySoundEffects(3);
                yield return StartCoroutine(tile.FlipTile(false, (isDone) =>
                {
                    tile.farmerIconHolder.SetActive(true);
                    tile.fruitIconHolder.SetActive(false);
                    CanButtonsInteract(true);
                }));
            }
        }

        yield return new WaitForEndOfFrame();
        m_GridLayoutGroup.enabled = false;
    }

    public void CanButtonsInteract(bool canInteract)
    {
        if (tileControllers.Count > 0)
        {
            foreach (var tile in tileControllers)
            {
                tile.farmerIconButton.interactable = canInteract;
            }
        }
    }

    public bool CheckGridIsEmpty()
    {
        return tileControllers.All(go => !go.gameObject.activeInHierarchy);
    }
}
