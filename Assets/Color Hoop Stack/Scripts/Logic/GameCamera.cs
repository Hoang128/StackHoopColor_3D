using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Camera cam;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_RING_STACK_NUMBER_CHANGE, param => OnRingStackNumberChange());
    }

    public void OnRingStackNumberChange()
    {
        int currentStackNumber = GameplayMgr.Instance.ringStackList.Count;
        cam.orthographicSize = GameplayMgr.Instance.stackRowListConfig.stackRowList[currentStackNumber].cameraSize;
    }
}
