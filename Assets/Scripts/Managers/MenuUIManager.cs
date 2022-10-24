using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DG.Tweening.Plugins.Core.PathCore;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

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
    [SerializeField] private Canvas canvas;
        
    [SerializeField] private StarCurrencyIndicator starCurrencyIndicator;
    [SerializeField] private RectTransform starCurrencyTargetPos;
    [SerializeField] private DiamondCurrencyIndicator diamondCurrencyIndicator;
    [SerializeField] private RectTransform diamondCurrencyTargetPos;

    [SerializeField] private EventSystem eventSystem;
    private bool isCheckedForAnimation;
    
    private void Start()
    {
        isCheckedForAnimation = false;
        MakeCurrencyAnimation(LevelManager.instance.targetStarCount, LevelManager.instance.targetDiamondCount);
        
        LevelManager.instance.targetDiamondCount = 0;
        LevelManager.instance.targetStarCount = 0;
    }

    private void Update()
    {
        if (starCurrencyIndicator.isStarAnimEnded && diamondCurrencyIndicator.isDiamondAnimEnded && !isCheckedForAnimation)
        {
            eventSystem.enabled = true;
            isCheckedForAnimation = true;
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
    
    private void SetMaskState(bool playPanelIsOpen)
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
    
    private void TryToPlayLevel(bool isUnlocked, int levelIndex, Button button)
    {
        if (isUnlocked)
        {
            char backgroundLetter = button.transform.parent.transform.parent.name[0];
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
    
    private void SetLevelPanel(int levelIndex)
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
    
    private void MakeCurrencyAnimation(int starCount, int diamondAmount)
    {
        if (starCount>0 )
        {
            Vector3 startPosition = mainMenuMapBuilder.GetLCurrentevelButtonPosition();
            Vector3 endPosition = starCurrencyTargetPos.position;
            starCurrencyIndicator.StarAnimation(canvas.transform, startPosition, endPosition, starCount);
        }
        else
        {
            starCurrencyIndicator.SetText(PlayerPrefs.GetInt("Star"));
        }
                    
        if (diamondAmount >0)
        {
            eventSystem.enabled = false;
            Vector3 startPosition = mainMenuMapBuilder.GetLCurrentevelButtonPosition();
            Vector3 endPosition = diamondCurrencyTargetPos.position;
            diamondCurrencyIndicator.DiamondAnimation(canvas.transform,startPosition,endPosition,diamondAmount);
        }
    }
    
}