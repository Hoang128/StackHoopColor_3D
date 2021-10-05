#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMgr : Singleton<GameplayMgr>
{
    [Header("Game Design Datas")]
    public RingTypeConfig ringTypeConfig;
    public LevelListConfig levelListConfig;
    public StackRowListConfig stackRowListConfig;

    [Header("Ring Speed")]
    public float ringUpSpeed = 0.2f;
    public float ringMoveSpeed = 0.5f;
    public float ringDownSpeed = 0.2f;
    public float ringJumpTime = 0.05f;
    public float ringJumpPower = 0.05f;

    [Header("Positions")]
    public Vector2 ringStackDistance;

    [Header("Debug Infos")]
    public int currentLevel = 0;


    [HideInInspector] public StateGameplay stateGameplayInit;
    [HideInInspector] public StateGameplay stateGameplayIdle;
    [HideInInspector] public StateGameplay stateGameplayRingUp;
    [HideInInspector] public StateGameplay stateGameplayRingReady;
    [HideInInspector] public StateGameplay stateGameplayRingMove;
    [HideInInspector] public StateGameplay stateGameplayRingDown;
    [HideInInspector] public StateGameplay stateGameplayCompleteLevel;
    [HideInInspector] public StateGameplay stateGameplayEnd;
    [HideInInspector] public StateGameplay stateGameplayAddStack;

    [HideInInspector] public StateMachine stateMachine;

    [HideInInspector] public List<RingStack> ringStackList;
    [HideInInspector] public bool touched = false;
    [HideInInspector] public bool readyToTouch = false;
    [HideInInspector] public bool firstLoad = true;
    
    public int ringTypeNumber = 0;
    public int stackCompleteNumber = 0;

    private void Awake()
    {

        ringStackList = new List<RingStack>();

        stateMachine = new StateMachine();

        stateGameplayInit = new StateGameplayInit(this, stateMachine);
        stateGameplayIdle = new StateGameplayIdle(this, stateMachine);
        stateGameplayRingUp = new StateGameplayRingUp(this, stateMachine);
        stateGameplayRingReady = new StateGameplayRingReady(this, stateMachine);
        stateGameplayRingMove = new StateGameplayRingMove(this, stateMachine);
        stateGameplayRingDown = new StateGameplayRingDown(this, stateMachine);
        stateGameplayCompleteLevel = new StateGameplayCompleteLevel(this, stateMachine);
        stateGameplayEnd = new StateGameplayEnd(this, stateMachine);
        stateGameplayAddStack = new StateGameplayAddStack(this, stateMachine);
    }

    // Start is called before the first frame update
    private void Start()
    {
        GoogleAdMobController.Instance.Init();
        FileHandler fileHandler = new FileHandler();
        if (!fileHandler.IsSaveDataExist())
        {
            fileHandler.SaveDefaultData();
            
        }
        fileHandler.ReadData();

        stateMachine.StateChange(stateGameplayInit);
    }

    // Update is called once per frame
    private void Update()
    {
        stateMachine.StateHandleInput(); 
        stateMachine.StateLogicUpdate();
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void SetUpRingStacksPosition(int ringStackPerRow, int ringStackNumber)
    {
        int ringStackPerColumn = (int)Mathf.Ceil((float)ringStackNumber/(float)ringStackPerRow);
        Vector3 startPos = new Vector3(
            - (float)(ringStackPerRow - 1) / 2 * ringStackDistance.x, 
            0f, 
            (float)(ringStackPerColumn - 1) / 2 * ringStackDistance.y);

        int ringStackListCurrent = 0;

        for (int i = 0; i < ringStackPerColumn; i++)
        {
            for (int j = 0; j < ringStackPerRow; j++)
            {
                ringStackList[ringStackListCurrent].transform.position = startPos + new Vector3(j * ringStackDistance.x, 0f, i * -ringStackDistance.y);
                ringStackListCurrent++;
                if (ringStackListCurrent >= ringStackList.Count)
                    break;
            }
            if (ringStackListCurrent >= ringStackList.Count)
                break;
        }

        EventDispatcher.Instance.PostEvent(EventID.ON_RING_STACK_NUMBER_CHANGE);
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void AddRingStack()
    {
        if (ringStackList.Count < 20)
        {
            GameObject newRingStack = PoolerMgr.Instance.ringStackPooler.GetNextRingStack();
            RingStack newRingStackComp = newRingStack.GetComponent<RingStack>();
            ringStackList.Add(newRingStackComp);
            newRingStackComp.number = ringStackList.Count - 1;
            int ringStackNumber = ringStackList.Count;
            int ringStackPerRow = stackRowListConfig.stackRowList[ringStackList.Count].maxStackInRow;
            SetUpRingStacksPosition(ringStackPerRow, ringStackNumber);
        }
        else
        {
            Utils.Common.Log("Reached ring stack number limit!");
        }
    }

    public void ChangeStateToAddStack()
    {
        stateMachine.StateChange(stateGameplayAddStack);
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void GoToLevel(int level)
    {
        currentLevel = level;
        stateMachine.StateChange(stateGameplayEnd);
        stateMachine.StateChange(stateGameplayInit);
    }

    public void GoToLevel(int level, float goAfterSeconds)
    {
        StartCoroutine(GoToLevelAfter(level, goAfterSeconds));
    }

    private IEnumerator GoToLevelAfter(int level, float goAfterSeconds)
    {
        yield return new WaitForSeconds(goAfterSeconds);
        GoToLevel(level);
    }

#if UNITY_EDITOR
    [Button]
    public void GoNextLevel()
    {
        currentLevel++;
        stateMachine.StateChange(stateGameplayEnd);
        stateMachine.StateChange(stateGameplayInit);
    }

    [Button]
    public void GoPreviousLevel()
    {
        currentLevel--;
        stateMachine.StateChange(stateGameplayEnd);
        stateMachine.StateChange(stateGameplayInit);
    }
#endif

    public void TriggerCompleteLevelEffect()
    {
        float effectYPos = ringStackList[0].transform.position.y +
            ringStackList[0].boxCol.size.y / 2 +
            0.15f;
        foreach (RingStack ringStack in ringStackList)
        {
            if (ringStack.ringStack.Count == 4)
            {
                GameObject particleGO = PoolerMgr.Instance.VFXCompletePooler.GetNextPS();
                particleGO.transform.position = new Vector3(ringStack.transform.position.x, effectYPos, ringStack.transform.position.z);
            }
        }
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.FULL_ALL], false);
    }

    public void TriggerCompleteLevelEffect(float afterSeconds)
    {
        StartCoroutine(TriggerCompleteLevelEffectAfter(afterSeconds));
    }

    private IEnumerator TriggerCompleteLevelEffectAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        TriggerCompleteLevelEffect();
    }

    public void GetRingTypeNumber(int level)
    {
        LevelConfig levelConfig = levelListConfig.levelList[level];
        List<RingType> ringTypeList = new List<RingType>();

        foreach(RingStackList ringStackList in levelConfig.ringStackList)
        {
            foreach(RingType ringType in ringStackList.ringList)
            {
                bool isDuplicated = false;
                if (ringType == RingType.NONE)
                {
                    continue;
                }
                if (ringTypeList.Count == 0)
                {
                    ringTypeList.Add(ringType);
                    continue;
                }
                foreach(RingType currentRingType in ringTypeList)
                {
                    if (currentRingType == ringType)
                    {
                        isDuplicated = true;
                        break;
                    }
                }
                if (!isDuplicated)
                {
                    ringTypeList.Add(ringType);
                }
            }
        }

        ringTypeNumber = ringTypeList.Count;
    }
}
