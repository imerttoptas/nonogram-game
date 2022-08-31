using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public System.Action<int> OnSceneChanged;
    public static int currentLevelIndex;
    private UserData userData;
    public int levelBackgroundIndex;
    public UserData UserData => userData;

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
    }

    public void ChangeScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
        OnSceneChanged?.Invoke(buildIndex);
    }
    public void CreateUserData()
    {
        userData = new UserData();
    }
    public void SaveUserData()
    {
        string userDataStringToSave = JsonUtility.ToJson(userData, true);
        userData.levelDataList[0].unlocked = true;
        PlayerPrefs.SetString("UserData", userDataStringToSave);
        PlayerPrefs.Save();
    }

    public void SaveLevelData()
    {
        Debug.Log("save level data çalıştı");
        LevelData levelData = userData.levelDataList[currentLevelIndex];
        levelData.cellDataList = GameManager.instance.GetGridManager().GetCellDataList();
        levelData.lifeLeft = GameManager.instance.lives;
        levelData.unlocked = true;
        string userDataStringToSave = JsonUtility.ToJson(userData, true);
        PlayerPrefs.SetString("UserData", userDataStringToSave);
        PlayerPrefs.Save();
    }

    public void SaveStars(int star)
    {
        if (star > userData.levelDataList[currentLevelIndex].stars)
        {
            userData.totalStar += (star - userData.levelDataList[currentLevelIndex].stars);
            userData.levelDataList[currentLevelIndex].stars = star;
        }
    }

    public void LoadUserData()
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

    public void DeleteLevel()
    {
        LevelData levelData = userData.levelDataList[currentLevelIndex];
        levelData.DeleteLevelData();
        userData.levelDataList[currentLevelIndex] = levelData;
    }

    public void SaveGame(GameState state)
    {
        if (state == GameState.Win)
        {
            LevelData levelData = userData.levelDataList[currentLevelIndex];
            SaveStars(GameManager.instance.lives);
            if (currentLevelIndex >= userData.lastLevelReached)
            {
                userData.diamonds += 50;
                userData.lastLevelReached += 1;
                userData.levelDataList.Add(new LevelData(GameManager.instance.GetGridManager().gridSize));
            }
            DeleteLevel();
        }
        if (state == GameState.Lose)
        {
            DeleteLevel();
            //CurrencyManager.instance.TryToDecreaseCurrencyCount(CurrencyItemType.Diamond, 20);
        }
    }

    private void OnApplicationQuit()
    {
        SaveUserData();
    }
}