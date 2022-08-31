using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Rocket : Powerup
{
    // gameObject
    [SerializeField] GameObject rocketPrefab;
    public override void Use(Cell cell)
    {
        //CurrencyManager.instance.TryToDecreaseCurrencyCount(CurrencyItemType.Rocket, 1);
        LevelManager.instance.UserData.rocketPowerupCount -= 1;
        int gridSize = GameManager.instance.GetGridManager().gridSize;
        
        
        Vector3 defaultPowerupScale = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 powerupScaler = defaultPowerupScale / (gridSize / 5);
        float defaultPowerUpSpeed = 1f;
        float powerUpSpeeder = defaultPowerUpSpeed * (gridSize / 5);
        GameObject createdRocketPrefab = Instantiate(rocketPrefab);
        createdRocketPrefab.transform.position = new Vector2(cell.transform.position.x, -30);
        createdRocketPrefab.transform.localScale = powerupScaler;
        createdRocketPrefab.transform.DOMoveY(30f, powerUpSpeeder);
        
        gridManager.CompleteColumn(cell);
        List<Cell> columnCellList = gridManager.GetColumn(cell.column);
        for (int i = 0; i < columnCellList.Count; i++)
        {
            if (columnCellList[i].currentCellState == CellState.Square)
            {
                gridManager.TryToFillRow(columnCellList[i]);
            }
            gridManager.CheckText(columnCellList[i]);
        }
        gridManager.CheckGrid(cell);
    }
}