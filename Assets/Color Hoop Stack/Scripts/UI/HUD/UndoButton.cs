using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UndoButton : MonoBehaviour
{
    [HideInInspector]
    private int UndoTime = 5;
    public TextMeshProUGUI undoNumberText;

    public void UndoMove()
    {
        if (GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayIdle)
        {
            if (InputMgr.Instance.moveStack.Count > 0)
            {
                HandleUndoNumber();

                undoNumberText.text = "<sprite index=" + UndoTime + ">";
                InputMgr.Instance.UndoMove();
                SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
            }
        }
    }

    public void HandleUndoNumber()
    {
        if (UndoTime > 0)
        {
            if (UndoTime == 0)
            {
                UndoTime = 5;
                return;
            }
            UndoTime--;
        }
        else
        {
            UndoTime = 5;
        }
    }
}
