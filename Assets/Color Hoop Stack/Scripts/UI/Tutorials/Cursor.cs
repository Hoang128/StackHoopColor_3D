using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public Camera cam;
    public RectTransform canvasRect;
    public RectTransform cursorRect;
    public float ySpaceMove = 10f;
    public float ySpaceMoveTime;
    public float cursorMoveTime;
    Sequence seq;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_MOVE_TUTORIAL_CURSOR, param => MoveCursor());
    }

    private void OnEnable()
    {
        if (GameplayMgr.Instance.ringStackList.Count > 0)
        {
            float cursorScreenPosX = GetScreenPosXFromWorldPos(GameplayMgr.Instance.ringStackList[0].transform.position);
            this.cursorRect.anchoredPosition = new Vector2(cursorScreenPosX, cursorRect.anchoredPosition.y);
            seq = DOTween.Sequence();
            seq.Append(cursorRect.DOAnchorPosY(cursorRect.anchoredPosition.y + ySpaceMove, ySpaceMoveTime).SetEase(Ease.Linear));
            seq.Append(cursorRect.DOAnchorPosY(cursorRect.anchoredPosition.y, ySpaceMoveTime).SetEase(Ease.Linear));
            seq.SetLoops(-1);
        }
    }

    public float GetScreenPosXFromWorldPos(Vector3 position)
    {
        float cursorViewportPosX = cam.WorldToViewportPoint(position).x;
        float cursorScreenPosX = (cursorViewportPosX * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f);
        return cursorScreenPosX;
    }

    public void MoveCursor()
    {
        seq.Pause();

        float newX = GetScreenPosXFromWorldPos(GameplayMgr.Instance.ringStackList[1].transform.position);
        cursorRect.DOLocalMoveX(newX, cursorMoveTime).SetEase(Ease.Linear).OnComplete(()=> seq.Play());
    }
}
