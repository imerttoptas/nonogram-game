using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupCurrencyIndicator : CurrencyIndicator
{
    [SerializeField] Image amountBackground;
    [SerializeField] Sprite greenBackground, redBackground;

    private void Awake()
    {
        SetUI(CurrencyManager.instance.GetItemCount(currencyItemType));
    }
    private void OnEnable()
    {
        SetText(CurrencyManager.instance.GetItemCount(currencyItemType));
        CurrencyManager.instance.GetCurrencyItem(currencyItemType).OnCurrencyCountChange += SetUI;
    } 
    private void OnDisable()
    {
        CurrencyManager.instance.GetCurrencyItem(currencyItemType).OnCurrencyCountChange -= SetUI;
    }

    public void SetUI(int amount)
    {
        SetImage(amount);
        SetText(amount);
    }
    
    public void SetImage(int amount)
    {
        if (amount == 0)
        {
            amountBackground.sprite = greenBackground;
            amountBackground.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            amountBackground.sprite = redBackground;
            amountBackground.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
