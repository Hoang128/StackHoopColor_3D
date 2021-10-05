using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingStackData
{
    private Stack<Ring> ringStack;
    private bool canControl;
    private int number;

    public RingStackData()
    {
        ringStack = new Stack<Ring>();
        canControl = true;
        number = 0;
    }

    public RingStackData(RingStack ringStack)
    {
        this.canControl = ringStack.canControl;
        this.number = ringStack.number;
        
        foreach (Ring ring in ringStack.ringStack)
        {
            this.ringStack.Push(ring);
        }    
    }
}
