using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    List<RingStackData> listRingStack;

    public MapData(List<RingStack> listRingStack)
    {
        foreach (RingStack ringStack in listRingStack)
        {
            listRingStack.Add(new RingStackData(ringStack));
        }
    }
}
