using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public System.Action<int> OnSceneChanged;
    public static int currentLevelIndex;
    private UserData userData;
    public int levelBackgroundIndex;
    
    public UserData UserData => userData;
    
    private static int starCount;
    public int targetStarCount;
    private static int diamondCount;
    public int targetDiamondCount;
    

    private void Start()
    {
        if (PlayerPrefs.HasKey("UserData"))
        {
            LoadUserData();
        }
        else
        {
            CreateUserData();
        }
        
        CalculateInitalCurrency();
        
    }
    
    public void ChangeScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
        OnSceneChanged?.Invoke(buildIndex);
    }

    private void CreateUserData()
    {
        userData = new UserData();
    }

    private void SaveUserData()
    {
        string userDataStringToSave = JsonUtility.ToJson(userData, true);
        userData.levelDataList[0].unlocked = true;
        PlayerPrefs.SetString("UserData", userDataStringToSave);
        PlayerPrefs.Save();
    }
    
    public void SaveLevelData()
    {
        LevelData levelData = userData.levelDataList[currentLevelIndex];
        levelData.cellDataList = GameManager.instance.GetGridManager().GetCellDataList();
        levelData.lifeLeft = GameManager.instance.lives;
        levelData.unlocked = true;
        string userDataStringToSave = JsonUtility.ToJson(userData, true);
        PlayerPrefs.SetString("UserData", userDataStringToSave);
        PlayerPrefs.Save();
        
    }

    private void SaveStars(int star)
    {
        Debug.Log("Save Stars");
        if (star > userData.levelDataList[currentLevelIndex].stars)
        {
            targetStarCount += star - userData.levelDataList[currentLevelIndex].stars;
            Debug.Log("targetStarCount : " + targetStarCount);
            userData.levelDataList[currentLevelIndex].stars = star;
        }
    }
    
    private void LoadUserData()
    {
        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString("UserData"));
    }

    public void LoadLevel()
    {
        LevelData levelData = userData.levelDataList[currentLevelIndex];
        GameManager.instance.GetGridManager().SetCellDataList(levelData.cellDataList);
        GameManager.instance.lives = levelData.lifeLeft;
        GameManager.instance.GetGridManager().CheckLoadedLevelText();
    }

    private void DeleteLevel()
    {
        LevelData levelData = userData.levelDataList[currentLevelIndex];
        levelData.DeleteLevelData();
        userData.levelDataList[currentLevelIndex] = levelData;
    }
    
    public void SaveGame(GameState state)
    {
        if (state == GameState.Win)
        {   
            
            // CalculateDiamondCountToIncrease(currentLevelIndex);
            userData.lastLevelPlayed = currentLevelIndex + 1;
            SaveStars(GameManager.instance.lives);
            if (currentLevelIndex >= userData.lastLevelReached)
            {
                userData.lastLevelReached += 1;
                userData.levelDataList.Add(new LevelData(GameManager.instance.GetGridManager().gridSize));
            }
            DeleteLevel();
        }
        if (state == GameState.Lose)
        {
            userData.lastLevelPlayed = currentLevelIndex + 1;
            DeleteLevel();
        }
    }
    
    private void CalculateInitalCurrency()
    {
        targetStarCount = 0;
        starCount = PlayerPrefs.GetInt("Star");
        diamondCount = PlayerPrefs.GetInt("Diamond");
    }
        
    public int CalculateDiamondCountToIncrease(int levelIndex)
    {
        if (levelIndex >= userData.lastLevelReached)
        {
            if (levelIndex % 6 <=3)
            {
                targetDiamondCount += 250;
            }
            else
            {
                targetDiamondCount += 750;
            }
        }
        else
        {
            if (levelIndex % 6 <=3)
            {
                targetDiamondCount += 100;
            }
            else
            {
                targetDiamondCount += 250;
            }
        }

        return targetDiamondCount;
    }
            
    private void ArangeStarCount(int sceneIndex)
    {
        if (sceneIndex == 0)
        {
            starCount = PlayerPrefs.GetInt("Star");
        }
    }
    
        
    private void OnEnable()
    {
        OnSceneChanged += ArangeStarCount;
    }

    private void OnDisable()
    {
        OnSceneChanged -= ArangeStarCount;
    }

    private void OnApplicationQuit()
    {
        CurrencyManager.instance.IncreaseCurrencyCount(CurrencyItemType.Star,targetStarCount);
        CurrencyManager.instance.IncreaseCurrencyCount(CurrencyItemType.Diamond,targetDiamondCount);
        SaveUserData();
    }
}