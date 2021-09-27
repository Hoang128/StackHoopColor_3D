using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRingUp : Command
{
    public RingStack ringStackStart;
    public Ring ring;

    public CommandRingUp(RingStack ringStackStart, Ring ring)
    {
        this.ring = ring;
        this.ringStackStart = ringStackStart;
    }

    public override void Execute()
    {
        base.Execute();
        GameplayMgr.Instance.stateMachine.StateChange(GameplayMgr.Instance.stateGameplayRingUp);
    }

    public override void Undo()
    {
        base.Undo();
        InputMgr.Instance.ringStackEnd = ringStackStart;
        InputMgr.Instance.ringMove = ring;
        GameplayMgr.Instance.stateMachine.StateChange(GameplayMgr.Instance.stateGameplayRingDown);
    }
}
