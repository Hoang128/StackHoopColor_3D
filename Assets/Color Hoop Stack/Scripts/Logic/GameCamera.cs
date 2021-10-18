#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Camera cam;
    public Vector2 defaultCamSizeRatio;

    private float camRatio;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_RING_STACK_NUMBER_CHANGE, param => OnRingStackNumberChange());
        camRatio = 1/cam.aspect;
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void OnRingStackNumberChange()
    {
        int currentStackNumber = GameplayMgr.Instance.ringStackList.Count;
        camRatio = 1 / cam.aspect;
        cam.orthographicSize = GameplayMgr.Instance.stackRowListConfig.stackRowList[currentStackNumber].cameraSize / (defaultCamSizeRatio.y / defaultCamSizeRatio.x) * camRatio;
    }
}
