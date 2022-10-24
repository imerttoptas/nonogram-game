using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Compatibility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameUIManager : MonoBehaviour
{
    
    public InputManager inputManager;
    public GridManager gridManager;
    [SerializeField] GameObject gridBackground;
    [SerializeField] TextMeshProUGUI levelInfoText;
    [SerializeField] Image mask;
    
    #region LevelBackground
    [SerializeField] SpriteRenderer levelBackground;
    [SerializeField] Sprite[] gameSeceneBackgroundSprites;
    #endregion

    #region SettingsPanel
    bool isOpened;
    private bool soundOn = true, musicOn = true;
    [SerializeField] Button settingsButton;
    [SerializeField] CanvasGroup settingsPanel;
    [SerializeField] Image settingsButtonImage;
    [SerializeField] Image soundOff;
    [SerializeField] Image musicOff;
    [SerializeField] private Canvas settingsButtonCanvas;
    #endregion

    #region PowerupBar
    [SerializeField] Image[] PowerUpCountBackgroundImage;
    [SerializeField] Sprite GreenElipse;
    [SerializeField] private Canvas powerUpBarCanvas;
    [SerializeField] Button[] PowerUpButtons;
    [SerializeField] GameObject whiteBackground;
    Button selectedPowerUp;
    public bool isPowerupSelected;
    #endregion

    #region PowerupPurchasePanel
    [SerializeField] private Button buyPowerupButton;
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private TextMeshProUGUI buyPowerupButtonText;
    [SerializeField] Image powerupPurchasePanel ;
    [SerializeField] Image powerupImage;
    [SerializeField] Sprite[] powerupSprites;
    [SerializeField] TextMeshProUGUI powerupTypeText;
    [SerializeField] TextMeshProUGUI powerUpExplanationText;

    #endregion

    #region HealthBar
    [SerializeField] Image[] Hearts;
    [SerializeField] Sprite brokenHeart;
    #endregion

    #region WinPanel
    [SerializeField] Image winPanel;
    [SerializeField] Button continueButton;
    [SerializeField] Image[] stars;
    [SerializeField] ParticleSystem[] confettiParticles;
    #endregion
    
    #region LosePanel
    [SerializeField] Image losePanel;
    [SerializeField] TextMeshProUGUI losePanelLevelInfo;
    #endregion
    
    #region InputButton
    [SerializeField] Button inputButton;
    public Sprite squareImage;
    public Sprite crossImage;
    [SerializeField] Image circle;
    [SerializeField] Image[] inputImage;
    #endregion
    
    #region GridElements
    [SerializeField] TextMeshPro leftText; 
    [SerializeField] TextMeshPro upperText;
    #endregion
    
    private void Start()
    {
        SetSettingsButtons();
        SetGameSceneBackground(LevelManager.instance.levelBackgroundIndex);
        levelInfoText.text = "LEVEL" + (LevelManager.currentLevelIndex + 1);
        settingsButton.onClick.AddListener(() => FadeInSettingsPanel());
        isOpened = false;
        mask.gameObject.SetActive(false);
        ArrangeHearts();
        inputImage[1].gameObject.SetActive(false);
    }
    
    private void SetMaskState(bool isActive, Action onClickAction = null)
    {
        if (isActive)
        {
            mask.GetComponent<EventTrigger>().enabled = true;
            SetMaskClickAction(onClickAction);
            mask.gameObject.SetActive(true);
            mask.DOFade(0.5f, 0.15f).From(0f);
        }
        else
        {
            mask.GetComponent<EventTrigger>().enabled = false;
            mask.gameObject.SetActive(false);
            mask.SetAlpha(0f);
        }
    }
    
    private void SetMaskClickAction(Action action)
    {
        EventTrigger trigger = mask.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        trigger.triggers.Clear();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { action(); });
        trigger.triggers.Add(entry);
    }
    
    private void SetPowerUpButtonsSortingOrder(int sortingOrderValue)
    {
        foreach (Button button in PowerUpButtons)
        {
            button.GetComponent<Canvas>().sortingOrder = sortingOrderValue;
        }
    }
    
    private void FadeInSettingsPanel()
    {
        if (isOpened == false)
        {
            SetPowerUpButtonsSortingOrder(-1);
            gridBackground.GetComponent<SortingGroup>().sortingOrder = -1;
            powerUpBarCanvas.sortingOrder = -1; 
            settingsButtonCanvas.sortingOrder = 1;
            
            DOTween.Kill(settingsPanel.transform);
            
            SetMaskState(true, FadeOutSettingsPanel);
            settingsPanel.gameObject.SetActive(true);
            GameManager.instance.ChangeGameState(GameState.Pause);
            settingsPanel.transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutBack, 2f).From(0f);
            settingsButtonImage.transform.DORotate(new Vector3(0, 0, -90), 0.3f);
            isOpened = true;
            
        }
        else
        {
            settingsButtonImage.transform.DORotate(new Vector3(0, 0, 0), 0.3f);
            FadeOutSettingsPanel();
        }
    }

    private void FadeOutSettingsPanel()
    {
        settingsButtonCanvas.sortingOrder = -1;
        powerUpBarCanvas.sortingOrder = 1; 
        gridBackground.GetComponent<SortingGroup>().sortingOrder = 0;
        SetPowerUpButtonsSortingOrder(1);
        DOTween.Kill(settingsPanel.transform);
        GameManager.instance.ChangeGameState(GameState.Playing);
        settingsPanel.transform.DOScaleY(0f, 0.3f).OnComplete(() => settingsPanel.gameObject.SetActive(true));
        SetMaskState(false);
        isOpened = false;
    }
    
    public void ChangeInputType()
    {
        if (inputManager.currentInputType == InputType.Square)
        {
            inputManager.currentInputType = InputType.Cross;
            circle.transform.DOLocalMoveX(-100, 0.25f);
            inputImage[1].gameObject.SetActive(true);
            inputImage[0].gameObject.SetActive(false);
        }
        else
        {
            inputManager.currentInputType = InputType.Square;
            circle.transform.DOLocalMoveX(100, 0.25f);
            inputImage[0].gameObject.SetActive(true);
            inputImage[1].gameObject.SetActive(false);
        }
    }

    public void SelectPowerup(int powerUpType)
    {
        if (PowerupController.instance.GetPowerUpCount((PowerUpType)powerUpType) > 0)
        {
            if (isPowerupSelected == false)
            {
                FadeInPowerUpMask(powerUpType);
                switch (powerUpType)
                {
                    case 0:
                        selectedPowerUp = PowerUpButtons[0]; 
                        break;
                    case 1:
                        selectedPowerUp = PowerUpButtons[1];
                        break;
                    case 2:
                        selectedPowerUp = PowerUpButtons[2];
                        break;
                }
                
                isPowerupSelected = true;
                selectedPowerUp.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f).From(1f);
                
                PowerupController.instance.currentPowerUpType = (PowerUpType)powerUpType;
                
                if (GameManager.instance.currentGameState == GameState.Playing)
                {
                    GameManager.instance.AddGameState(GameState.PowerUpInUsage);
                }
            }
            else
            {
                selectedPowerUp.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
                if (GameManager.instance.currentGameState == GameState.Playing)
                {
                    GameManager.instance.RemoveGameState(GameState.PowerUpInUsage);
                }
                FadeOutPowerUpMask(powerUpType);
                isPowerupSelected = false;
            }
        }
        else
        {
            PowerupController.instance.currentPowerUpType = (PowerUpType)powerUpType;
            ActivatePowerUpPurchasePanel(powerUpType);
        }
    }

    private void ActivatePowerUpPurchasePanel(int powerUpType)
    {
        int cost = PowerupController.instance.GetPowerupCost();
        buyPowerupButton.interactable = CurrencyManager.instance.GetCurrencyItem(CurrencyItemType.Diamond).count >= cost;
        buyPowerupButtonText.text = "PURCHASE  <sprite index=0 >  " + cost;
        GameManager.instance.ChangeGameState(GameState.Pause);
        powerupPurchasePanel.gameObject.SetActive(true);
        powerupPurchasePanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).From(new Vector3(0, 0, 0));
        
        SetPowerUpPurchaseIcon(((PowerUpType)powerUpType));
        powerupTypeText.text = ((PowerUpType)powerUpType).ToString();
        FadeInPowerUpPurhcaseMask();
    }
    
    public void ExitPowerUpPurchasePanel()
    {
        powerupPurchasePanel.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).OnComplete(() => powerupPurchasePanel.gameObject.SetActive(false));
        GameManager.instance.ChangeGameState(GameState.Playing);
        FadeOutPowerUpPurhcaseMask();
    }
    
    private void FadeInPowerUpPurhcaseMask()
    {
        mask.gameObject.SetActive(true);
        mask.SetAlpha(0.5f);
        powerUpBarCanvas.sortingOrder = -1;
        foreach (Button button in PowerUpButtons)
        {
            button.GetComponent<Canvas>().sortingOrder = -1;
        }
        gridBackground.GetComponent<SortingGroup>().sortingOrder = -1;
    }
    
    private void FadeOutPowerUpPurhcaseMask()
    {
        mask.gameObject.SetActive(false);
        mask.SetAlpha(0f);
        gridBackground.GetComponent<SortingGroup>().sortingOrder = 0;
    }
    
    private void SetPowerUpPurchaseIcon(PowerUpType powerUpType)
    {
        powerupImage.sprite = powerupSprites[(int)powerUpType];
        switch (powerUpType)    
        {
            case PowerUpType.Rocket:
                powerupImage.rectTransform.sizeDelta = new Vector2(371, 542);
                powerupImage.rectTransform.eulerAngles = new Vector3(0, 0, -45);
                powerUpExplanationText.text = "Unlocks Vertical cells";
                break;
            case PowerUpType.Bomb:
                powerupImage.rectTransform.sizeDelta = new Vector2(443f, 545);
                powerupImage.rectTransform.eulerAngles = new Vector3(0, 0, 45);
                powerUpExplanationText.text = "Unlocks 3x3 area of cells";

                break;
            case PowerUpType.Fist:
                powerupImage.rectTransform.sizeDelta = new Vector2(475, 321);
                powerupImage.rectTransform.eulerAngles = new Vector3(0, 0, 45);
                powerUpExplanationText.text = "Unlocks Horizontal cells";
                break;
        }
    }
    
    public void BuyPowerUpCurrency()
    {
        int cost = PowerupController.instance.GetPowerupCost();
        if (CurrencyManager.instance.GetCurrencyItem(CurrencyItemType.Diamond).count >= cost)
        {
            CurrencyManager.instance.TryToDecreaseCurrencyCount(CurrencyItemType.Diamond,cost);
            CurrencyManager.instance.IncreaseCurrencyCount(PowerupController.instance.GetCurrencyItemTypeOfPowerUp(),3);
            buyPowerupButton.interactable = PlayerPrefs.GetInt("Diamond") >= cost;
        }
        else
        {
            buyPowerupButton.interactable = false;
        }
    }
    
    private void FadeInPowerUpMask(int index)
    {
        gridBackground.GetComponent<SortingGroup>().sortingOrder = 1;
        PowerUpButtons[index].GetComponent<Canvas>().sortingOrder = 2;
        settingsButtonCanvas.sortingOrder = -1;
        mask.gameObject.SetActive(true);
        whiteBackground.gameObject.SetActive(true);
        foreach (TextMeshPro text in gridManager.gridBuilder.rowTextList)
        {
            text.sortingOrder = 2;
        }
        foreach (TextMeshPro text in gridManager.gridBuilder.colTextList)
        {
            text.sortingOrder = 2;
        }
    }
    
    public void FadeOutPowerUpMask(int index)
    {
        gridBackground.GetComponent<SortingGroup>().sortingOrder = -1;
        powerUpBarCanvas.sortingOrder = -1;
        PowerUpButtons[index].GetComponent<Canvas>().sortingOrder = 1;
        whiteBackground.gameObject.SetActive(false);
        selectedPowerUp.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
        foreach (TextMeshPro text in gridManager.gridBuilder.rowTextList)
        {
            text.sortingOrder = 0;
        }
        foreach (TextMeshPro text in gridManager.gridBuilder.colTextList)
        {
            text.sortingOrder = 0;
        }
        if (GameManager.instance.currentGameState != GameState.Win)
        {
            mask.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < PowerUpButtons.Length; i++)
            {
                PowerUpButtons[i].GetComponent<Canvas>().sortingOrder = -1;
            }
        }
        GameManager.instance.RemoveGameState(GameState.PowerUpInUsage);
    }
    
    private void ArrangeHearts()
    {
        for (int i = 0; i < GameManager.instance.lives; i++)
        {
            Hearts[i].gameObject.SetActive(true);
        }
    }
    
    private void DestroyHeart(int amount)
    {
        int lives = GameManager.instance.lives;
        Hearts[lives].sprite = brokenHeart;
        Hearts[lives].transform.DOScale(new Vector3(1.5f, 1.5f, 1f), 0.7f);
        Hearts[lives].DOFade(0, 0.7f).OnComplete(() => Hearts[GameManager.instance.lives].gameObject.SetActive(false));
    }
    
    private void ActivateEndPanel(GameState state)
    {
        if (state == GameState.Win)
        {
            settingsButtonCanvas.sortingOrder = -1;
            powerUpBarCanvas.sortingOrder = -1;
            SetPowerUpButtonsSortingOrder(-1);
            mask.gameObject.SetActive(true);
            Extensions.SetAlpha(mask, 0f);
            LevelManager.instance.CalculateDiamondCountToIncrease(LevelManager.currentLevelIndex);
            Sequence mySequence = DOTween.Sequence();
            
            mySequence.Append(gridBackground.transform.DOScale(new Vector3(0f, 0f, 0f), 1f).From(1f).OnComplete(() => winPanel.gameObject.SetActive(true)));
            mySequence.Append(winPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).From(0f).OnComplete(() => mask.DOFade(0.5f, 0.15f).From(0f)));
            mySequence.PrependInterval(0.3f);
            
            for (int i = 0; i < confettiParticles.Length; i++)
            {
                ParticleSystem particleSystem = Instantiate(confettiParticles[i]);
                ParticleSystem.ShapeModule shapePos = particleSystem.shape;
                shapePos.position = new Vector3(Random.Range(-6, 6), Random.Range(-11, 11), 1f);
            }
            
            float delay = 0;
            for (int i = 0; i < GameManager.instance.lives; i++)
            {
                stars[i].gameObject.SetActive(true);
                
                mySequence.Append(stars[i].transform.DOScale(new Vector3(1.10f, 1.10f, 1f), 0.5f).From(0f));
                mySequence.Append(stars[i].transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
                mySequence.PrependInterval(delay);
                delay += 0.10f;
                
            }
        }
        else if (state == GameState.Lose)
        {
            powerUpBarCanvas.sortingOrder = -1;
            settingsButtonCanvas.sortingOrder = -1;
            SetPowerUpButtonsSortingOrder(-1);
            losePanelLevelInfo.text = "LEVEL " + (LevelManager.currentLevelIndex + 1);
            mask.gameObject.SetActive(true);
            mask.DOFade(0.5f, 0.15f).From(0f);
            losePanel.gameObject.SetActive(true);
            losePanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).From(0f);
            gridBackground.SetActive(false);
        }
    }

    
    public void ExitLevelPanel()
    {
        
        mask.gameObject.SetActive(false);
        if (winPanel.gameObject.activeSelf)
        {
            winPanel.gameObject.SetActive(false);
        }
        if (losePanel.gameObject.activeSelf)
        {
            losePanel.gameObject.SetActive(false);
        }
        LevelManager.instance.ChangeScene(0);
    }
    
    public void ContinueNextLevel()
    {
        mask.gameObject.SetActive(false);
        LevelManager.currentLevelIndex += 1;
        LevelManager.instance.ChangeScene(1);
    }
    
    public void TryAgainButton()
    {
        mask.gameObject.SetActive(false);
        LevelManager.instance.ChangeScene(1);
    }
    
    public void SoundButton()
    {
        if (soundOn)
        {
            soundOff.gameObject.SetActive(true);
            LevelManager.instance.UserData.isSoundOn = false;
            SoundManager.instance.audioSources[1].enabled = false;
            
            soundOn = false;
        }
        else
        {
            soundOff.gameObject.SetActive(false);
            LevelManager.instance.UserData.isSoundOn = true;
            SoundManager.instance.audioSources[1].enabled = true;

            soundOn = true;
        }
    }
    
    public void MusicButton()
    {
        if (musicOn)
        {
            musicOff.gameObject.SetActive(true);
            LevelManager.instance.UserData.isMusicOn = false;
            SoundManager.instance.audioSources[0].Pause();
            musicOn = false;
        }
        else
        {
            musicOff.gameObject.SetActive(false);
            LevelManager.instance.UserData.isMusicOn = true;
            SoundManager.instance.audioSources[0].Play();
            musicOn = true;

        }
    }
    
    public void ChangeScene(int buildIndex)
    {
        LevelManager.instance.ChangeScene(buildIndex);
    }
    
    public void SaveLevel()
    {
        LevelManager.instance.SaveLevelData();
    }

    private void SetGameSceneBackground(int mapIndex)
    {
       levelBackground.sprite = gameSeceneBackgroundSprites[mapIndex];
    }

    private void SetSettingsButtons()
    {
        soundOff.gameObject.SetActive(!LevelManager.instance.UserData.isSoundOn);

        musicOff.gameObject.SetActive(!LevelManager.instance.UserData.isMusicOn);
    }
    
    private void OnEnable()
    {
        GameManager.instance.OnLifeChanged += DestroyHeart;
        GameManager.instance.OnGameStateChanged += ActivateEndPanel;
    }

    private void OnDisable()
    {
        GameManager.instance.OnLifeChanged -= DestroyHeart;
        GameManager.instance.OnGameStateChanged += ActivateEndPanel;
    }

    
}
