using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;

[ExecuteInEditMode]
public class FixText : MonoBehaviour
{
    [Multiline] public string text;
    public bool tashkeel = true;
    public bool hinduNumbers = true;

    private void Update()
    {
        gameObject.GetComponent<Text>().text = ArabicFixer.Fix(text, tashkeel, hinduNumbers);
    }
}

