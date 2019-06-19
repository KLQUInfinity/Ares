using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;

[ExecuteInEditMode]
public class FixText : MonoBehaviour
{
    [Multiline]
    [SerializeField] public string text;
    [SerializeField] public bool tashkeel = true;
    [SerializeField] public bool hinduNumbers = true;

    private void Update()
    {
        gameObject.GetComponent<Text>().text = ArabicFixer.Fix(text, tashkeel, hinduNumbers);
    }
}

