using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] SpriteRenderer levelBackground;
    [SerializeField] Sprite[] gameSeceneBackgroundSprites;
    
    [SerializeField] TextMeshProUGUI levelInfoText;
    [SerializeField] Image mask;
    public AudioSource winSound;

    public int levelBackgroundIndex;
    #region SettingsPanel

    bool isOpened;
    bool soundOn = true, musicOn = true, vibrationOn = true;
    [SerializeField] Button settingsButton;
    [SerializeField] CanvasGroup settingsPanel;
    [SerializeField] Image settingsButtonImage;
    [SerializeField] Image soundOff;
    [SerializeField] Image musicOff;
    [SerializeField] Image vibrationOff;
    [SerializeField] private Canvas settingsButtonCanvas;
    #endregion

    #region PowerupBar
    [SerializeField] Image[] PowerUpCountBackgroundImage;
    [SerializeField] Sprite GreenElipse;
    [SerializeField] private Canvas powerUpBarCanvas;
    [SerializeField] Button[] PowerUpButtons;

    [SerializeField] GameObject whiteBackground;
    [SerializeField] Button rocketPowerUp;
    [SerializeField] Button bombPowerUp;
    [SerializeField] Button fistPowerUp;
    Button selectedPowerUp;
    public bool isPowerupSelected;
    #endregion

    #region PowerupPanel
    [SerializeField] Image powerupPanel;
    [SerializeField] Image powerupImages;
    [SerializeField] Sprite[] powerupSprites;
    [SerializeField] TextMeshProUGUI powerupInfoText;
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
        SetSettingsButton();
        SetGameSceneBackground(LevelManager.instance.levelBackgroundIndex);
        levelInfoText.text = "LEVEL" + (LevelManager.currentLevelIndex+1).ToString();
        settingsButton.onClick.AddListener(() => FadeInSettingsPanel());
        isOpened = false;
        mask.gameObject.SetActive(false);
        ArrangeHearts();
        powerUpBarCanvas.sortingOrder = 1;

        inputImage[1].gameObject.SetActive(false);
    }

    public void SetMaskState(bool isActive, Action onClickAction = null)
    {
        if (isActive)
        {
            SetMaskClickAction(onClickAction);
            mask.gameObject.SetActive(true);
            mask.DOFade(0.5f, 0.15f).From(0f);
        }
        else
        {
            mask.gameObject.SetActive(false);
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

    public void FadeInSettingsPanel()
    {
        if (isOpened == false)
        {
            
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
            settingsButtonCanvas.sortingOrder = -1;
            powerUpBarCanvas.sortingOrder = 1;  //
            gridBackground.GetComponent<SortingGroup>().sortingOrder = 0;

          
        }
    }

    public void FadeOutSettingsPanel()
    {
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
        if (PowerupController.instance.GetPowerupCount((PowerUpType)powerUpType) > 0)
        {
            if (isPowerupSelected == false)
            {
                FadeInPowerUpMask(powerUpType);
                switch (powerUpType)
                {
                    case 0:
                        selectedPowerUp = rocketPowerUp;
                        break;
                    case 1:
                        selectedPowerUp = bombPowerUp;
                        break;
                    case 2:
                        selectedPowerUp = fistPowerUp;
                        break;
                    default:
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
            ActivatePowerUpPanel(powerUpType);
        }
    }
    public void ActivatePowerUpPanel(int powerUpType)
    {
        //mask ayarları ve animasyon
        powerupPanel.gameObject.SetActive(true);
        powerupImages.sprite = powerupSprites[powerUpType];
        powerupInfoText.text = ((PowerUpType)powerUpType).ToString();
        GameManager.instance.ChangeGameState(GameState.Pause);
    }
    public void ExitePowerUpPanel()
    {
        powerupPanel.gameObject.SetActive(false);
        GameManager.instance.ChangeGameState(GameState.Playing);
    }

    public void BuyPowerupCurrency(int powerUpType)
    {
        int cost = PowerupController.instance.GetPowerupCost((PowerUpType)powerUpType);
        
        if (LevelManager.instance.UserData.diamonds > cost)
        {
            //LevelManager.instance.UserData.
        }
    }

    public void FadeInPowerUpMask(int index)
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

    }

    public void ArrangeHearts()
    {
        for (int i = 0; i < GameManager.instance.lives; i++)
        {
            Hearts[i].gameObject.SetActive(true);
        }
    }

    public void DestroyHeart(int amount)
    {
        Hearts[GameManager.instance.lives].sprite = brokenHeart;
        Hearts[GameManager.instance.lives].transform.DOScale(new Vector3(1.5f, 1.5f, 1f), 0.7f);
        Hearts[GameManager.instance.lives].DOFade(0, 0.7f).OnComplete(() => Hearts[GameManager.instance.lives].gameObject.SetActive(false));

    }

    public void ActivateEndPanel(GameState state)
    {

        if (state == GameState.Win)
        {

            settingsButtonCanvas.sortingOrder = -1;
            powerUpBarCanvas.sortingOrder = -1;
            mask.gameObject.SetActive(true);
            Extensions.SetAlpha(mask, 0f);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(gridBackground.transform.DOScale(new Vector3(0f, 0f, 0f), 1f).From(1f).OnComplete(() => winPanel.gameObject.SetActive(true)));
            mySequence.Append(winPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).From(0f).OnComplete(() => mask.DOFade(0.5f, 0.15f).From(0f)));
            mySequence.PrependInterval(1f);
            
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
                delay += 0.25f;

            }
        }
        else if (state == GameState.Lose)
        {
            powerUpBarCanvas.sortingOrder = -1;
            settingsButtonCanvas.sortingOrder = -1;
            losePanelLevelInfo.text = "LEVEL " + (LevelManager.currentLevelIndex + 1);
            mask.gameObject.SetActive(true);
            mask.DOFade(0.5f, 0.15f).From(0f);
            losePanel.gameObject.SetActive(true);
            losePanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).From(0f);
            gridBackground.SetActive(false);
        }
    }

    public void WaitFunction()
    {
        powerUpBarCanvas.sortingOrder = -1;
    }

    public void ExitLevelPanel()
    {
        
        mask.gameObject.SetActive(false);

        if (winPanel.gameObject.activeSelf == true)
        {
            winPanel.gameObject.SetActive(false);
        }
        if (losePanel.gameObject.activeSelf == true)
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

    public void VibrateButton()
    {
        if (vibrationOn)
        {
            vibrationOff.gameObject.SetActive(true);
            LevelManager.instance.UserData.isVibrationOn = false;
            vibrationOn = false;

        }
        else
        {
            vibrationOff.gameObject.SetActive(false);
            LevelManager.instance.UserData.isVibrationOn = true;
            vibrationOn = true;

        }
    }

    public void ChangeScene(int buildindex)
    {
        LevelManager.instance.ChangeScene(buildindex);
    }

    public void SaveLevel()
    {
        LevelManager.instance.SaveLevelData();
    }

    public void SetGameSceneBackground(int mapIndex)
    {
       levelBackground.sprite = gameSeceneBackgroundSprites[mapIndex];
    }

    public void SetSettingsButton()
    {

        if (LevelManager.instance.UserData.isSoundOn)
        {
            soundOff.gameObject.SetActive(false);

        }
        else
        {
            soundOff.gameObject.SetActive(true);
        }

        if (LevelManager.instance.UserData.isMusicOn)
        {
            musicOff.gameObject.SetActive(false);

        }
        else 
        {
            musicOff.gameObject.SetActive(true);
        }

        if (LevelManager.instance.UserData.isVibrationOn)
        {
            vibrationOff.gameObject.SetActive(false);
        }
        else
        {
            vibrationOff.gameObject.SetActive(true);
        }
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
