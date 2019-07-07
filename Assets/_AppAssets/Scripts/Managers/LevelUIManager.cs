using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : UIManager
{
    #region Singleton
    public static LevelUIManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public Color GoodColor;
    public Color MiddleColor;
    public Color BadColor;
}
