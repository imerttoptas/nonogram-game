using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public bool unlocked = false;
    public int stars;
    public int levelIndex;
    public List<CellData> cellDataList;
    public int lifeLeft;
    public LevelData(int gridSize)
    {
        cellDataList = new List<CellData>(gridSize * gridSize);
        lifeLeft = 3;
    }
    public void DeleteLevelData()
    {
        stars = LevelManager.instance.UserData.levelDataList[LevelManager.currentLevelIndex].stars;
        lifeLeft = 3;
        cellDataList = new List<CellData>(GameManager.instance.GetGridManager().gridSize * GameManager.instance.GetGridManager().gridSize);
    }
}