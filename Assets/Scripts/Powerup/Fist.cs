using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Fist : Powerup
{
    [SerializeField] GameObject fistPrefab;
    public int Cost { get; } = 1000;
    
    public override void Use(Cell cell)
    {
        base.Use(cell);
        
        CurrencyManager.instance.TryToDecreaseCurrencyCount(CurrencyItemType.Fist, 1);
        
        
        Vector3 defaultPowerUpScale = new Vector3(0.25f, 0.25f, 0.25f);
        
        Vector3 powerUpScaler = defaultPowerUpScale / (gridManager.gridSize / 5);
        float defaultPowerUpSpeed = 1f;
        float powerUpSpeeder = defaultPowerUpSpeed * (gridManager.gridSize / 5);
        
        Vector2 position = new Vector2(15,cell.transform.position.y);
        PoolItem powerUp = GetInstantiatedPowerUp(PoolItemType.Fist, position, powerUpScaler);
        SoundManager.instance.PlaySoundEffect(SoundEffectType.FistPowerUp);
        powerUp.transform.DOMoveX(-15f, powerUpSpeeder).OnComplete(() => EndAnimation(powerUp));
        
        gridManager.CompleteRow(cell);
        
        List<Cell> rowList = gridManager.GetRow(cell.row);
        
        for (int i = 0; i < rowList.Count; i++)
        {
            if (rowList[i].currentCellState == CellState.Square)
            {
                gridManager.TryToFillColumn(rowList[i]);
            }
            
            gridManager.CheckText(rowList[i]);
        }
        gridManager.CheckGrid(cell);
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.left,+1f);
        Debug.DrawRay(transform.position,Vector3.left, Color.red, 10f);
        if (hit.collider != null )
        {
            if (hit.collider.CompareTag("Cell"))
            {
                Cell cell = hit.collider.gameObject.GetComponent<Cell>();
                Debug.Log(cell.row + "X" + cell.column);    
            }
        }
    }
}