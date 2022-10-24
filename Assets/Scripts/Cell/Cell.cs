using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public CellState currentCellState;
    public GameObject[] cellSprites;
    public bool isTouched = false;
    public int row;
    public int column;

    private void Start()
    {
        if (isTouched)
        {
            GameObject createdCellSprite;

            if (currentCellState == CellState.Square)
            {
                PoolItem squareItem = PoolManager.instance.TryToGetItem(PoolItemType.Square);
                createdCellSprite = squareItem.gameObject;
            }
            else
            {
                PoolItem crossItem = PoolManager.instance.TryToGetItem(PoolItemType.Cross);
                createdCellSprite = crossItem.gameObject;
            }

            createdCellSprite.transform.localScale = gameObject.transform.lossyScale;
            createdCellSprite.transform.position = gameObject.transform.position;
            createdCellSprite.gameObject.transform.SetParent(gameObject.transform);
        }
    }
    public void ChangeCellState(CellState state)
    {
        if (currentCellState != state)
            currentCellState = state;
    }

    public void FillCell(CellState cellState)
    {
        GameObject createdCellSprite;
        if (!isTouched)
        {
            if (cellState == CellState.Square)
            {
                PoolItem squareItem = PoolManager.instance.TryToGetItem(PoolItemType.Square);
                createdCellSprite = squareItem.gameObject;
                isTouched = true;
            }
            else
            {
                PoolItem crossItem = PoolManager.instance.TryToGetItem(PoolItemType.Cross);
                createdCellSprite = crossItem.gameObject;
                isTouched = true;

            }

            createdCellSprite.transform.localScale = gameObject.transform.lossyScale;
            createdCellSprite.transform.position = gameObject.transform.position;
            createdCellSprite.gameObject.transform.SetParent(gameObject.transform);
        }
    }

    public void FillSavedCells(CellState cellState)
    {
        GameObject createdCellSprite;

        if (cellState == CellState.Square)
        {
            PoolItem squareItem = PoolManager.instance.TryToGetItem(PoolItemType.Square);
            createdCellSprite = squareItem.gameObject;
        }
        else
        {
            PoolItem crossItem = PoolManager.instance.TryToGetItem(PoolItemType.Cross);
            createdCellSprite = crossItem.gameObject;
        }

        createdCellSprite.transform.localScale = gameObject.transform.lossyScale;
        createdCellSprite.transform.position = gameObject.transform.position;
        createdCellSprite.gameObject.transform.SetParent(gameObject.transform);

    }
    public bool IsCellReadyToDisplay(Cell cell, InputType currentInputType)
    {
        if (currentInputType == InputType.Square)
        {
            return cell.currentCellState == CellState.Square;
        }
        else
        {
            return cell.currentCellState == CellState.Cross;
        }
    }
    public CellData getCellData()
    {
        return new CellData(isTouched);
    }
}

[Serializable]
public class CellData
{
    public bool isTouched;

    public CellData(bool touched)
    {
        isTouched = touched;
    }
}

