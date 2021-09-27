using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public RingType ringType = RingType.NONE;
    private MeshRenderer meshRenderer;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public BoxCollider boxCol;
    [HideInInspector] public bool isMoving = false;

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        boxCol = GetComponentInChildren<BoxCollider>();
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeRingType(RingType ringType)
    {
        this.ringType = ringType;
        meshRenderer.material = GameplayMgr.Instance.ringTypeConfig.configDic[ringType];
    }

    public void InitRing(RingType ringType)
    {
        ChangeRingType(ringType);
        FreezeGroundPositions();
    }

    public void FreezeGroundPositions()
    {
        rb.constraints = 
            RigidbodyConstraints.FreezePositionX | 
            RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;
    }

    public void FreeGroundPositions()
    {
        rb.constraints = 
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;
    }
}
