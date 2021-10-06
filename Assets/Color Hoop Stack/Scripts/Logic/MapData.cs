using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    private List<RingStackData> listRingStack;
    private int stackCompleteNumber = 0;
    private int ringStackNumber = 0;

    public List<RingStackData> ListRingStack { get => listRingStack; set => listRingStack = value; }
    public int StackCompleteNumber { get => stackCompleteNumber; set => stackCompleteNumber = value; }
    public int RingStackNumber { get => ringStackNumber; set => ringStackNumber = value; }

    public MapData(List<RingStack> listRingStack, int stackCompleteNumber, int ringStackNumber)
    {
        this.StackCompleteNumber = stackCompleteNumber;
        this.ringStackNumber = ringStackNumber;
        this.ListRingStack = new List<RingStackData>();

        foreach (RingStack ringStack in listRingStack)
        {
            RingStackData ringStackData = new RingStackData(ringStack);
            this.ListRingStack.Add(ringStackData);
        }
    }

    public MapData()
    {
        this.ListRingStack = new List<RingStackData>();
    }
}
