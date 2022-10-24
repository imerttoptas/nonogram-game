using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bomb : Powerup
{
    [SerializeField] GameObject bombPrefab;
    public int Cost { get; } = 2000;

    public Cell _cell;
    
    public override void Use(Cell cell)
    {
        base.Use(cell);
        _cell = cell;
        
        CurrencyManager.instance.TryToDecreaseCurrencyCount(CurrencyItemType.Bomb, 1);
        
        int gridSize = GameManager.instance.GetGridManager().gridSize;
        Vector3 defaultPowerUpScale = new Vector3(0.25f, 0.25f, 0.25f);
        Vector3 powerUpScaler = defaultPowerUpScale / (gridSize / 5);
        
        
        PoolItem powerUp = GetInstantiatedPowerUp(PoolItemType.Bomb, cell.transform.position, powerUpScaler);
        
        
    }

    public void FillAffectedCells()
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectType.BombPowerUp);
        
        int row = _cell.row;
        int col = _cell.column;
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
            
            gridManager.CheckGrid(_cell);
        }
        PowerupController.instance.isPowerUpAnimEnded = true;
        GameManager.instance.EnableInput();
        // EndAnimation(GetComponent<PoolItem>());
    }
}