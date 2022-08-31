using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuMapBuilder : MonoBehaviour
{
    //#region Test
    //[SerializeField] List<Map> mapPrefabList;

    //public Map downMap;
    //public Map midMap;
    //public Map upMap;
    //private Map tempMap;

    //[SerializeField] ScrollRect scrollRect;
    //private RectTransform rectTransform;

    //private float mapHeight = 2048f;

    //private MenuUIManager menuUIManager;

    //public int currentMapIndex = 0;
    //public int maxLevelIndex;

    //public bool upMove = false;
    //public bool downMove = false;
    //int mapCounter;
    //void Initialize()
    //{
    //    if (currentMapIndex == 0)
    //    {
    //        downMap = null;
    //        midMap = GetInstantiatedMap(0, 0);
    //        upMap = GetInstantiatedMap(1, mapHeight);
    //        mapCounter = 2;
    //    }
    //    else if (currentMapIndex == maxLevelIndex)
    //    {
    //        upMap = null;
    //        downMap = GetInstantiatedMap(0, -mapHeight);
    //        midMap = GetInstantiatedMap(1, 0f);
    //    }

    //}


    //void Start()
    //{
    //    maxLevelIndex = LevelManager.instance.userData.levelDataList.Count;
    //    Initialize();
    //}

    //void Update()
    //{

    //    if (currentMapIndex == 0)
    //    {
    //        if (scrollRect.content.localPosition.y > -1024)
    //        {
    //            //scrollRect.velocity = Vector2.zero;
    //            //scrollRect.vertical = false;
    //            scrollRect.movementType = ScrollRect.MovementType.Clamped;
    //            scrollRect.content.localPosition = Vector3.up * -1025f;
    //            scrollRect.movementType = ScrollRect.MovementType.Unrestricted;

    //        }
    //    }
    //    else if (currentMapIndex == maxLevelIndex)
    //    {

    //    }
    //}

    //public Map GetInstantiatedMap(int index, float positionY)
    //{

    //    Map tempMap = Instantiate(mapPrefabList[index]);
    //    rectTransform = tempMap.GetComponent<RectTransform>();
    //    tempMap.transform.SetParent(transform);
    //    rectTransform.localScale = Vector3.one;
    //    rectTransform.anchoredPosition = new Vector3(0f, positionY, 0f);


    //    return tempMap;
    //}

    //public void SetMidMap()
    //{

    //    if (-GetComponent<RectTransform>().anchoredPosition.y == (midMap.transform.GetComponent<RectTransform>().anchoredPosition.y) + mapHeight/2)
    //    {
    //        Debug.Log("Mid Map e ulaşıldı");
    //        Map tempMap = GetInstantiatedMap(2, (currentMapIndex + 1) * mapHeight);

    //        downMap = midMap;
    //        midMap = upMap;
    //        upMap = tempMap;

    //        
    //    }

    //    currentMapIndex = -(int)GetComponent<RectTransform>().anchoredPosition.y / 2048;
    //    if (currentMapIndex != 0)
    //    {
    //        if (currentMapIndex == 1 && mapCounter < 3)
    //        {
    //            GetInstantiatedMap(2, (currentMapIndex + 1) * mapHeight + mapHeight / 2);
    //            mapCounter++;
    //        }
    //        else if (currentMapIndex % 3 == 0)
    //        {
    //            Debug.Log("Instantiate down map");
    //        }
    //        Debug.Log(currentMapIndex);
    //    }

    //}
    //#endregion

    #region main
    [SerializeField] private List<Map> mapPrefabList;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] Sprite firstMapSprite;
    [SerializeField] Map firstBeachMapPrefab;
    private RectTransform rectTransform;

    private int index;
    private int maxLevelIndex;
    private int moveDest = -1;

    private Vector3 lastMousePos;
    private Vector3 startMousePos;

    private Map botMap;
    private Map midMap;
    private Map topMap;
    private Map tempMap;

    public Vector3 defaultBotPos;
    public Vector3 defaultMidPos;
    public Vector3 defaultTopPos;

    private float mapHeight = 2048f;
    private float mapChangeOffset = 250f;
    private float botBound;
    private float topBound;

    private bool check = false;
    private MenuUIManager mainMenuManager;
    [SerializeField] Image upperBar;

    private void Initialize()
    {
        if (index == 0)
        {
            botMap = null;
            midMap = InstantiateMap(0, 0f);
            topMap = InstantiateMap(1, mapHeight);
        }
        else if (index == maxLevelIndex)
        {
            topMap = null;
            botMap = InstantiateMap(0, -mapHeight);
            midMap = InstantiateMap(1, 0f);
        }
        else
        {
            botMap = InstantiateMap(index - 1, -mapHeight);
            midMap = InstantiateMap(index, 0f);
            topMap = InstantiateMap(index + 1, mapHeight);
        }
    }
    private void Start()
    {
        mainMenuManager = FindObjectOfType<MenuUIManager>();

        int mapCounter = -1;
        int temp = LevelManager.instance.UserData.levelDataList.Count;

        while (temp > 0 && mapCounter < mapPrefabList.Count)
        {
            temp -= mapPrefabList[mapCounter + 1].buttonPositions.Count;
            mapCounter++;
        }

        index = mapCounter;

        maxLevelIndex = mapPrefabList.Count - 1;
        botBound = (index * scrollRect.GetComponent<RectTransform>().sizeDelta.y) - (scrollRect.GetComponent<RectTransform>().sizeDelta.y / 2f);
        topBound = (-(maxLevelIndex - index) * mapHeight) - (mapHeight / 2f) - (mapHeight -scrollRect.GetComponent<RectTransform>().sizeDelta.y)/2 - upperBar.GetComponent<RectTransform>().sizeDelta.y;

        defaultBotPos = Vector3.up * (-mapHeight);
        defaultMidPos = Vector3.zero;
        defaultTopPos = Vector3.up * (mapHeight);

        Initialize();
        //ArrangeFirstMap();
    }

    private void Update()
    {
        if (index == 0)
        {
            check = false;

            if (Input.GetKey(KeyCode.Mouse0))
            {
                startMousePos = Input.mousePosition;
                check = true;
            }

            if (scrollRect.content.localPosition.y > botBound)
            {
                scrollRect.velocity = Vector2.zero;
                scrollRect.vertical = false;
                scrollRect.content.localPosition = Vector3.up * botBound;
            }

            if (check && startMousePos.y < lastMousePos.y)
            {
                scrollRect.vertical = true;
            }

            lastMousePos = Input.mousePosition;
        }

        else if (index == maxLevelIndex)
        {
            check = false;

            if (Input.GetKey(KeyCode.Mouse0))
            {
                startMousePos = Input.mousePosition;
                check = true;
            }

            if (scrollRect.content.localPosition.y < topBound)
            {
                scrollRect.velocity = Vector2.zero;
                scrollRect.vertical = false;
                scrollRect.content.localPosition = Vector3.up * topBound;
            }

            if (check && startMousePos.y > lastMousePos.y)
            {
                scrollRect.vertical = true;
            }

            lastMousePos = Input.mousePosition;
        }
    }
    private int CalculatePreviousButtonCount(int mapIndex)
    {
        int totalCounter = 0;

        for (int i = 0; i < mapIndex; i++)
        {
            totalCounter += mapPrefabList[i].buttonPositions.Count;
        }

        return totalCounter;
    }
    private Map InstantiateMap(int index, float verticalPosition)
    {
        PoolItem poolItem = PoolManager.instance.TryToGetItem(mapPrefabList[index].GetComponent<PoolItem>().poolItemType);

        poolItem.transform.position = transform.position;
        rectTransform = poolItem.GetComponent<RectTransform>();
        poolItem.transform.SetParent(transform);
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition = new Vector3(0f, verticalPosition, 0f);
        tempMap = poolItem.GetComponent<Map>();
        tempMap.mapIndex = index;
        tempMap.previousButtonCount = CalculatePreviousButtonCount(index);
        mainMenuManager.InitializeButtons(tempMap.GetButtonList(), tempMap.previousButtonCount);
        return tempMap;
    }

    public void CheckMap(Vector2 vector)
    {
        //ArrangeFirstMap();

        if (botMap != null && index > 0 && scrollRect.content.transform.localPosition.y > -botMap.transform.localPosition.y - (mapHeight / 2f) - mapChangeOffset)
        {
            index--;
            moveDest = 0;
        }

        else if (topMap != null && index < maxLevelIndex && scrollRect.content.transform.localPosition.y < -midMap.transform.localPosition.y - (3f * mapHeight / 2f) + mapChangeOffset)
        {
            index++;
            moveDest = 1;
        }

        if (index > 0 && moveDest == 0)
        {
            defaultBotPos = botMap.transform.localPosition;
            defaultMidPos = midMap.transform.localPosition;

            tempMap = midMap;
            midMap = botMap;

            if (topMap != null)
            {
                topMap.SendButtonsBackToPool();
                PoolManager.instance.RemoveItemFromPool(topMap.GetComponent<PoolItem>());
            }

            topMap = tempMap;
            botMap = InstantiateMap(index - 1, defaultBotPos.y - mapHeight).GetComponent<Map>();

            midMap.transform.localPosition = defaultBotPos;
            topMap.transform.localPosition = defaultMidPos;
        }
        else if (index == 0 && moveDest == 0)
        {
            defaultBotPos = botMap.transform.localPosition;
            defaultMidPos = midMap.transform.localPosition;

            tempMap = midMap;
            midMap = botMap;

            if (topMap != null)
            {
                topMap.SendButtonsBackToPool();
                PoolManager.instance.RemoveItemFromPool(topMap.GetComponent<PoolItem>());
            }

            botMap = null;
            topMap = tempMap;

            midMap.transform.localPosition = defaultBotPos;
            topMap.transform.localPosition = defaultMidPos;
        }

        else if (index < maxLevelIndex && moveDest == 1)
        {
            defaultMidPos = midMap.transform.localPosition;
            defaultTopPos = topMap.transform.localPosition;

            tempMap = midMap;
            midMap = topMap;

            if (botMap != null)
            {
                botMap.SendButtonsBackToPool();
                PoolManager.instance.RemoveItemFromPool(botMap.GetComponent<PoolItem>());
            }

            botMap = tempMap;
            topMap = InstantiateMap(index + 1, defaultTopPos.y + mapHeight).GetComponent<Map>();

            midMap.transform.localPosition = defaultTopPos;
            botMap.transform.localPosition = defaultMidPos;
        }

        else if (index == maxLevelIndex && moveDest == 1)
        {
            defaultMidPos = midMap.transform.localPosition;
            defaultTopPos = topMap.transform.localPosition;

            tempMap = midMap;
            midMap = topMap;

            if (botMap != null)
            {
                botMap.SendButtonsBackToPool();
                PoolManager.instance.RemoveItemFromPool(botMap.GetComponent<PoolItem>());
            }

            topMap = null;
            botMap = tempMap;

            midMap.transform.localPosition = defaultTopPos;
            botMap.transform.localPosition = defaultMidPos;
        }

        moveDest = -1;
    }

    public List<Map> GetMaps()
    {
        List<Map> tempMapList = new List<Map>();
        if (botMap != null)
        {
            tempMapList.Add(botMap);
        }
        if (midMap != null)
        {
            tempMapList.Add(midMap);
        }
        if (topMap != null)
        {
            tempMapList.Add(topMap);
        }


        return tempMapList;
    }

    private void ArrangeFirstMap()
    {
        if (midMap!=null && midMap.mapIndex == 0)
        {
            Debug.Log("mid map index = 0");
            midMap.gameObject.GetComponent<Image>().sprite = firstMapSprite;
            List<Transform> firstMapButtonPositions = firstBeachMapPrefab.buttonPositions;
            for (int i = 0; i < midMap.buttonPositions.Count; i++)
            {
                midMap.buttonPositions[i] = firstMapButtonPositions[i];
            }
        }
        else if (botMap != null && botMap.mapIndex == 0)
        {
            Debug.Log("bot map index = 0");
            botMap.gameObject.GetComponent<Image>().sprite = firstMapSprite;
            List<Transform> firstMapButtonPositions = firstBeachMapPrefab.buttonPositions;
            for (int i = 0; i < botMap.buttonPositions.Count; i++)
            {
                botMap.buttonPositions[i] = firstMapButtonPositions[i];
            }
        }
    }
    #endregion
}
