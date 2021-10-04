using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoreStackButton : MonoBehaviour
{
    public Button button;

    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_LOADED_REWARDED_AD, param => EnableButton());
        EventDispatcher.Instance.RegisterListener(EventID.ON_FAILED_LOAD_REWARDED_AD, param => DisableButton());
    }

    public void AddMoreStack()
    {
        GoogleAdMobController.Instance.ShowRewardedAd();
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
    }

    public void EnableButton()
    {
        button.interactable = true;
    }

    public void DisableButton()
    {
        button.interactable = false;
    }
}
