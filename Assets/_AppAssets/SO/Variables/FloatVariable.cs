using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "SO/Variables/FloatVariable")]
public class FloatVariable : ScriptableObject
{
    [Range(0,1)]
    public float Value;

    //public UnityEngine.Events.UnityEvent onValueChanged;

    //public float value
    //{
    //    get
    //    {
    //        return value;
    //    }
    //    set {
    //        if(value != this.value)
    //        {
    //            set(value);
    //        }
    //    }
    //}

    public void Add(float num)
    {
        if (!((Value+num)>1f) && !((Value + num)< 0f))
        {
            Value += num;
        }
    }

    public void Set(float num)
    {
        Value = num;
        //onValueChanged.Invoke();
    }
}
