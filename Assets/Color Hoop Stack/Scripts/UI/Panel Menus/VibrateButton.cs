using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateButton : MonoBehaviour
{
    public GameObject cross;

    public void Awake()
    {
        cross.SetActive(false);
    }
    public void OnClickButton()
    {
        if (GameManager.Instance.VibrateEnable)
        {
            cross.SetActive(true);
            GameManager.Instance.VibrateEnable = false;
            return;
        }
        else
        {
            cross.SetActive(false);
            GameManager.Instance.VibrateEnable = true;
            return;
        }
    }
}
