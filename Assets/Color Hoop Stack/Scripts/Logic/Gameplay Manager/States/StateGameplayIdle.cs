using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayIdle : StateGameplay
{
    public StateGameplayIdle(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        gameplayMgr.readyToTouch = true;
    }

    public override void OnHandleInput()
    {
        base.OnHandleInput();
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        if (gameplayMgr.stackCompleteNumber == gameplayMgr.ringTypeNumber)
        {
            stateMachine.StateChange(gameplayMgr.stateGameplayCompleteLevel);
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        gameplayMgr.readyToTouch = false;
    }
}
