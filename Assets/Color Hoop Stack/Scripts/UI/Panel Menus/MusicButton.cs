using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButton : MonoBehaviour
{
    public GameObject cross;

    public void Awake()
    {
        cross.SetActive(false);
    }
    public void OnEnable()
    {
        if (GameManager.Instance.SoundEnable)
        {
            cross.active = false;
        }
        else
        {
            cross.active = true;
        }
    }
    public void OnClickButton()
    {
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
        if (GameManager.Instance.SoundEnable)
        {
            cross.SetActive(true);
            GameManager.Instance.SoundEnable = false;
            return;
        }
        else
        {
            cross.SetActive(false);
            GameManager.Instance.SoundEnable = true;
            return;
        }
    }
}
