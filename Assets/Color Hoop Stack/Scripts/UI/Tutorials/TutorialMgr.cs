using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMgr : MonoBehaviour
{
    [SerializeField] private GameObject tutorialText;
    [SerializeField] private GameObject tutorialCursor;
    [SerializeField] private GameObject tutorialCorrect;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_ENABLED_TUTORIAL, param => EnableTutorial());
        EventDispatcher.Instance.RegisterListener(EventID.ON_DISABLED_TUTORIAL, param => DisableTutorial());
    }

    public void EnableTutorial()
    {
        tutorialText.SetActive(true);
        if (GameplayMgr.Instance.currentLevel == 0)
        {
            tutorialCursor.SetActive(true);
        }
        else if (GameplayMgr.Instance.currentLevel == 1)
        {
            tutorialCorrect.SetActive(true);
        }
    }

    public void DisableTutorial()
    {
        if (tutorialText.activeSelf)
        {
            tutorialText.SetActive(false);
        }
        if (tutorialCursor.activeSelf)
        {
            tutorialCursor.SetActive(false);
        }
        if (tutorialCorrect.activeSelf)
        {
            tutorialCorrect.SetActive(false);
        }
    }
}
