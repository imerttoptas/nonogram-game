using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [System.Serializable]
    public class Pool
    {
        public PoolItem poolPrefab;
        public PoolItemType poolItemType;
        public Queue<PoolItem> availableList = new Queue<PoolItem>(25);
        public List<PoolItem> usingList;
    }
    public List<Pool> poolList;
    public Dictionary<PoolItemType, Pool> poolDictionary;
    
    void Start()
    {
        poolDictionary = new Dictionary<PoolItemType, Pool>();
        foreach (Pool pool in poolList)
        {
            poolDictionary.Add(pool.poolItemType, pool);
        }
    }
    
    public void RemoveItemFromPool(PoolItem poolItem)
    {

        if (poolDictionary[poolItem.poolItemType].usingList.Contains(poolItem))
        {
            poolDictionary[poolItem.poolItemType].usingList.Remove(poolItem);
            poolItem.gameObject.SetActive(false);
            poolItem.gameObject.transform.SetParent(this.transform);
            poolDictionary[poolItem.poolItemType].availableList.Enqueue(poolItem);
        }
    }

    

    public void ResetAllPools()
    {
        foreach (Pool pool in poolList)
        {
            for (int i = 0; i < pool.usingList.Count; i++)
            {
                pool.usingList[i].gameObject.SetActive(false);
                PoolItem tempPoolItem = pool.usingList[i];
                pool.availableList.Enqueue(tempPoolItem);
                pool.usingList.RemoveAt(i);
            }
        }
    }
    
    public PoolItem TryToGetItem(PoolItemType poolItemType)
    {
        PoolItem targetItem = null;
        if (poolDictionary.TryGetValue(poolItemType, out Pool targetPool))
        {
            if (targetPool.availableList.Count > 0)
            {
                targetItem = targetPool.availableList.Dequeue();
                targetItem.gameObject.SetActive(true);
            }
            else
            {
                targetItem = Instantiate(targetPool.poolPrefab);
            }
            if (!targetPool.usingList.Contains(targetItem))
            {
                targetPool.usingList.Add(targetItem);
            }
            else
            {
                Debug.LogError("ERROR");
            }
        }
        return targetItem;
    }
}

