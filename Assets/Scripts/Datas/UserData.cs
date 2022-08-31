﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public int totalStar;
    public int diamonds;
    
    public int lastLevelReached;
    public int lastLevelPlayed;
    
    public bool isMusicOn;
    public bool isSoundOn;
    public bool isVibrationOn;

    public int rocketPowerupCount;
    public int bombPowerupCount;
    public int fistPowerupCount;



    public List<LevelData> levelDataList;


    public UserData()
    {
        lastLevelReached = 0;
        levelDataList = new List<LevelData>();
        levelDataList.Add(new LevelData(5));
        isMusicOn = true;
        isSoundOn = true;
        isVibrationOn = true;
        diamonds = 100;
        rocketPowerupCount = 3;
        bombPowerupCount = 3;
        fistPowerupCount = 3;
        
    }
    public UserData(int LastLevelReached)
    {
        lastLevelReached = LastLevelReached;
    }
    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex <= lastLevelReached;
    }
}