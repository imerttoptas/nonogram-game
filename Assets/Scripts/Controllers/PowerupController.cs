using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : Singleton<PowerupController>
{
    public GridManager gridManager;
    public PowerUpType currentPowerUpType;
    public Rocket rocket;
    public Bomb bomb;
    public Fist fist;
    public System.Action<int> OnPowerUpCountChange;


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
            default:
                break;
        }
    }
    public void SetGridManager(GridManager _gridManager)
    {
        gridManager = _gridManager;
    }
    public int GetPowerupCount(PowerUpType powerUpType)
    {
        UserData userData = LevelManager.instance.UserData;
        
        switch (powerUpType)
        {
            case PowerUpType.Rocket:
                return userData.rocketPowerupCount;
            case PowerUpType.Bomb:
                return userData.bombPowerupCount;
            case PowerUpType.Fist:
                return userData.fistPowerupCount;
            default:
                return 0;
        }
    }
    public int GetPowerupCost(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.Rocket:
                return 100;
            case PowerUpType.Bomb:
                return 150;
            case PowerUpType.Fist:
                return 100;
            default:
                return 0;
        }
    }


}
