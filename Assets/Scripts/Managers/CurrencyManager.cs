using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public List<CurrencyItem> currencyItemsList;
    public System.Action<CurrencyItemType> OnCurrencyCountChange;



    public void TryToDecreaseCurrencyCount(CurrencyItemType currencyItemType, int amount)
    {
        CurrencyItem currencyItem = currencyItemsList.Find(x => x.currencyItemType == currencyItemType);
        if (currencyItem.targetCount - amount >= 0)
        {
            currencyItem.ChangeAmount(currencyItem.targetCount - amount);
        }
    }

    public void IncreaseCurrencyCount(CurrencyItemType currencyItemType, int amount)
    {
        CurrencyItem currencyItem = currencyItemsList.Find(x => x.currencyItemType == currencyItemType);

        currencyItem.ChangeAmount(currencyItem.targetCount + amount);
    }

    public CurrencyItem GetCurrencyItem(CurrencyItemType currencyItemType)
    {
        return currencyItemsList.Find(x => x.currencyItemType == currencyItemType);
    }

    public System.Action<int> GetCurrencyChangeAction(CurrencyItemType currencyItemType)
    {
        return currencyItemsList.Find(x => x.currencyItemType == currencyItemType).OnCurrencyCountChange;
    }

    public int GetItemCount(CurrencyItemType currencyItemType)
    {
        return currencyItemsList.Find(x => x.currencyItemType == currencyItemType).targetCount;
    }

    public bool CanUseCurrency(CurrencyItemType currencyItemType)
    {
        CurrencyItem currencyItem = currencyItemsList.Find(x => x.currencyItemType == currencyItemType);
        if (currencyItem.count>0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnEnable()
    {
        foreach (CurrencyItem currencyItem in currencyItemsList)
        {
            currencyItem.Initialize();
            Debug.Log(currencyItem.currencyItemKey + " : " + currencyItem.initialCount);

        }
    }
}
