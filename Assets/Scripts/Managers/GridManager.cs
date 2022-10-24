using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GridManager : MonoBehaviour
{
    public GridBuilder gridBuilder;
    public List<Cell> cellList = new List<Cell>();
    List<Cell> returnList = new List<Cell>();
    public List<int> numbers = new List<int>();
    public int gridSize;
    public string numberToFade;
    public List<string> printNumbersList = new List<string>();
    [SerializeField] GameObject gridBackground;
    
    private void Start()
    {
        GameManager.instance.SetGridManager(this);

        int levelIndex = LevelManager.currentLevelIndex % 6;
        
        if (levelIndex <4)
        {
            gridSize = 5;
        }
        else
        {
            gridSize = 10;
        }
        gridBuilder.GenerateGrid(cellList, gridSize, LevelManager.currentLevelIndex);
        
        GameManager.instance.Initialize();
    }
    public void CheckLoadedLevelText()
    {
        for (int i = 0; i < cellList.Count; i++)
        {
            if (cellList[i].isTouched)
            {
                CheckText(cellList[i]);
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if ((x >= 0 && x<gridSize) && (y >= 0 && y < gridSize))
        {
            return cellList[(x * gridSize) + y];
        }
        return null;
    }

    public List<Cell> GetColumn(int ColIndex)
    {
        returnList.Clear();

        for (int i = 0; i < gridSize; i++)
        {
            returnList.Add(GetCell(i, ColIndex));
        }

        return returnList;
    }

    public List<Cell> GetRow(int RowIndex)
    {
        returnList.Clear();

        for (int i = 0; i < gridSize; i++)
        {
            returnList.Add(GetCell(RowIndex, i));
        }

        return returnList;
    }
    
    public List<int> GetLineInfo(List<Cell> line)
    {
        numbers.Clear();
        int counter = 0;
        
        foreach (Cell cell in line)
        {
            if (cell.currentCellState == CellState.Square)
            {
                counter++;
            }
            else
            {
                if (counter != 0)
                {
                    numbers.Add(counter);
                    counter = 0;
                }
            }
        }

        if (counter != 0)
        {
            numbers.Add(counter);
            counter = 0;
        }
        
        return numbers;

    }

    public void CheckText(Cell cell)
    {
        
        int m = 0;
        List<int> numbersRow = GetLineInfo(GetRow(cell.row));
        List<List<Cell>> listOfSquareBlockInRow = new List<List<Cell>>(numbersRow.Count);
        List<Cell> squaresInRow = FindSquares(GetRow(cell.row));
        
        for (int i = 0; i < numbersRow.Count; i++)
        {
            List<Cell> tempList = new List<Cell>(numbersRow[i]);
            for (int j = 0; j < numbersRow[i]; j++)
            {
                tempList.Add(squaresInRow[m]);
                m++;
            }
            listOfSquareBlockInRow.Add(tempList);
        }
        printNumbersList.Clear();
        
        for (int i = 0; i < listOfSquareBlockInRow.Count; i++)
        {
            if (listOfSquareBlockInRow[i].FindAll(squares => squares.isTouched).Count == listOfSquareBlockInRow[i].Count)
            {
                int leftSideCounter = 0;
                for (int j = 0; j < listOfSquareBlockInRow[i][0].column; j++)
                {
                    if (GetCell(listOfSquareBlockInRow[i][0].row, j).isTouched)
                    {
                        leftSideCounter++;
                    }
                }
                
                bool isFaded = false;
                
                if (leftSideCounter == listOfSquareBlockInRow[i][0].column)
                {
                    numberToFade = "<color=#ffffff66>" + numbersRow[i] + "</color>";
                    printNumbersList.Add(numberToFade);
                    isFaded = true;
                }
                if (isFaded == false)
                {
                    int rightSideCounter = 0;
                    for (int j = listOfSquareBlockInRow[i][0].column; j < gridSize; j++)
                    {
                        if (GetCell(listOfSquareBlockInRow[i][0].row, j).isTouched)
                        {
                            rightSideCounter++;
                        }
                    }
                    if (rightSideCounter == gridSize - listOfSquareBlockInRow[i][0].column)
                    {
                        numberToFade = "<color=#ffffff66>" + numbersRow[i] + "</color>";
                        printNumbersList.Add(numberToFade);
                        isFaded = true;
                    }
                }
                if (isFaded == false)
                {
                    printNumbersList.Add(numbersRow[i].ToString());
                }
            }
            else
            {
                printNumbersList.Add(numbersRow[i].ToString());
            }
        }
        
        gridBuilder.rowTextList[cell.row].text = string.Empty;
        string str = String.Join(" ", printNumbersList);
        gridBuilder.rowTextList[cell.row].text = str;
        printNumbersList.Clear();
        
        List<int> numbersCol = GetLineInfo(GetColumn(cell.column));
        List<List<Cell>> listOfSquareBlockInCol = new List<List<Cell>>(numbersCol.Count);
        List<Cell> squaresInCol = FindSquares(GetColumn(cell.column));
        int n = 0;
        for (int i = 0; i < numbersCol.Count; i++)
        {
            List<Cell> tempList = new List<Cell>(numbersCol[i]);
            for (int j = 0; j < numbersCol[i]; j++)
            {
                tempList.Add(squaresInCol[n]);
                n++;
            }
            listOfSquareBlockInCol.Add(tempList);
        }
        printNumbersList.Clear();
        
        for (int i = 0; i < listOfSquareBlockInCol.Count; i++)
        {
            if (listOfSquareBlockInCol[i].FindAll(squares => squares.isTouched).Count == listOfSquareBlockInCol[i].Count)
            {
                int upSideCounter = 0;
                for (int j = 0; j < listOfSquareBlockInCol[i][0].row; j++)
                {
                    if (GetCell(j, listOfSquareBlockInCol[i][0].column).isTouched)
                    {
                        upSideCounter++;
                    }
                }
                
                bool isFaded = false;
                
                if (upSideCounter == listOfSquareBlockInCol[i][0].row)
                {
                    numberToFade = "<color=#ffffff66>" + numbersCol[i] + "</color>";
                    printNumbersList.Add(numberToFade);
                    isFaded = true;
                }
                if (isFaded == false)
                {
                    int downSideCounter = 0;
                    for (int j = listOfSquareBlockInCol[i][0].row; j < gridSize; j++)
                    {
                        if (GetCell(j, listOfSquareBlockInCol[i][0].column).isTouched)
                        {
                            downSideCounter++;
                        }
                    }
                    if (downSideCounter == gridSize - listOfSquareBlockInCol[i][0].row)
                    {
                        numberToFade = "<color=#ffffff66>" + numbersCol[i] + "</color>";
                        printNumbersList.Add(numberToFade);
                        isFaded = true;
                    }
                }
                if (isFaded == false)
                {
                    printNumbersList.Add(numbersCol[i].ToString());
                }
            }
            else
            {
                printNumbersList.Add(numbersCol[i].ToString());
            }
        }
        gridBuilder.colTextList[cell.column].text = string.Empty;
        string str_ = String.Join(" ", printNumbersList);
        gridBuilder.colTextList[cell.column].text = str_;
        printNumbersList.Clear();
    }

    private List<Cell> FindSquares(List<Cell> cellList)
    {
        List<Cell> squareList = new List<Cell>();
        foreach (Cell _cell in cellList)
        {
            if (_cell.currentCellState == CellState.Square)
            {
                squareList.Add(_cell);
            }
        }

        return squareList;
    }

    private List<Cell> FindCrosses(List<Cell> cellList)
    {
        List<Cell> crossList = new List<Cell>();
        foreach (Cell cell in cellList)
        {
            if (cell.currentCellState == CellState.Cross)
            {
                crossList.Add(cell);
            }
        }

        return crossList;
    }

    private void FillCrossesInLine(List<Cell> lineList)
    {
        List<Cell> crossList = FindCrosses(lineList); 

        float delay = 0;
        
        for (int i = 0; i < crossList.Count; i++)
        {
            if (crossList[i].isTouched == false)
            {
                crossList[i].FillCell(0);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(crossList[i].transform.GetChild(0).DOScale(1f, 0.3f).From(0f).SetDelay(delay));
                delay += 0.15f;

                crossList[i].isTouched = true;
                CheckText(crossList[i]);
            }
        }
    }

    public bool TryToFillRow(Cell cell)
    {
        List<Cell> squareList = FindSquares(GetRow(cell.row));
        
        int touchedSquareNumberinLine = squareList.FindAll(square => square.isTouched).Count;

        if (squareList.Count == touchedSquareNumberinLine)
        {
            FillCrossesInLine(GetRow(cell.row));
            return true;
        }
        return false;
    }

    public bool TryToFillColumn(Cell cell )
    {
        
        // List<Cell> colList = GetColumn(cell.column);
        List<Cell> squareList = FindSquares(GetColumn(cell.column));
        
        int touchedSquareNumberinLine = squareList.FindAll(square => square.isTouched).Count;
        
        
        if (squareList.Count == touchedSquareNumberinLine)
        {
            FillCrossesInLine(GetColumn(cell.column));
            
            return true;
        }
        return false;
    }
    
    public void CompleteRow(Cell cell)
    {
        List<Cell> rowList = GetRow(cell.row);
        
        float delay = 0;
        for (int i = rowList.Count-1; i >= 0; i--)
        {
            if (rowList[i].isTouched == false)
            {
                rowList[i].FillCell(rowList[i].currentCellState);
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(rowList[i].transform.GetChild(0).DOScale(1f, 0.3f).From(0f).SetDelay(delay));
                delay += 0.15f;
            }
        }
    }

    public void CompleteColumn(Cell cell)
    {
        List<Cell> columnList = GetColumn(cell.column);
        float delay = 0;

        for (int i = columnList.Count-1; i >= 0; i--)
        {
            if (columnList[i].isTouched == false)
            {
                columnList[i].FillCell(columnList[i].currentCellState);
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(columnList[i].transform.GetChild(0).DOScale(1f, 0.3f).From(0f).SetDelay(delay));
                delay += 0.15f;
            }
        }
    }

    public void CheckGrid(Cell cell)
    {
        if (!(GameManager.instance.currentGameState.Contains(GameState.PowerUpInUsage)))
        {
            if (TryToFillRow(cell))
            {
                int lineFullCounter = 0;

                for (int i = 0; i < gridSize; i++)
                {
                    if (TryToFillRow(GetCell(i, 0)))
                    {
                        lineFullCounter++;
                    }
                }
                if (lineFullCounter == gridSize)
                {
                    GameManager.instance.ChangeGameState(GameState.Win);
                    
                }
            }
            TryToFillColumn(cell);
        }
        else
        {
            List<Cell> allSquares = FindSquares(cellList);
            int openedSquares = 0;
            foreach (Cell _cell in allSquares)
            {
                if (_cell.isTouched)
                {
                    openedSquares++;
                }
            }
            if (openedSquares==allSquares.Count)
            {
                GameManager.instance.ChangeGameState(GameState.Win);
            }
        }
        CheckText(cell); 
    }
    
    public void WrongClickAnimation()
    {
        gridBackground.transform.DOShakePosition(1f, 0.5f, 10, 1f);
    }
    public List<CellData> GetCellDataList()
    {
        List<CellData> dataList = new List<CellData>();

        foreach (Cell cell in cellList)
        {
            dataList.Add(cell.getCellData());
        }
        return dataList;
    }

    public void SetCellDataList(List<CellData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            cellList[i].isTouched = list[i].isTouched;
        }
    }

    
}
