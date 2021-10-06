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
            moveStack.Peek().Undo();
            moveStack.Pop();
        }
    }

    public void HandleTap(RingStack ringStackTap)
    {
        if (GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayIdle)
        {
            ringStackStart = ringStackTap;
            if (ringStackStart.ringStack.Count > 0)
            {
                ringMove = ringStackStart.ringStack.Peek();
                //command up
                Command newMove = new CommandRingUp(ringStackStart, ringMove, null, null);
                newMove.Execute();
                moveStack.Push(newMove);
                return;
            }
        }
        else if (GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayRingReady)
        {
            ringStackEnd = ringStackTap;
            if (ringStackEnd.number == ringStackStart.number)
            {
                //command down
                Command newMove = new CommandRingDown(ringStackEnd, ringMove);
                newMove.Execute();
                moveStack.Pop();
            }
            else
            {
                if (ringStackEnd.ringStack.Count > 0)
                {
                    if (ringStackEnd.ringStack.Peek().ringType != ringMove.ringType)
                    {
                        //Command up
                        RingStack ringStackReady = ringStackStart;
                        Ring ringReady = ringStackReady.ringStack.Peek();
                        ringStackStart = ringStackEnd;
                        ringMove = ringStackStart.ringStack.Peek();

                        Command newMove = new CommandRingUp(ringStackStart, ringMove, ringReady, ringStackReady);
                        
                        newMove.Execute();
                        moveStack.Pop();
                        moveStack.Push(newMove);
                        return;
                    }
                    else if (ringStackEnd.ringStack.Count == 4)
                    {
                        //Command up
                        RingStack ringStackReady = ringStackStart;
                        Ring ringReady = ringStackReady.ringStack.Peek();
                        ringStackStart = ringStackEnd;
                        ringMove = ringStackStart.ringStack.Peek();

                        Command newMove = new CommandRingUp(ringStackStart, ringMove, ringReady, ringStackReady);
                        newMove.Execute();
                        moveStack.Pop();
                        moveStack.Push(newMove);
                        return;
                    }
                    else if (ringStackEnd.ringStack.Peek().ringType == ringMove.ringType)
                    {
                        //Command move
                        Command newMove = new CommandRingMove(ringStackStart, ringStackEnd);
                        moveStack.Push(newMove);
                        newMove.Execute();
                        return;
                    }
                }
                else
                {
                    //Command move
                    Command newMove = new CommandRingMove(ringStackStart, ringStackEnd);
                    moveStack.Push(newMove);
                    newMove.Execute();
                    return;
                }
            }

            return;
        }
    }
}
