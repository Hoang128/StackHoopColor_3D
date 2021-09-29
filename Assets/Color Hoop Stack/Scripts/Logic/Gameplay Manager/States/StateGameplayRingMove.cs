using DG.Tweening;
using Newtonsoft.Json;
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

        ringStackStart = InputMgr.Instance.ringStackStart;
        ringStackEnd = InputMgr.Instance.ringStackEnd;

        command = (CommandRingMove)InputMgr.Instance.moveStack.Peek();

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
        int placeOrder = ringStackEnd.ringStack.Count - 1;
        Ring nextRing = null;
        if (ringStackStart.ringStack.Count > 0)
            nextRing = ringStackStart.ringStack.Peek();
        float newRingYPos = ringStackStart.transform.position.y +
            ringStackStart.boxCol.size.y / 2 +
            ringMove.boxCol.size.z / 2;
        if (!InputMgr.Instance.isUndoMove)
        {
            command.ringMoveNumber++;
        }
        else
        {
            command.ringMoveNumber--;
        }

        if (InputMgr.Instance.isUndoMove)
        {
            if (command.ringMoveNumber == 0)
            {
                nextRing = null;
            }
        }
        else
        {
            if (ringStackEnd.ringStack.Count >= 4)
            {
                nextRing = null;
            }
        }

        if (ringMove.transform.position.y != newRingYPos)
        {
            ringMove.transform.DOMoveY(newRingYPos, (newRingYPos - ringMove.transform.position.y) / gameplayMgr.ringUpSpeed)
                .OnComplete(() => BringRingToNewStack(ringMove, nextRing, placeOrder));
        }
        else
        {
            BringRingToNewStack(ringMove, nextRing, placeOrder);
        }
    }

    public void BringRingToNewStack(Ring ringMove, Ring nextRing, int placeOrder)
    {
        Vector3 newPos = new Vector3(ringStackEnd.transform.position.x, ringMove.transform.position.y, ringStackEnd.transform.position.z);
        float distance = Vector3.Distance(ringMove.transform.position, newPos);
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.RING_MOVE], false);
        ringMove.transform.DOMove(newPos, distance / gameplayMgr.ringMoveSpeed).OnComplete(()=>DropRingDown(ringMove, nextRing, placeOrder));

        if (!InputMgr.Instance.isUndoMove)
        {
            if (ringStackStart.ringStack.Count > 0)
            {
                if (ringMove.ringType == ringStackStart.ringStack.Peek().ringType)
                {
                    if (ringStackEnd.ringStack.Count < 4)
                    {
                        MoveRing();
                    }
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

    public void DropRingDown(Ring ring, Ring nextRing, int placeOrder)
    {
        if ((nextRing == null) || (nextRing.ringType != ring.ringType))
        {
            InputMgr.Instance.ringMove = ringMove;
            ActiveCommandRingDown(ringStackEnd, ringMove);
            return;
        }

        Utils.Common.Log("place order = " + placeOrder);
        float newY = -1.123066f + ringStackEnd.boxCol.size.z / 2 + ring.boxCol.size.z / 2 + ring.boxCol.size.z * placeOrder;
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
            if (!ringStackStart.canControl)
            {
                ringStackStart.canControl = true;
            }
            InputMgr.Instance.UndoMove();
        }
        else
        {
            if (ringStackEnd.IsStackFullSameColor())
            {
                GameplayMgr.Instance.stackCompleteNumber++;
                ringStackEnd.canControl = false;
            }
            Command commandRingDown = new CommandRingDown(ringStack, ringMove);
            commandRingDown.Execute();
            InputMgr.Instance.moveStack.Push(commandRingDown);
        }
    }
}
