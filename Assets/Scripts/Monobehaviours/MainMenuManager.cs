using System.Collections;
using System.Collections.Generic;

using TMPro;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Image audioButtonImage;
    public TMP_Dropdown difficultyDropdown;
    public bool canPlayAudio = true;

    [Header("<----- Lists ----->")]
    public List<Button> buttons;
    public List<Sprite> audioToggleSprites = new();

    private GameController m_GameController;

    private void Awake()
    {
        SubscribeMethodsButtons();
    }

    private void Start()
    {
        m_GameController = GameController.Instance;
        difficultyDropdown.value = m_GameController.difficultyIndex;
    }

    private void SubscribeMethodsButtons()
    {
        if (buttons.Count > 0)
        {
            buttons[0].onClick.AddListener(delegate { OnPlayClicked(); });
            buttons[1].onClick.AddListener(delegate { OnAudioToggle(); });
        }

        difficultyDropdown.onValueChanged.AddListener(delegate { OnDropdownValueChange(); });
    }

    private void OnPlayClicked()
    {
        m_GameController.OnButtonClickSound();
        m_GameController.LoadScene(2);
    }

    private void OnAudioToggle()
    {
        canPlayAudio = !canPlayAudio;
        audioButtonImage.sprite = canPlayAudio ? audioToggleSprites[0] : audioToggleSprites[1];
        m_GameController.OnToggleAudioSources(canPlayAudio);

        if (canPlayAudio)
        {
            m_GameController.OnButtonClickSound();
        }
    }

    private void OnDropdownValueChange()
    {
        m_GameController.difficultyIndex = difficultyDropdown.value;
    }
}
