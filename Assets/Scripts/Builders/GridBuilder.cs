using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridBuilder : MonoBehaviour
{
    #region Grid
    [SerializeField] GridManager gridManager;
    [SerializeField] SpriteRenderer gridBackground;
    [SerializeField] GameObject thinLine;
    [SerializeField] GameObject boldLine;
    #endregion

    #region GridTexts
    public TextMeshPro leftText;
    public TextMeshPro upperText;

    public List<TextMeshPro> rowTextList;
    public List<TextMeshPro> colTextList;

    private bool displayRow = false;
    private bool displayColumn = false;
    #endregion
    public void GenerateGrid(List<Cell> cellList, int gridSize, int seed)
    {

        ScalingGridBackground();
        
        #region Scaling Cells
        
        float gridWidth = gridBackground.bounds.size.x * gridBackground.transform.localScale.x;
        float gridHeight = gridBackground.bounds.size.y * gridBackground.transform.localScale.y;

        float newCellScaleX = gridWidth / gridSize;
        float newCellScaleY = gridHeight / gridSize;

        float defaultPosX = (gridBackground.transform.position.x - gridWidth / 2) + (newCellScaleX / 2);
        float defaultPosY = (gridBackground.transform.position.y + gridHeight / 2) - (newCellScaleY / 2);
        
        #endregion
        
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                float newCellPosX = defaultPosX + (col * newCellScaleX);
                float newCellPosY = defaultPosY - (row * newCellScaleY);
                
                Cell createdCell = GetCreatedCell(new Vector2(newCellPosX, newCellPosY),
                    new Vector2(newCellScaleX, newCellScaleY), row, col);
                cellList.Add(createdCell);
            }
            
        }

        RandomLevelGenerator(cellList, seed);
        
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                if (row == gridSize - 1) //column
                {
                    displayRow = false;
                    displayColumn = true;
                    float rowPosX = defaultPosX + (newCellScaleX * col);
                    float rowPosY = defaultPosY + (newCellScaleX * 0.65f);
                    NumberDisplayer(row, col, newCellScaleX, rowPosX, rowPosY);
                    float thinPosX = defaultPosX + (newCellScaleY * col) + (newCellScaleY / 2);
                    float thinPosY = defaultPosY + (newCellScaleY / 2);
                    if (gridSize == 10)
                    {
                        if (col != gridSize - 1 && col != 4)
                        {
                            thinLine.GetComponent<SpriteRenderer>().size = new Vector2(gridWidth, 0.04f);
                            Instantiate(thinLine, new Vector2(thinPosX, thinPosY), Quaternion.Euler(new Vector3(0, 0, -90)), gridBackground.transform);
                        }
                        else if (col == 4)
                        {
                            boldLine.GetComponent<SpriteRenderer>().size = new Vector2(gridWidth, 0.08f);
                            Instantiate(boldLine, new Vector2(thinPosX, thinPosY), Quaternion.Euler(new Vector3(0, 0, -90)),gridBackground.transform);
                        }
                    }
                    else
                    {
                        if (col != gridSize - 1) //for thin lines
                        {
                            thinLine.GetComponent<SpriteRenderer>().size = new Vector2(gridWidth, 0.04f);
                            Instantiate(thinLine, new Vector2(thinPosX, thinPosY), Quaternion.Euler(new Vector3(0, 0, -90)),gridBackground.transform);
                        }
                    }
                }
                if (col == gridSize - 1) //row
                {
                    displayRow = true;
                    displayColumn = false;
                    float rowPosX = defaultPosX - (newCellScaleY * 0.65f);
                    float rowPosY = defaultPosY - (newCellScaleY * row);
                    NumberDisplayer(row, col, newCellScaleY, rowPosX, rowPosY);
                    float thinPosX = defaultPosX - (newCellScaleY / 2);
                    float thinPosY = defaultPosY - (newCellScaleY * row) - (newCellScaleY / 2);
                    if (gridSize == 10)
                    {
                        if (row != gridSize - 1 && row != 4)
                        {
                            thinLine.GetComponent<SpriteRenderer>().size = new Vector2(gridWidth, 0.04f);
                            Instantiate(thinLine, new Vector2(thinPosX, thinPosY), Quaternion.identity, gridBackground.transform);
                        }
                        else if (row == 4)
                        {
                            boldLine.GetComponent<SpriteRenderer>().size = new Vector2(gridWidth, 0.08f);
                            Instantiate(boldLine, new Vector2(thinPosX, thinPosY), Quaternion.identity, gridBackground.transform);
                        }
                    }
                    else
                    {
                        if (row != gridSize - 1)
                        {
                            thinLine.GetComponent<SpriteRenderer>().size = new Vector2(gridWidth, 0.04f); //SABİT DEĞERİ DÜZELT
                            Instantiate(thinLine, new Vector2(thinPosX, thinPosY), Quaternion.identity, gridBackground.transform);
                        }
                    }
                }
            }
        }
    }

    private void ScalingGridBackground()
    {

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float gridScaleCoefficent = 1f;
        
        if (screenRatio < Constants.DEFAULT_SCREEN_RATIO)
        {
            gridScaleCoefficent = screenRatio / Constants.DEFAULT_SCREEN_RATIO;
        }
        else
        {
            gridScaleCoefficent = Constants.DEFAULT_SCREEN_RATIO / screenRatio;
        }
        gridBackground.GetComponent<SpriteRenderer>().size *= gridScaleCoefficent;
        
        gridBackground.transform.GetChild(0).localScale = new Vector2(gridBackground.GetComponent<SpriteRenderer>().size.x, gridBackground.GetComponent<SpriteRenderer>().size.y);

    }
    
    private Cell GetCreatedCell(Vector2 position, Vector2 scale,int row, int col)
    {
        PoolItem _cell = PoolManager.instance.TryToGetItem(PoolItemType.Cell);

        Cell createdCell = _cell.GetComponent<Cell>();
        
        createdCell.transform.position = position;
        createdCell.transform.localScale = scale;
        createdCell.transform.SetParent(gridBackground.transform);
        createdCell.row = row;
        createdCell.column = col;

        return createdCell;
    }

    private void DisplayRowText(int row)
    {
        List<int> numbers = gridManager.GetLineInfo(gridManager.GetRow(row));
        Debug.Log(numbers);
    }
    private void DisplayColText(int col)
    {
        List<int> numbers = gridManager.GetLineInfo(gridManager.GetColumn(col));
        Debug.Log(numbers);
    }
    private void RandomLevelGenerator(List<Cell> cellList, int seed)
    {
        Random.InitState(seed);
        float[] gridMap = new float[gridManager.gridSize * gridManager.gridSize];

        for (int i = 0; i < gridMap.Length; i++)
        {

            gridMap[i] = Random.Range(0, 3);
            if (gridMap[i] == 1)
            {
                cellList[i].ChangeCellState(CellState.Cross);
            }
            else
            {
                cellList[i].ChangeCellState(CellState.Square);
            }
        }
    }

    private void NumberDisplayer(int row, int col, float newCellScaleX, float rowPosX, float rowPosY)
    {
        if (displayRow == true && displayColumn == false) //row
        {
            List<int> numbers = gridManager.GetLineInfo(gridManager.GetRow(row));
            PoolItem leftText = PoolManager.instance.TryToGetItem(PoolItemType.LeftText);
            TextMeshPro text = leftText.GetComponent<TextMeshPro>();
            for (int i = 0; i < numbers.Count; i++)
            {
                text.text += numbers[i] + " ";
            }
            text.transform.position = new Vector2(rowPosX, rowPosY);
            text.fontSize = newCellScaleX * Constants.FONT_SIZE_MULTIPLIER;
            text.transform.SetParent(gridBackground.transform);
            rowTextList.Add(text);
            numbers.Clear();
        }
        else
        {
            List<int> numbers = gridManager.GetLineInfo(gridManager.GetColumn(col)); //column
            PoolItem upperText = PoolManager.instance.TryToGetItem(PoolItemType.UpperText);
            TextMeshPro text = upperText.GetComponent<TextMeshPro>();
            for (int i = 0; i < numbers.Count; i++)
            {
                text.text += numbers[i] + "\n";
            }
            text.transform.position = new Vector2(rowPosX, rowPosY);
            text.fontSize = newCellScaleX * Constants.FONT_SIZE_MULTIPLIER;
            text.transform.SetParent(gridBackground.transform);
            colTextList.Add(text);
            numbers.Clear();
        }
    }
}
