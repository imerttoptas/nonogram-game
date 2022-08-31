using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Fist : Powerup
{
    [SerializeField] GameObject fistPrefab;
    public override void Use(Cell cell)
    {
        CurrencyManager.instance.TryToDecreaseCurrencyCount(CurrencyItemType.Fist, 1);
        LevelManager.instance.UserData.fistPowerupCount -= 1;
        int gridSize = GameManager.instance.GetGridManager().gridSize;
        Vector3 defaultPowerupScale = new Vector3(0.25f, 0.25f, 0.25f);
        Vector3 powerupScaler = defaultPowerupScale / (gridSize / 5);
        float defaultPowerUpSpeed = 1f;
        float powerUpSpeeder = defaultPowerUpSpeed * (gridSize / 5);
        GameObject createdFistPrefab = Instantiate(fistPrefab);
        createdFistPrefab.transform.position = new Vector2(15, cell.transform.position.y);
        createdFistPrefab.transform.localScale = powerupScaler;
        createdFistPrefab.transform.DOMoveX(-15f, powerUpSpeeder);
        gridManager.CompleteRow(cell);
        List<Cell> rowCellList = gridManager.GetRow(cell.row);
        for (int i = 0; i < rowCellList.Count; i++)
        {
            if (rowCellList[i].currentCellState == CellState.Square)
            {
                gridManager.TryToFillColumn(rowCellList[i]);
            }
            gridManager.CheckText(rowCellList[i]);
        }
        gridManager.CheckGrid(cell);
    }
}