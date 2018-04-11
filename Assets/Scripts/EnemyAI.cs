using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    // Enemy 
    public EyeSensor eyeSensor;
    public EarSensor earSensor;
    public Move move;

    // Working memory
    public Vector3 spawnPos;
    public Vector3 playerPos;
    public bool seePlayer;
    public bool hearPlayer;
    public bool seeCompanion;
    public bool informed;
    public State currentState;
    public Action currentAction;
    public GameObject companion;

    //for test
    public enemyStates s;
    public actions a;

    // Define States
    State onlySeeState, onlyHearState, seeAndHearState, seeAndHearAndInformState, informedState, nothingState;
    // Define Actions
    Action runAwayAction, evadeAction, goBackAction, informAction, wanderAction;

    SortedList<float, State> statesDetected = new SortedList<float, State>(new DescComparer<float>());

    // Use this for initialization
    void Start () {
        seePlayer = false;
        hearPlayer = false;
        seeCompanion = false;
        informed = false;
        eyeSensor = GetComponentInChildren<EyeSensor>();
        earSensor = GetComponentInChildren<EarSensor>();
        move = GetComponentInChildren<Move>();

        spawnPos = GameObject.Find("SpawnPoint").transform.position;
        spawnPos.y = 0;
       
        currentState = transform.FindChild("state").GetComponentInChildren<State>();
        currentAction = transform.FindChild("action").GetComponentInChildren<Action>();

        onlySeeState = transform.FindChild("onlySee").GetComponentInChildren<State>();
        onlyHearState =transform.FindChild("onlyHear").GetComponentInChildren<State>();
        seeAndHearState = transform.FindChild("seeAndHear").GetComponentInChildren<State>();
        seeAndHearAndInformState =transform.FindChild("seeAndHearAndInform").GetComponentInChildren<State>();
        informedState = transform.FindChild("informed").GetComponentInChildren<State>();
        nothingState = transform.FindChild("nothing").GetComponentInChildren<State>();

        runAwayAction = transform.FindChild("runAway").GetComponentInChildren<Action>();
        evadeAction = transform.FindChild("evade").GetComponentInChildren<Action>();
        goBackAction = transform.FindChild("goBack").GetComponentInChildren<Action>();
        informAction = transform.FindChild("inform").GetComponentInChildren<Action>();
        wanderAction = transform.FindChild("wander").GetComponentInChildren<Action>();

        InitRules();
    }
	
	// Update is called once per frame
	void Update () {
        //Recognize - act cycle
        Detect();

        if (statesDetected.Count == 0)
        {
            DefineState();
        }
        
        currentState = statesDetected.Values[0];
        CheckProduction(currentState);
        statesDetected.Clear();
        currentAction.Behavior(currentState);
    
        s = currentState.stateName;
        a = currentAction.actionName;  
    }

    // Production
    // Rules database
    void InitRules()
    {
        // Define Actions
        runAwayAction.Init(actions.kRunAway, move);
        evadeAction.Init(actions.kEvade, move);
        goBackAction.Init(actions.kGoBack, move);
        informAction.Init(actions.kInform, move);
        wanderAction.Init(actions.kWander, move);
        // Define States
        onlySeeState.Init(enemyStates.kOnlySee, runAwayAction, 0.3f);
        onlyHearState.Init(enemyStates.kOnlyHear, evadeAction, 0.3f);
        seeAndHearState.Init(enemyStates.kSeeAndHear, goBackAction, 0.4f);
        seeAndHearAndInformState.Init(enemyStates.kSeeAndHearAndInform, informAction, 0.2f);
        informedState.Init(enemyStates.kInformed, goBackAction, 0.7f);
        nothingState.Init(enemyStates.kNothing, wanderAction, 0.1f);
        
    }


    void Detect()
    {
        eyeSensor.GetComponent<SphereCollider>().enabled = true;
        seePlayer = eyeSensor.seePlayer;
        seeCompanion = eyeSensor.seeCompanion;

        earSensor.GetComponent<SphereCollider>().enabled = true;
        hearPlayer = earSensor.hearPlayer;
    }
    //match more than one states at the same time:
    //eg. informed is prior to other states

    void CheckProduction(State currentState)
    {
        currentAction = currentState.action;
    }

    public void DefineState()
    {
        if (seePlayer && !hearPlayer)
        {
            if (!statesDetected.ContainsValue(onlySeeState))
                statesDetected.Add(onlySeeState.weight, onlySeeState);
        }
        if (!seePlayer && hearPlayer)
        {
            if(!statesDetected.ContainsValue(onlyHearState))
            statesDetected.Add(onlyHearState.weight, onlyHearState);
        }
        if (seePlayer && hearPlayer)
        {
            if (!statesDetected.ContainsValue(seeAndHearState))
                statesDetected.Add(seeAndHearState.weight, seeAndHearState);
        }
        if (!seePlayer && !hearPlayer)
        {
            if (!statesDetected.ContainsValue(nothingState))
                statesDetected.Add(nothingState.weight, nothingState);
        }
        if (currentState == seeAndHearState && seeCompanion)
        {
            if (!statesDetected.ContainsValue(seeAndHearAndInformState))
                statesDetected.Add(seeAndHearAndInformState.weight, seeAndHearAndInformState);
        }
        if (informed)
        {
            if (!statesDetected.ContainsValue(informedState))
                statesDetected.Add(informedState.weight, informedState);
        }
    }

}

class DescComparer<State> : IComparer<float>
{
    public int Compare(float x, float y)
    {
        return Comparer<float>.Default.Compare(y, x);
    }
}