using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : Singleton<PowerupController>
{
    public PowerUpType currentPowerUpType;
    public Rocket rocket;
    public Bomb bomb;
    public Fist fist;
    public bool isPowerUpAnimEnded = true;
    public System.Action<int> OnPowerUpCountChange;
    public Dictionary<PowerUpType, Powerup> powerUpDictionary;
    private void Start()
    {
        powerUpDictionary = new Dictionary<PowerUpType, Powerup>()
        {
            { PowerUpType.Rocket, rocket },
            { PowerUpType.Bomb, bomb },
            { PowerUpType.Fist, fist }
        }; 
        
    }

    public void UsePowerUp(Cell cell)
    {
        switch (currentPowerUpType)
        {
            case PowerUpType.Rocket:
                rocket.Use(cell);
                break;
            case PowerUpType.Bomb:
                bomb.Use(cell);
                break;
            case PowerUpType.Fist:
                fist.Use(cell);
                break;
        }
    }
   
    public int GetPowerUpCount(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.Rocket:
                return CurrencyManager.instance.GetItemCount(CurrencyItemType.Rocket);
            case PowerUpType.Bomb:
                return CurrencyManager.instance.GetItemCount(CurrencyItemType.Bomb);
            case PowerUpType.Fist:
                return CurrencyManager.instance.GetItemCount(CurrencyItemType.Fist);
            default:
                return 0;
        }
    }
    
    public int GetPowerupCost()
    {
        switch (currentPowerUpType)
        {
            case PowerUpType.Rocket:
                return rocket.Cost;
            case PowerUpType.Bomb:
                return bomb.Cost;
            case PowerUpType.Fist:
                return fist.Cost;
            default:
                return 0;
        }
    }
    
    public void GetPowerUpSpriteSizeAndRotation()
    {
        
    }
    public CurrencyItemType GetCurrencyItemTypeOfPowerUp()
    {
        switch (currentPowerUpType)
        {
            case PowerUpType.Rocket:
                return CurrencyItemType.Rocket;
            case PowerUpType.Bomb:
                return CurrencyItemType.Bomb;
            case PowerUpType.Fist:
                return CurrencyItemType.Fist;
            default:
                return 0;
        }
    }

    
}
