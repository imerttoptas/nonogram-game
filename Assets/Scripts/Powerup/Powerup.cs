using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public GridManager gridManager;
    
    public virtual void Use(Cell cell)
    {
        PowerupController.instance.isPowerUpAnimEnded = false;
        GameManager.instance.DisableInput();
        
        
        gridManager = GameManager.instance.GetGridManager();
        
    }
    
    protected PoolItem GetInstantiatedPowerUp(PoolItemType poolItemType, Vector2 position, Vector2 scale)
    {
        PoolItem powerUp = PoolManager.instance.TryToGetItem(poolItemType);
        powerUp.gameObject.transform.position = position;
        powerUp.gameObject.transform.localScale = scale;
        
        return powerUp;
    }

    protected void EndAnimation(PoolItem poolItem)
    {
        PoolManager.instance.RemoveItemFromPool(poolItem);
        PowerupController.instance.isPowerUpAnimEnded = true;
        GameManager.instance.EnableInput();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cell"))
        {
            Debug.Log("hit cell");
        }
    }
}
