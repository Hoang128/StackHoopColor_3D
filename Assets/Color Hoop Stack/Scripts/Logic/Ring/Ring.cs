using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public RingType ringType = RingType.NONE;
    public MeshRenderer meshRenderer;
    public BoxCollider boxCol;
    [HideInInspector] public bool isMoving = false;

    public void ChangeRingType(RingType ringType)
    {
        this.ringType = ringType;
        meshRenderer.material.color = GameplayMgr.Instance.ringColorConfig.configDic[ringType];
    }

    public void InitRing(RingType ringType)
    {
        ChangeRingType(ringType);
    }
}
