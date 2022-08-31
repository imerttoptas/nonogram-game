using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    #region PlayLevelPanel
    bool playPanelIsOpen;
    [SerializeField] Image playLevelPanel;
    [SerializeField] TextMeshProUGUI levelInfoText;
    [SerializeField] Image[] stars = new Image[3];
    [SerializeField] Image mask;
    #endregion


    List<PoolItem> buttonPoolItemList;


    int levelBackgroundIndex;
    int levelToGo;

    [SerializeField] MainMenuMapBuilder mainMenuMapBuilder;

    private void Start()
    {
        foreach (Map map in mainMenuMapBuilder.GetMaps())
        {
            foreach (PoolItem poolItem in map.GetButtonList())
            {
                if (LevelManager.instance.UserData.lastLevelReached >= poolItem.GetComponent<PlayLevelButton>().indexLevel)
                {
                    //poolItem.GetComponent<PlayLevelButton>().SetLevelButton();
                }
            }
        }
    }

    public void InitializeButtons(List<PoolItem> buttonList, int buttonCount)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            PlayLevelButton playLevelButton = buttonList[i].GetComponent<PlayLevelButton>();
            int levelIndex = buttonCount + i;
            playLevelButton.indexLevel = buttonCount + i;
            playLevelButton.initalize();
            playLevelButton.SetLevelButton();
            playLevelButton.levelButton.onClick.AddListener(() => TryToPlayLevel(LevelManager.instance.UserData.IsLevelUnlocked(levelIndex), levelIndex, playLevelButton.gameObject.GetComponent<Button>()));
        }
    }

    public void SetMaskState(bool playPanelIsOpen)
    {
        if (playPanelIsOpen)
        {
            mask.gameObject.SetActive(true);
            mask.DOFade(0.5f, 0.15f).From(0f);
        }
        else
        {
            mask.gameObject.SetActive(false);
        }
    }

    public void TryToPlayLevel(bool isUnlocked, int levelIndex, Button button)
    {
        if (isUnlocked)
        {
            char backgroundLetter = button.transform.parent.transform.parent.name.ToString()[0];
            switch (backgroundLetter)
            {
                case 'B':
                    levelBackgroundIndex =  0;
                    break;
                case 'F':
                    levelBackgroundIndex =  1;
                    break;
                case 'S':
                    levelBackgroundIndex =  2;
                    break;
            }

            for (int i = 0; i < 3; i++)
            {
                stars[i].gameObject.SetActive(false);
            }
            SetLevelPanel(levelIndex);
        }
    }

    void SetLevelPanel(int levelIndex)
    {
        float delay = 0;
        for (int i = 0; i < LevelManager.instance.UserData.levelDataList[levelIndex].stars; i++)
        {
            stars[i].gameObject.SetActive(true);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(stars[i].transform.DOScale(new Vector3(1.10f, 1.10f, 1f), 0.5f).From(0f));
            mySequence.Append(stars[i].transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
            mySequence.PrependInterval(delay);
            delay += 0.25f;
        }

        playLevelPanel.gameObject.SetActive(true);
        playLevelPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).From(0f);

        playPanelIsOpen = true;
        SetMaskState(playPanelIsOpen);

        levelInfoText.text = "LEVEL " + (levelIndex + 1).ToString();
        levelToGo = levelIndex;
    }

    public void ExitLevelPanel()
    {
        mask.gameObject.SetActive(false);
        playLevelPanel.gameObject.SetActive(false);
    }

    public void PlayLevel()
    {
        LevelManager.currentLevelIndex = levelToGo;
        playLevelPanel.gameObject.SetActive(false);
        LevelManager.instance.ChangeScene(1);
        LevelManager.instance.levelBackgroundIndex = levelBackgroundIndex;
    }

}