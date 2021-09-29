using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public WinMenu winMenu;
    public RestartButton restartButton;
    public MoreStackButton moreStackButton;
    public UndoButton undoButton;

    // Start is called before the first frame update
    void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_COMPLETE_LEVEL, param => CompleteLevelUI());
        EventDispatcher.Instance.RegisterListener(EventID.ON_INIT_LEVEL, param => InitLevelUI());
    }

    public void InitLevelUI()
    {
        winMenu.gameObject.SetActive(false);    
        restartButton.gameObject.SetActive(true);
        moreStackButton.gameObject.SetActive(true);
        undoButton.gameObject.SetActive(true);
    }

    public void CompleteLevelUI()
    {
        winMenu.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        moreStackButton.gameObject.SetActive(false);
        undoButton.gameObject.SetActive(false);
    }
}
