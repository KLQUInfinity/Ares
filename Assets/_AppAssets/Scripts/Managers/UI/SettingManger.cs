using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingManger : MonoBehaviour
{
    #region Singleton
    public static SettingManger Instance { get; private set; }

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

    #region Language
    // Variables
    [Header("Language")]
    [HideInInspector] public List<LangSetting> LangSettings;
    [SerializeField] private TMP_Dropdown langDropDown;

    // Methods

    public void OnChangeLanguage(int langIndex)
    {
        string lang = langDropDown.options[langIndex].text.Substring(0, 2).ToLower();
        foreach (LangSetting i in LangSettings)
        {
            i.OnLangStateChanged(lang);
        }
        PlayerPrefs.SetString("Lang", lang);
    }
    #endregion


    private void Start()
    {
        langDropDown.value = langDropDown.options.FindIndex(x => x.text.Substring(0, 2).ToLower().Equals(PlayerPrefs.GetString("Lang")));
    }
}
