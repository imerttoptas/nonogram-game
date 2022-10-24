using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    
    public int lastLevelReached;
    public int lastLevelPlayed;
    
    public bool isMusicOn;
    public bool isSoundOn;
    
    public Dictionary<CurrencyItemType, CurrencyItem> currencyItemDictionary;

    
    
    public List<LevelData> levelDataList;

    public UserData()
    {
        lastLevelPlayed = 0;
        lastLevelReached = 0;
        levelDataList = new List<LevelData>();
        levelDataList.Add(new LevelData(5));
        isMusicOn = false;
        isSoundOn = false;
    }
   
    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex <= lastLevelReached;
    }
}