using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MenuPanel
{
    public GameObject ratePanel;
    public GameObject policyPanel;

    public void OpenRatePanel()
    {
        ratePanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OpenPolicyPanel()
    {
        policyPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
