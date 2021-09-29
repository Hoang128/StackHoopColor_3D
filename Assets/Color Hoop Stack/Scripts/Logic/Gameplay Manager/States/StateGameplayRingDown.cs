using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayRingDown : StateGameplay
{
    public StateGameplayRingDown(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Ring ringMove = InputMgr.Instance.ringMove;
        ringMove.rb.isKinematic = false;
        ringMove.isMoving = false;
        ringMove.rb.velocity = new Vector3(0f, -GameplayMgr.Instance.ringDownSpeed, 0f);
        if (InputMgr.Instance.isUndoMove)
            InputMgr.Instance.isUndoMove = false;
        stateMachine.StateChange(GameplayMgr.Instance.stateGameplayIdle);

        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.RING_DOWN], false);
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
}
