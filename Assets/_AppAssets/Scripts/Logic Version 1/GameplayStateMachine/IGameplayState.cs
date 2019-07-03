using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameplayState
{
    void OnStateEnter();
    void OnStateUpdate();
    void OnStateExit();
    string ToString();
    GameplayState GetStateName();
}
