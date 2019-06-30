using UnityEngine;
using TMPro;
using ArabicSupport;

[ExecuteInEditMode]
public class FixTextMeshPro : MonoBehaviour
{
    [Multiline] public string text;
    public bool tashkeel = true;
    public bool hinduNumbers = true;

    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = ArabicFixer.Fix(text, tashkeel, hinduNumbers);
    }

}
