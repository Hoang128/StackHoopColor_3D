using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayRingUp : StateGameplay
{
    public StateGameplayRingUp(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        RingStack ringStackStart = InputMgr.Instance.ringStackStart;
        Ring ringMove = InputMgr.Instance.ringMove;
        ringMove.isMoving = true;

        float newRingYPos = ringStackStart.transform.position.y + 
            ringStackStart.boxCol.size.y / 2 + 
            ringMove.boxCol.size.z / 2;
        InputMgr.Instance.ringMove.transform.DOMoveY(newRingYPos, 0.2f).SetEase(Ease.OutCubic)
            .OnComplete(() => ChangeToNextState());
        ringMove.rb.isKinematic = true;

        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.RING_UP], false);
    }

    public override void OnHandleInput()
    {
        base.OnHandleInput();
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void ChangeToNextState()
    {
        if (InputMgr.Instance.isUndoMove)
        {
            InputMgr.Instance.UndoMove();
        }
        else
        {
            stateMachine.StateChange(gameplayMgr.stateGameplayRingReady);
        }
    }
}
