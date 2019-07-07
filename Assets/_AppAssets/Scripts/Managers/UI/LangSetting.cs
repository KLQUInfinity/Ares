using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangSetting : MonoBehaviour
{
    [Multiline] [SerializeField] private string arText;
    [Multiline] [SerializeField] private string enText;

    private FixTextMeshPro fixTextMeshPro;
    private FixText fixText;

    private void Start()
    {
        fixTextMeshPro = GetComponent<FixTextMeshPro>();
        fixText = GetComponent<FixText>();
        SettingManger.Instance.LangSettings.Add(this);

        if (!PlayerPrefs.HasKey("Lang"))
        {
            PlayerPrefs.SetString("Lang", "ar");
        }

        if (fixTextMeshPro)
        {
            fixTextMeshPro.text = (PlayerPrefs.GetString("Lang").Equals("ar")) ? arText : enText;
        }
        else if (fixText)
        {
            fixText.text = (PlayerPrefs.GetString("Lang").Equals("ar")) ? arText : enText;
        }
    }

    public void OnLangStateChanged(string lang)
    {
        switch (lang)
        {
            case "ar":
                if (fixTextMeshPro)
                    fixTextMeshPro.text = arText;
                else
                    fixText.text = arText;
                break;
            case "en":
                if (fixTextMeshPro)
                    fixTextMeshPro.text = enText;
                else
                    fixText.text = enText;
                break;
        }
    }

    //public void ChangeTxt(string arText,string enText)
    //{
    //    this.
    //}
}
