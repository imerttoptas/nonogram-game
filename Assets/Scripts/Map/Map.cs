using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{   
    public List<Transform> buttonPositions;
    public int mapIndex;
    public int previousButtonCount;
    public PlayLevelButton levelButtonPrefab;
    public List<PoolItem> buttonList;
    private PoolItem poolItem;
    private void Awake()
    {
        
        buttonList = new List<PoolItem>();
    }

    private void Initialize()
    {
        for (int i = 0; i < buttonPositions.Count; i++)
        {
            poolItem = PoolManager.instance.TryToGetItem(PoolItemType.LevelButton);
            poolItem.gameObject.transform.SetParent(buttonPositions[i].transform);
            poolItem.GetComponent<RectTransform>().localPosition = buttonPositions[i].GetComponent<RectTransform>().localPosition;
            buttonList.Add(poolItem);
            poolItem.transform.localPosition = Vector3.zero;
            poolItem.transform.localScale = Vector3.one;
        }
    }
    public List<PoolItem> GetButtonList()
    {
        return buttonList;
    }
    public void SendButtonsBackToPool()
    {
        int size = buttonList.Count;
        for (int i = 0; i < size; i++)
        {
            poolItem = buttonList[0];
            buttonList.RemoveAt(0);
            PoolManager.instance.RemoveItemFromPool(poolItem);
        }
    }
    private void OnEnable()
    {
        buttonList.Clear();
        Initialize();
    }

    private void OnDisable()
    {

    }
}
