using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public NextLevelButton nextLevelButton;
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
        nextLevelButton.gameObject.SetActive(false);
        moreStackButton.gameObject.SetActive(true);
        undoButton.gameObject.SetActive(true);
    }

    public void CompleteLevelUI()
    {
        nextLevelButton.gameObject.SetActive(true);
        moreStackButton.gameObject.SetActive(false);
        undoButton.gameObject.SetActive(false);
    }
}
