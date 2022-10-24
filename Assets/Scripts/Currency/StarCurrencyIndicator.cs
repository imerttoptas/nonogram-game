using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StarCurrencyIndicator : CurrencyIndicator
{
    [SerializeField] GameObject starPrefab;
    public bool isStarAnimEnded;
    
    public void StarAnimation(Transform parent, Vector3 startPos, Vector3 targetPos, int amount)
    {
        float delay = 0f;
        isStarAnimEnded = false;
        
        for (int i = 0; i < amount; i++)
        {
            
            PoolItem createdStar = PoolManager.instance.TryToGetItem(PoolItemType.StarIcon);
            RectTransform starRectTransform = createdStar.GetComponent<RectTransform>();
            starRectTransform.SetParent(parent);
            starRectTransform.localPosition = startPos+ new Vector3(25f,0f,0f)*i;
            starRectTransform.localScale = Vector3.one/2;
            
            Sequence mySequence = DOTween.Sequence();
            mySequence.PrependInterval(delay);

            mySequence.Append(starRectTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f).From(0f).SetDelay(0.3f));
            mySequence.Append(starRectTransform.DOMove(targetPos, 1f).OnComplete(()=>DestroyIcon(createdStar)));
            delay += 0.25f;

            if (i == amount-1)
            {
                mySequence.OnComplete(() => isStarAnimEnded = true);
            }
        }
    }

    private void DestroyIcon(PoolItem icon)
    {
        CurrencyManager.instance.IncreaseCurrencyCount(CurrencyItemType.Star, 1, 0f);
        PoolManager.instance.RemoveItemFromPool(icon);
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
