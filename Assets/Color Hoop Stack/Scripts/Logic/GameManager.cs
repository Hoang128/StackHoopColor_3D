using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Performance Params")]
    public int framerate = 60;
    public Vector2Int resolution;

    [Header("Options")]
    public bool SoundEnable = true;
    public bool VibrateEnable = true;

    private void Awake()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = framerate;
        Screen.SetResolution(resolution.x, resolution.y, true);
        GoogleAdMobController.Instance.Init();
#endif
    }
}
