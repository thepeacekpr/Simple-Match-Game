using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TileController : MonoBehaviour
{
    public GameObject farmerIconHolder, fruitIconHolder;
    public FruitVariety fruitVariety;
    public Button farmerIconButton;

    [SerializeField] private Image fruitIcon;
    [SerializeField] private float lerpTime = 5.0f;
    private GameController m_GameController;

    private void Awake()
    {
        farmerIconButton = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        m_GameController = GameController.Instance;
        farmerIconButton.onClick.AddListener(delegate { ForwardFlip(); });
        SetFruitIcon();
    }

    public void ForwardFlip()
    {
        StopAllCoroutines();
        m_GameController.PlaySoundEffects(3);
        m_GameController.selectedTileControllers.Add(this);

        StartCoroutine(FlipTile(true, (isDone) =>
        {
            farmerIconHolder.SetActive(false);
            fruitIconHolder.SetActive(true);
            m_GameController.CheckTileSelection();
        }));
    }

    public void ReverseFlip()
    {
        StopAllCoroutines();
        m_GameController.PlaySoundEffects(3);
        StartCoroutine(FlipTile(false, (isDone) =>
        {
            farmerIconHolder.SetActive(true);
            fruitIconHolder.SetActive(false);
        }));
    }

    public void SetFruitIcon() => fruitIcon.sprite = m_GameController.tileSprites[(int)fruitVariety];

    public IEnumerator FlipTile(bool showFruit, Action<bool> isDone)
    {
        Vector3 rotation = Vector3.zero;
        if (showFruit)
        {
            rotation = new Vector3(0.0f, 90.0f, 0.0f);
            while (transform.localEulerAngles.y < rotation.y)
            {
                transform.Rotate(lerpTime * Time.deltaTime * rotation, Space.Self);
                yield return null;
            }
            transform.localEulerAngles = rotation;
        }
        else
        {
            rotation = new Vector3(0.0f, 180.0f, 0.0f);
            while (transform.localEulerAngles.y > rotation.y)
            {
                transform.Rotate(lerpTime * Time.deltaTime * rotation, Space.Self);
                yield return null;
            }
            transform.localEulerAngles = Vector3.zero;
        }

        isDone(true);
    }
}
