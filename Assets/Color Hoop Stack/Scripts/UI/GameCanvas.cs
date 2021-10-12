using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public WinMenu winMenu;
    public RestartButton restartButton;
    public MoreStackButton moreStackButton;
    public UndoButton undoButton;
    public float winPanelTime = 0.5f;

    private Coroutine enableWinMenu = null;

    // Start is called before the first frame update
    void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_COMPLETE_LEVEL, param => CompleteLevelUI());
        EventDispatcher.Instance.RegisterListener(EventID.ON_INIT_LEVEL, param => InitLevelUI());
    }

    public void InitLevelUI()
    {
        if (enableWinMenu != null)
            StopCoroutine(enableWinMenu);
        winMenu.gameObject.SetActive(false);
        moreStackButton.InitLevel();
    }

    public void CompleteLevelUI()
    {
        enableWinMenu = StartCoroutine(EnableWinMenuAfter(winPanelTime));
    }

    private IEnumerator EnableWinMenuAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        winMenu.gameObject.SetActive(true);
    }
}
