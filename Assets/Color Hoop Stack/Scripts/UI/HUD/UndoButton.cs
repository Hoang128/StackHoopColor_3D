using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public void UndoMove()
    {
        if (GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayIdle)
        {
            InputMgr.Instance.UndoMove();
        }
    }
}
