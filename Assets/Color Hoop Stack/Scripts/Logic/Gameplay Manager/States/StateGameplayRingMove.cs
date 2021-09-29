using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayRingMove : StateGameplay
{
    private RingStack ringStackStart;
    private RingStack ringStackEnd;
    private Ring ringMove;
    public CommandRingMove command;

    public StateGameplayRingMove(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        string json = JsonUtility.ToJson(InputMgr.Instance.moveStack.Peek());
        System.Object @object = JsonUtility.FromJson<CommandRingMove>(json);
        command = (CommandRingMove)@object;

        ringStackStart = InputMgr.Instance.ringStackStart;
        ringStackEnd = InputMgr.Instance.ringStackEnd;

        MoveRing();
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

    public void MoveRing()
    {
        ringMove = ringStackStart.ringStack.Pop();
        ringMove.transform.SetParent(ringStackEnd.transform);
        ringStackEnd.ringStack.Push(ringMove);
        Ring nextRing = null;
        if (ringStackStart.ringStack.Count > 0)
            nextRing = ringStackStart.ringStack.Peek();
        float newRingYPos = ringStackStart.transform.position.y +
            ringStackStart.boxCol.size.y / 2 +
            ringMove.boxCol.size.z / 2;
        if (InputMgr.Instance.isUndoMove)
        {
            command.ringMoveNumber++;
        }
        else
        {
            command.ringMoveNumber--;
        }

        if (ringMove.transform.position.y != newRingYPos)
        {
            ringMove.transform.DOMoveY(newRingYPos, (newRingYPos - ringMove.transform.position.y) / gameplayMgr.ringUpSpeed)
                .OnComplete(() => BringRingToNewStack(ringMove, nextRing));
        }
        else
        {
            BringRingToNewStack(ringMove, nextRing);
        }
    }

    public void BringRingToNewStack(Ring ringMove, Ring nextRing)
    {
        Vector3 newPos = new Vector3(ringStackEnd.transform.position.x, ringMove.transform.position.y, ringStackEnd.transform.position.z);
        float distance = Vector3.Distance(ringMove.transform.position, newPos);
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.RING_MOVE], false);
        ringMove.transform.DOMove(newPos, distance / gameplayMgr.ringMoveSpeed).OnComplete(()=>DropRingDown(ringMove, nextRing));

        if (!InputMgr.Instance.isUndoMove)
        {
            if (ringStackStart.ringStack.Count > 0)
            {
                if (ringMove.ringType == ringStackStart.ringStack.Peek().ringType)
                {
                    MoveRing();
                }
            }
        }
        else
        {
            if (command.ringMoveNumber > 0)
            {
                MoveRing();
            }
        }
    }

    public void DropRingDown(Ring ring, Ring nextRing)
    {
        if (!InputMgr.Instance.isUndoMove)
        {
            if ((nextRing == null) || (nextRing.ringType != ring.ringType))
            {
                ActiveCommandRingDown(ringStackEnd, ringMove);
                return;
            }
        }
        float newY = -1.123066f + ringStackEnd.boxCol.size.z / 2 + ring.boxCol.size.z / 2 + ring.boxCol.size.z * (ringStackEnd.ringStack.Count - 1);
        ring.transform.DOMoveY(newY, (ring.transform.position.y - newY) / gameplayMgr.ringDownSpeed)
            .OnComplete(
                () => ring.transform.DOJump(ring.transform.position, gameplayMgr.ringJumpPower, 2, gameplayMgr.ringJumpTime)
            );
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.RING_DOWN], false);
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
