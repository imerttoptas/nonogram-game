using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CurrencyIndicator : MonoBehaviour
{
    public CurrencyItemType currencyItemType;
    public TextMeshProUGUI countText;

    private void Start()
    {
        //countText.text = PlayerPrefs.GetInt(CurrencyManager.instance.GetCurrencyItem(currencyItemType).currencyItemKey).ToString();
        //countText.text = 
    }
    public void SetText(int amount)
    {
        countText.text = amount.ToString();
    }
    
}
