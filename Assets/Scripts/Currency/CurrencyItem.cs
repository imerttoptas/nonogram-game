using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[System.Serializable]
public class CurrencyItem
{
    public int initialCount;
    public int count;
    public int targetCount;
    public string currencyItemKey;
    public CurrencyItemType currencyItemType;
    public System.Action<int> OnCurrencyCountChange;
    
    public void Initialize()
    {
        if (PlayerPrefs.HasKey(currencyItemKey))
        {
            count = targetCount = PlayerPrefs.GetInt(currencyItemKey);
        }
        else
        {
            count = targetCount = initialCount;
        }
    }
    
    public void ChangeAmount(int amount ,float duration = 0.5f)
    {
        targetCount = amount;
        DOTween.To(() => count, x => count = x, targetCount, duration).OnUpdate(()=>
        OnCurrencyCountChange?.Invoke(count));
        PlayerPrefs.SetInt(currencyItemKey, targetCount);
    }
}
