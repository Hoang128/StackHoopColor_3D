using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayRingMove : StateGameplay
{
    public StateGameplayRingMove(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        RingStack ringStackStart = InputMgr.Instance.ringStackStart;
        RingStack ringStackEnd = InputMgr.Instance.ringStackEnd;
        Ring ringMove = InputMgr.Instance.ringMove;
        ringStackStart.ringStack.Pop();
        ringMove.transform.SetParent(ringStackEnd.transform);
        ringStackEnd.ringStack.Push(ringMove);

        float newRingYPos = ringStackStart.transform.position.y +
            ringStackStart.boxCol.size.y / 2 +
            ringMove.boxCol.size.z / 2;
        Vector3 newPos = new Vector3(ringStackEnd.transform.position.x, newRingYPos, ringStackEnd.transform.position.z);
        float distance = Vector3.Distance(ringMove.transform.position, newPos);
        InputMgr.Instance.ringMove.transform.DOMove(newPos, distance / GameplayMgr.Instance.ringMoveSpeed).SetEase(Ease.OutCubic)
            .OnComplete(() => ActiveCommandRingDown(ringStackEnd, ringMove));
        if (ringStackEnd.IsStackFullSameColor())
        {
            ringStackEnd.canControl = false;
            gameplayMgr.stackCompleteNumber++;
            SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.FULL_STACK], false);
            GameObject particleGO = PoolerMgr.Instance.VFXCompletePooler.GetNextPS();
            particleGO.transform.position = newPos;
        }

        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.RING_MOVE], false);
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

    public void ActiveCommandRingDown(RingStack ringStack, Ring ringMove)
    {
        if (InputMgr.Instance.isUndoMove)
        {
            InputMgr.Instance.UndoMove();
        }
        else
        {
            Command commandRingDown = new CommandRingDown(ringStack, ringMove);
            commandRingDown.Execute();
            InputMgr.Instance.moveStack.Push(commandRingDown);
        }
    }
}
