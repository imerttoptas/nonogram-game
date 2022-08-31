using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public Camera mainCam;
    public GridManager gridManager;
    public GameUIManager gameUIManager;
    public Cell cell;
    private Cell chosenCell;
    private Cell firstCell = null;
    bool wrongClick = false;
    public InputType currentInputType = InputType.Square;
    MoveDirection currentMoveDirection;

   
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.Playing || GameManager.instance.currentGameState.Contains(GameState.PowerUpInUsage))
        {
            if (!wrongClick && Input.GetMouseButton(0))
            {
                if (!GameManager.instance.currentGameState.Contains(GameState.PowerUpInUsage))
                {
                    Cell hitCell = TryToGetHitCell();

                    if (hitCell != null)
                    {
                        chosenCell = hitCell; //check out later
                    }

                    if (chosenCell != null && chosenCell.isTouched == false)
                    {
                        if (chosenCell.IsCellReadyToDisplay(chosenCell, currentInputType))
                        {
                            if (firstCell == null)
                            {
                                firstCell = chosenCell;
                            }

                            chosenCell.FillCell(chosenCell.currentCellState);
                            SoundManager.instance.PlaySoundEffect(SoundEffectType.ButtonClick);
                            chosenCell.transform.GetChild(0).DOScale(1f, 0.3f).From(0f); 

                            gridManager.CheckText(chosenCell);
                            
                            gridManager.CheckGrid(chosenCell);

                            SetDirection(firstCell, chosenCell);
                        }
                        else
                        {
                            wrongClick = true;
                            chosenCell.FillCell(chosenCell.currentCellState);
                            gridManager.CheckText(chosenCell);
                            SoundManager.instance.PlaySoundEffect(SoundEffectType.WrongClick);
                        }
                    }
                }
                else
                {

                    Cell hitCell = TryToGetHitCell();
                    RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    if (hitCell != null)
                    {
                        chosenCell = hitCell; //check out later
                    }
                    if (hit.collider != null && hit.collider.CompareTag("Cell") && chosenCell.isTouched == false)
                    {
                        PowerupController.instance.UsePowerUp(chosenCell);
                        gameUIManager.isPowerupSelected = false;
                    }
                    gameUIManager.FadeOutPowerUpMask((int)PowerupController.instance.currentPowerUpType);
                    GameManager.instance.RemoveGameState(GameState.PowerUpInUsage);
                    //gameUIManager.isPowerupSelected = false;
                }
            }

            else if (Input.GetMouseButtonUp(0))
            {
                
                currentMoveDirection = MoveDirection.Null;
                if (wrongClick)
                {

                    GameManager.instance.DecreaseLife();
                    wrongClick = false;
                    gridManager.CheckGrid(chosenCell);
                }
                

                chosenCell = null;
                firstCell = null;
            }

            
        }
    }

    private bool CheckMoveDirection(Cell cell)
    {
        if (currentMoveDirection == MoveDirection.Horizontal)
        {
            return cell.row == firstCell.row;
        }
        else if(currentMoveDirection == MoveDirection.Vertical)
        {
            return cell.column == firstCell.column;
        }
        return true;
    }

    private Cell TryToGetHitCell()
    {
        RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
       
        if (hit.collider != null && hit.collider.tag == "Cell")
        {
            Cell cell = hit.collider.gameObject.GetComponent<Cell>();

            if (firstCell == null || CheckMoveDirection(cell))
            {
                return cell;
            }
        }
        
        return null; 
    }

    private void SetDirection(Cell firstCell, Cell chosenCell)
    {
        if (firstCell != null && chosenCell != firstCell)
        {
            if (chosenCell.row == firstCell.row)
            {
                currentMoveDirection = MoveDirection.Horizontal;
            }
            else if (chosenCell.column == firstCell.column)
            {
                currentMoveDirection = MoveDirection.Vertical;
            }
        }
    }
    
    private enum MoveDirection
    {
        Null = 0,
        Horizontal = 1,
        Vertical = 2
    }
}


