using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum actions { kRunAway, kEvade, kGoBack, kInform, kWander }

public class Action : MonoBehaviour {
    public actions actionName;
    Move actionDriver;
    GameObject enemy;
    EnemyAI enemyAI;
    // Use this for initialization
    void Start () {
        enemy = transform.parent.gameObject;
        enemyAI = enemy.GetComponent<EnemyAI>();
        actionDriver = enemyAI.move;

    }

    public void Init(actions _name, Move _move)
    {
        actionName = _name;
        actionDriver = _move;
    }

    public void Behavior(State enemystate)
    {
        if(enemystate.stateName == enemyStates.kOnlySee)
        {
            RunAway();
        }
        if(enemystate.stateName == enemyStates.kOnlyHear)
        {
            Evade();
        }
        if (enemystate.stateName == enemyStates.kSeeAndHear)
        {
            GoBack();
        }
        if (enemystate.stateName == enemyStates.kSeeAndHearAndInform)
        {
            Inform();
        }
        if (enemystate.stateName == enemyStates.kInformed)
        {
            GoBack();
        }
        if (enemystate.stateName == enemyStates.kNothing)
        {
            Wandering();
        }
    }

    // Actions
    void RunAway()
    {
        gameObject.GetComponentInParent<Renderer>().material.color = Color.yellow;
        actionDriver.Moving(enemyAI.currentState, enemyAI.playerPos);
    }

    void Evade()
    {
        gameObject.GetComponentInParent<Renderer>().material.color = Color.yellow;
        actionDriver.Moving(enemyAI.currentState, enemyAI.playerPos);
    }


    void Wandering()
    {
        gameObject.GetComponentInParent<Renderer>().material.color = Color.white;
        actionDriver.Moving(enemyAI.currentState, enemyAI.playerPos);
    }

    void GoBack()
    {
        gameObject.GetComponentInParent<Renderer>().material.color = Color.red;
        actionDriver.Moving(enemyAI.currentState, enemyAI.spawnPos);
    }

    void Inform()
    {
        enemyAI.companion.GetComponent<EnemyAI>().informed = true;
    }
}
