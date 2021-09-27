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

    public void OnClickButton()
    {
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
