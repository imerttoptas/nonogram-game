using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public GameState currentGameState;
    public GridManager gridManager;
    public LevelData currentLevelData;
    public UserData userData;
    
    public System.Action<GameState> OnGameStateChanged;
    public System.Action<int> OnLifeChanged;
    
    public int lives;
    
    [SerializeField] private EventSystem eventSystem;
    public void Initialize()
    {
        userData = LevelManager.instance.UserData;
        currentGameState = GameState.Playing;
        currentLevelData = userData.levelDataList[LevelManager.currentLevelIndex];
        LevelManager.instance.LoadLevel();
    }
  
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            LevelManager.instance.SaveLevelData();
        }
    }

    private void OnApplicationQuit()
    {
        if (!(currentGameState == GameState.Win || currentGameState == GameState.Lose))
        {  
            LevelManager.instance.SaveLevelData();
        }
    }

    public void SetGridManager(GridManager _gridManager)
    {
        this.gridManager = _gridManager;
    }

    public GridManager GetGridManager()
    {
        return gridManager;
    }

    public void ChangeGameState(GameState state)
    {
        if (currentGameState != state)
        {
            currentGameState = state;
            OnGameStateChanged?.Invoke(currentGameState);
        }
    }

    public void AddGameState(GameState state)
    {
        if (!currentGameState.Contains(state))
        {
            currentGameState |= state;
            OnGameStateChanged?.Invoke(currentGameState);
        }
    }

    public void RemoveGameState(GameState state)
    {
        if (currentGameState.Contains(state))
        {
            currentGameState &= ~state;
            OnGameStateChanged?.Invoke(currentGameState);
        }
    }

    public void DecreaseLife()
    {
        if (lives>0)
        {
            lives--;
        }
        OnLifeChanged?.Invoke(lives);
    }

    private void CheckLifeCount(int lifeCount)
    {
        if (lifeCount == 0)
        {
            ChangeGameState(GameState.Lose);
        }
    }

   
    public void DisableInput()
    {
        eventSystem.enabled = false;
    }
    
    public void EnableInput()
    {
        eventSystem.enabled = true;
    }
    
    private void OnEnable()
    {
        OnLifeChanged += CheckLifeCount;
        OnGameStateChanged += LevelManager.instance.SaveGame;
        OnGameStateChanged += SoundManager.instance.PlayEndPanelSound;
    }

    private void OnDisable()
    {
        OnLifeChanged -= CheckLifeCount;
        OnGameStateChanged -= LevelManager.instance.SaveGame;
        OnGameStateChanged -= SoundManager.instance.PlayEndPanelSound;
    }



}


