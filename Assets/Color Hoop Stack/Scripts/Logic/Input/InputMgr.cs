#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMgr : Singleton<InputMgr>
{
    [HideInInspector] public Ring ringMove;
    [HideInInspector] public RingStack ringStackStart;
    [HideInInspector] public RingStack ringStackEnd;
    [HideInInspector] public Stack<Command> moveStack;
    [HideInInspector] public bool isUndoMove = false;

    private void Awake()
    {
        Instance = this;
        moveStack = new Stack<Command>();
    }

    public void UndoMove()
    {
        if (moveStack.Count > 0)
        {
            if (!isUndoMove)
                isUndoMove = true;
            Command lastMoveCommand = moveStack.Pop();
            lastMoveCommand.Undo();
        }
    }

    public void HandleTap(RingStack ringStackTap)
    {
        if (GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayIdle)
        {
            ringStackStart = ringStackTap;
            ringMove = ringStackStart.ringStack.Peek();

            //command up
            CommandRingUp newMove = new CommandRingUp(ringStackStart, ringMove);
            newMove.Execute();
            moveStack.Push(newMove);
            return;
        }
        else if (GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayRingReady)
        {
            ringStackEnd = ringStackTap;
            if (ringStackEnd.number == ringStackStart.number)
            {
                //command down
                CommandRingDown newMove = new CommandRingDown(ringStackEnd, ringMove);
                newMove.Execute();
                moveStack.Pop();
            }
            else
            {
                if (ringStackEnd.ringStack.Count > 0)
                {
                    if (ringStackEnd.ringStack.Peek().ringType != ringMove.ringType)
                    {
                        //Command down
                        ringStackEnd = ringStackStart;
                        CommandRingDown newMove = new CommandRingDown(ringStackEnd, ringMove);
                        newMove.Execute();
                        moveStack.Pop();
                    }
                    else if (ringStackEnd.ringStack.Count == 4)
                    {
                        //Command down
                        ringStackEnd = ringStackStart;
                        CommandRingDown newMove = new CommandRingDown(ringStackEnd, ringMove);
                        newMove.Execute();
                        moveStack.Pop();
                    }
                    else if (ringStackEnd.ringStack.Peek().ringType == ringMove.ringType)
                    {
                        //Command move
                        CommandRingMove newMove = new CommandRingMove(ringStackStart, ringStackEnd);
                        newMove.Execute();
                        moveStack.Push(newMove);
                    }
                }
                else
                {
                    //Command move
                    Command newMove = new CommandRingMove(ringStackStart, ringStackEnd);
                    newMove.Execute();
                    moveStack.Push(newMove);
                }
            }

            return;
        }
    }
}
