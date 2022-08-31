using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondCurrencyIndicator : CurrencyIndicator
{
    int count;
    private void Start()
    {
        count = LevelManager.instance.UserData.diamonds;
        //DOTween.To(() => count, x => count = x, LevelManager.instance.UserData.diamonds.ToString();, 0.5f)        
        countText.text = LevelManager.instance.UserData.diamonds.ToString();
    }
    
}
