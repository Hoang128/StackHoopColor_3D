using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [HideInInspector]
    private int UndoTime = 5;
    public TextMeshProUGUI undoNumberText;
    [SerializeField]
    private GameObject watchAd; 
    private bool riseEnableButtonFlag = false;
    private bool riseDisableButtonFlag = false;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_LOADED_REWARDED_AD, param => EnableButtonAd());
        EventDispatcher.Instance.RegisterListener(EventID.ON_FAILED_LOAD_REWARDED_AD, param => DisableButtonAd());
    }

    private void Update()
    {
        if (riseEnableButtonFlag)
        {
            button.interactable = true;
            riseEnableButtonFlag = false;
            undoNumberText.gameObject.SetActive(true);
        }

        if (riseDisableButtonFlag)
        {
            button.interactable = false;
            riseDisableButtonFlag = false;
            undoNumberText.gameObject.SetActive(false);
        }
    }

    public void UndoMove()
    {
        if ((GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayIdle) ||
            (GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayRingReady))
        {
            if (GameplayMgr.Instance.mapDataStack.Count > 0)
            {
                HandleUndoNumber();

                if (UndoTime == 0)
                {
                    watchAd.SetActive(true);
                }
                else if (UndoTime == 5)
                {
                    watchAd.SetActive(false);
                    GoogleAdMobController.Instance.ShowRewardedAd();
                }

                undoNumberText.text = "<sprite index=" + UndoTime + ">";
                GameplayMgr.Instance.UndoLevel();
                SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
            }
        }
    }

    public void HandleUndoNumber()
    {
        if (UndoTime > 0)
        {
            if (UndoTime == 0)
            {
                UndoTime = 5;
                return;
            }
            UndoTime--;
        }
        else
        {
            UndoTime = 5;
        }
    }

    public void EnableButtonAd()
    {
        if (UndoTime == 0)
        {
            EnableButton();
            Debug.Log("He he me go active OwO !");
        }
    }

    public void DisableButtonAd()
    {
        if (UndoTime == 0)
        {
            DisableButton();
            Debug.Log("He he me go deactive OwO !");
        }
    }

    public void EnableButton()
    {
        riseEnableButtonFlag = true;
        riseDisableButtonFlag = false;

    }

    public void DisableButton()
    {
        riseEnableButtonFlag = false;
        riseDisableButtonFlag = true;
    }
}
