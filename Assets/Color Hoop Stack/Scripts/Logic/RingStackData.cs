using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingStackData
{
    private List<RingType> ringList;
    private bool canControl;
    private int number;

    public RingStackData()
    {
        this.ringList = new List<RingType>();
        this.canControl = true;
        this.number = 0;
    }

    public RingStackData(RingStack ringStack)
    {
        this.CanControl = ringStack.canControl;
        this.number = ringStack.number;
        this.ringList = new List<RingType>();

        if (ringStack.ringStack.Count > 0)
        {
            foreach (Ring ring in ringStack.ringStack)
            {
                this.ringList.Add(ring.ringType);
            }
        }
    }

    public List<RingType> RingList { get => ringList; set => ringList = value; }
    public bool CanControl { get => canControl; set => canControl = value; }
    public int Number { get => number; set => number = value; }
}
