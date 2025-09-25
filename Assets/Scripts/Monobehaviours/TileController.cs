using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    public GameObject farmerIconHolder, fruitIconHolder;
    public Image fruitIcon;
    private GameController m_GameController;

    private void Start()
    {
        m_GameController = GameController.Instance;
        SwitchIcons();
    }

    private void SwitchIcons()
    {
        farmerIconHolder.SetActive(false);
        fruitIconHolder.SetActive(true);
    }

    public void SetFruitIcon(Sprite sprite)
    {
        fruitIcon.sprite = sprite;
    }
}
