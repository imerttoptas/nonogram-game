using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondCurrencyIndicator : CurrencyIndicator
{
    int count;
    [SerializeField] private GameObject diamondPrefab;
    public bool isDiamondAnimEnded;
    
    public void DiamondAnimation(Transform parent, Vector3 startPos, Vector3 targetPos, int amount)
    { 
        
        float delay = 0;
        int diamondIconCount = amount / 50;
        isDiamondAnimEnded = false;

        for (int i = 0; i < diamondIconCount; i++)
        {
            
            PoolItem createdDiamondIcon = PoolManager.instance.TryToGetItem(PoolItemType.DiamondIcon);
            RectTransform diamondRectTransform = createdDiamondIcon.GetComponent<RectTransform>();
            
            diamondRectTransform.SetParent(parent);
            diamondRectTransform.localPosition = startPos;
            diamondRectTransform.localScale = Vector3.one/2f;
            Sequence mySequence = DOTween.Sequence();
            
            mySequence.PrependInterval(delay);
            mySequence.Append(diamondRectTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f).From(0f))
                .SetDelay(0.3f);
            mySequence.Append(diamondRectTransform.DOMove(targetPos, 1f)
                .OnComplete(() => DestroyIcon(createdDiamondIcon)));
            delay += 0.25f;
            if (i==diamondIconCount-1)
            {
                mySequence.OnComplete(() => isDiamondAnimEnded =true);
            }
        }
    }

    private void DestroyIcon(PoolItem diamondIcon)
    {
        CurrencyManager.instance.IncreaseCurrencyCount(CurrencyItemType.Diamond,50,1f);
        PoolManager.instance.RemoveItemFromPool(diamondIcon);
    }
    
    private void OnEnable()
    {
        CurrencyManager.instance.GetCurrencyItem(currencyItemType).OnCurrencyCountChange += SetText;
    }
    
    private void OnDisable()
    {
        CurrencyManager.instance.GetCurrencyItem(currencyItemType).OnCurrencyCountChange -= SetText;
    }
}
