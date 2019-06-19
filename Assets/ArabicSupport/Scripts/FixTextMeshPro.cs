using UnityEngine;
using TMPro;
using ArabicSupport;

[ExecuteInEditMode]
public class FixTextMeshPro : MonoBehaviour
{
    [Multiline]
    [SerializeField] public string text;
    [SerializeField] public bool tashkeel = true;
    [SerializeField] public bool hinduNumbers = true;

    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = ArabicFixer.Fix(text, tashkeel, hinduNumbers);
    }

}
