using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyStates { kOnlySee, kOnlyHear, kSeeAndHear, kSeeAndHearAndInform, kInformed, kNothing}

public class State : MonoBehaviour {

    public enemyStates stateName;
    public Action action;
    public float weight;
 
    public void Init(enemyStates _name, Action _action, float _weight)
    {
        stateName = _name;
        action = _action;
        weight = _weight;
    }

}
