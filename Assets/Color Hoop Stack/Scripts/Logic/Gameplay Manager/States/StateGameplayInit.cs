using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayInit : StateGameplay
{
    public StateGameplayInit(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        InitLevel();
        if (gameplayMgr.firstLoad)
        {
            gameplayMgr.firstLoad = false;
            return;
        }
        else
        {
            GoogleAdMobController.Instance.ShowInterstitialAd();
        }
        GoogleAdMobController.Instance.RequestAndLoadRewardedAd();
    }

    public override void OnHandleInput()
    {
        base.OnHandleInput();
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        stateMachine.StateChange(gameplayMgr.stateGameplayIdle);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void InitLevel()
    {
        FileHandler fileHandler = new FileHandler();
        fileHandler.SaveData();

        int ringStackNumber = gameplayMgr.levelListConfig.levelList[gameplayMgr.currentLevel].ringStackList.Count;
        int ringStackPerRow = gameplayMgr.stackRowListConfig.stackRowList[ringStackNumber].maxStackInRow;
        InputMgr.Instance.moveStack.Clear();
        gameplayMgr.stackCompleteNumber = 0;
        gameplayMgr.GetRingTypeNumber(gameplayMgr.currentLevel);

        for (int i = 0; i < ringStackNumber; i++)
        {
            GameObject newRingStack = PoolerMgr.Instance.ringStackPooler.GetNextRingStack();
            RingStack newRingStackComp = newRingStack.GetComponent<RingStack>();
            gameplayMgr.ringStackList.Add(newRingStackComp);
            newRingStackComp.number = i;
            
        }

        gameplayMgr.SetUpRingStacksPosition(ringStackPerRow, ringStackNumber);

        for (int i = 0; i < gameplayMgr.ringStackList.Count; i++)
        {
            RingStackList ringStackList = gameplayMgr.levelListConfig.levelList[gameplayMgr.currentLevel].ringStackList[i];
            for (int j = 0; j < ringStackList.ringList.Count; j++)
            {
                RingStack ringStack = gameplayMgr.ringStackList[i];

                if (!(ringStackList.ringList[j] == RingType.NONE))
                {
                    GameObject newRing = PoolerMgr.Instance.ringPooler.GetNextRing(ringStackList.ringList[j]);
                    Ring newRingComp = newRing.GetComponent<Ring>();
                    newRing.transform.position = new Vector3(
                        ringStack.transform.position.x,
                        -1.123066f + ringStack.boxCol.size.z/2 + newRingComp.boxCol.size.z/2 + newRingComp.boxCol.size.z * j,
                        ringStack.transform.position.z
                        );

                    gameplayMgr.ringStackList[i].AddNewRing(newRingComp);
                }
            }
        }

        EventDispatcher.Instance.PostEvent(EventID.ON_INIT_LEVEL);
    }
}
