using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    public void GoNextLevel()
    {
        GameplayMgr.Instance.GoToLevel(GameplayMgr.Instance.currentLevel + 1);
    }
}
