using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayRingUp : StateGameplay
{
    private Ring ringReady = null;
    private RingStack ringStackReady = null;

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
        ringMove.transform.DOMoveY(newRingYPos, (newRingYPos - ringMove.transform.position.y) / gameplayMgr.ringUpSpeed).SetEase(Ease.Linear)
            .OnComplete(() => ChangeToNextState());
        if (ringReady != null)
        {
            ringReady.isMoving = false;
            float newY = -1.123066f + ringStackReady.boxCol.size.z / 2 + ringReady.boxCol.size.z / 2 + ringReady.boxCol.size.z * (ringStackReady.ringStack.Count - 1);
            ringReady.transform.DOMoveY(newY, (ringMove.transform.position.y - newY) / gameplayMgr.ringDownSpeed)
                .SetEase(Ease.Linear).OnComplete(
                    () => ringReady.transform.DOJump(ringReady.transform.position, gameplayMgr.ringJumpPower, 2, gameplayMgr.ringJumpTime));
            SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.RING_DOWN], false);
        }

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

        ringReady = null;
        ringStackReady = null;
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

    public void GetRingReady(Ring ring, RingStack ringStackReady)
    {
        this.ringReady = ring;
        this.ringStackReady = ringStackReady;
    }
}
