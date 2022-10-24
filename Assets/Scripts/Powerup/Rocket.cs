using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Rocket : Powerup
{
    [SerializeField] GameObject rocketPrefab;
    public int Cost { get; } = 1000;
    public override void Use(Cell cell)
    {
        
        base.Use(cell);

        CurrencyManager.instance.TryToDecreaseCurrencyCount(CurrencyItemType.Rocket, 1);
        
        int gridSize = gridManager.gridSize;
        
        Vector3 defaultPowerUpScale = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 powerUpScaler = defaultPowerUpScale / (gridSize / 5);

        Vector3 position = new Vector3(cell.transform.position.x, -15f, cell.transform.position.z);
        
        PoolItem powerUp = GetInstantiatedPowerUp(PoolItemType.Rocket,position,powerUpScaler);
        SoundManager.instance.PlaySoundEffect(SoundEffectType.RocketPowerUp);
        powerUp.transform.DOMoveY((15f), 2f).OnComplete(() => EndAnimation(powerUp));
        
        gridManager.CompleteColumn(cell);
        List<Cell> collList = gridManager.GetColumn(cell.column);
        
        for (int i = 0; i < collList.Count; i++)
        {
            if (collList[i].currentCellState == CellState.Square)
            {
                gridManager.TryToFillColumn(collList[i]);
            }
            gridManager.CheckText(collList[i]);
        }        
        
        gridManager.CheckGrid(cell);
        
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward,+15f);
        Debug.DrawRay(transform.position,Vector3.forward, Color.red, 10f);
        if (hit.collider != null )
        {
            if (hit.collider.CompareTag("Cell"))
            {
                Cell cell = hit.collider.gameObject.GetComponent<Cell>();
                Debug.Log(cell.row + "X" + cell.column);    
            }
        }
        
        
        
        // if (hit.collider!= null && hit.collider.CompareTag("Cell"))
        // {
        //     
        //     Cell cell = hit.collider.gameObject.GetComponent<Cell>();
        //     Debug.Log(cell.row + "X" + cell.column);
        //     if (!cell.isTouched)
        //     {
        //         cell.FillCell(cell.currentCellState);
        //         if (cell.currentCellState == CellState.Square)
        //         {
        //             gridManager.TryToFillRow(cell);
        //             // gridManager.CheckGrid(cell);
        //         }
        //         
        //         gridManager.CheckText(cell);
        //     }
        //     gridManager.CheckGrid(cell);
        // }
        
        
    }

    
}