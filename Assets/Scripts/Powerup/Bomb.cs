using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bomb : Powerup
{
    [SerializeField] GameObject bombPrefab;
    public override void Use(Cell cell)
    {
        //CurrencyManager.instance.TryToDecreaseCurrencyCount(CurrencyItemType.Bomb, 1);
        LevelManager.instance.UserData.bombPowerupCount -= 1;

        int gridSize = GameManager.instance.GetGridManager().gridSize;
        Vector3 defaultPowerupScale = new Vector3(0.25f, 0.25f, 0.25f);
        Vector3 powerupScaler = defaultPowerupScale / (gridSize / 5);
        GameObject createdBombPrefab = Instantiate(bombPrefab);
        createdBombPrefab.transform.position = new Vector2(cell.transform.position.x, cell.transform.position.y);
        createdBombPrefab.transform.localScale = powerupScaler;
        int row = cell.row;
        int col = cell.column;
        float delay = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Cell affectedCell = gridManager.GetCell(row + i, col + j);
                if (affectedCell != null)
                {
                    if (affectedCell.isTouched == false)
                    {
                        affectedCell.FillCell(affectedCell.currentCellState);
                        Sequence mySequence = DOTween.Sequence();
                        mySequence.Append(affectedCell.transform.GetChild(0).DOScale(1f, 0.3f).From(0f).SetDelay(delay));
                        delay += 0.05f;
                    }
                    if (affectedCell.currentCellState == CellState.Square)
                    {
                        gridManager.TryToFillColumn(affectedCell);
                        gridManager.TryToFillRow(affectedCell);
                    }
                    gridManager.CheckText(affectedCell);

                }
            }
            gridManager.CheckGrid(cell);
        }
    }
}