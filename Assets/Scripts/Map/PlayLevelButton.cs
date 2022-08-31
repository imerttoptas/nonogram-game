using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayLevelButton : MonoBehaviour
{

    public Button levelButton;
    public bool unlocked;
    [SerializeField] Sprite[] LevelButtonSprites;
    [SerializeField] Image[] emptyStars;
    [SerializeField] Image[] stars;
    public int indexLevel;
    public LevelData levelData;
    public Text levelInfoText;

    public void initalize()
    {
        levelButton.image.sprite = LevelButtonSprites[0];
        for (int i = 0; i < emptyStars.Length; i++)
        {
            emptyStars[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {
            stars[i].gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        levelButton.onClick.RemoveAllListeners();
    }
    public void SetLevelButton()
    {
        if (LevelManager.instance.UserData.lastLevelReached >= indexLevel)
        {
            levelButton.interactable = true;
            levelButton.image.sprite = LevelButtonSprites[1];
            for (int i = 0; i < emptyStars.Length; i++)
            {
                emptyStars[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < LevelManager.instance.UserData.levelDataList[indexLevel].stars; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
        }

        levelInfoText.text = (indexLevel + 1).ToString();

    }

}
