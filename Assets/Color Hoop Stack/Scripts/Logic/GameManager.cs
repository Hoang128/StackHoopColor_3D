using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Performance Params")]
    public int framerate = 60;
    public float ratio = 1.5f;

    [Header("Options")]
    public bool SoundEnable = true;
    public bool VibrateEnable = true;

    private void Awake()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = framerate;
        Screen.SetResolution((int)((float)Screen.width / ratio), (int)((float)Screen.height /ratio), true);
        GoogleAdMobController.Instance.Init();
#endif
    }
}
