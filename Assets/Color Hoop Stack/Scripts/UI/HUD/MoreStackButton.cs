using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreStackButton : MonoBehaviour
{
    public void AddMoreStack()
    {
        GameplayMgr.Instance.AddRingStack();
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
    }
}
