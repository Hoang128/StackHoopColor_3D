using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    public void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
    }
}
