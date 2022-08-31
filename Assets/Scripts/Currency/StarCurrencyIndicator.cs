using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCurrencyIndicator : CurrencyIndicator
{
    static int starCount;
    private void Start()
    {
        starCount = LevelManager.instance.UserData.diamonds;
            
        countText.text = (LevelManager.instance.UserData.totalStar).ToString();
    }
}
