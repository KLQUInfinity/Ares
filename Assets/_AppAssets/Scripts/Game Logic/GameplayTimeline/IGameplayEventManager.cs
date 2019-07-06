using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameplayEventManager
{
    void fireEvent(IGameplayEvent eventObj);
}
