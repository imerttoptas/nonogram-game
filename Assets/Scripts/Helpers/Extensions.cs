using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    #region UI
    public static void SetAlpha(this Image image,float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
    #endregion
    
    public static bool Contains(this GameState currentState, GameState state)
    {
        return ((currentState & state) == state);

    }

    public static float GetScreenRatio()
    {
        return Screen.width / Screen.height;
    }

   

}
