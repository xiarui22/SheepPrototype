using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public Vector3 currentV;
    public Vector3 desiredV;
    public float maxV = 0.2f;
    public Vector3 steering;
    public float maxF = 0.2f;
    public float m = 1;

    public Rigidbody rb;
    public float ciricleDistance = 1f;
    public float circleR = 0.5f;

    public float slowingRadius;

    public Vector3 targetCollisionFromAI;

    PathFinder AStarDriver;

    bool walkA;

    // Use this for initialization
    void Start () {
        rb = GetComponentInParent<Rigidbody>();
        rb.velocity = new Vector3(0, 0,0);
        targetCollisionFromAI = new Vector3(-100, 0, 0);

        AStarDriver = GetComponentInChildren<PathFinder>();

        walkA = false;
    }
	
	// Update is called once per frame
	void Update () {
        currentV = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
        currentV = Truncate(currentV, maxV);
        rb.velocity = new Vector3(currentV.x, 0, currentV.z);

        
    }

    public Vector3 RunAwaySteering(Vector3 target)
    {
        Vector3 start = gameObject.transform.parent.gameObject.transform.position;
        desiredV = (start - target).normalized * maxV;
        steering = desiredV - currentV;

        steering = Truncate(steering, maxF);
        steering = steering / m;
        //print(steering);
        //currentV = Truncate(currentV + steering, maxV);
        Vector3 end = start + currentV;

        //return end;
        //return currentV;

        return steering;
    }

    public Vector3 EvadeSteering(Vector3 target)
    {
        Vector3 circleCenter;
        circleCenter = currentV;
        circleCenter = circleCenter.normalized;
        circleCenter *= ciricleDistance;

        Vector3 RandomPlace;
        RandomPlace = circleCenter.normalized * circleR;
        int angle = Random.Range(0, 360);     
        RandomPlace = Quaternion.Euler(0, 0, angle) * RandomPlace;

        Vector3 wanderF = circleCenter + RandomPlace;
        if(Vector3.Dot(Vector3.Normalize(wanderF), Vector3.Normalize(gameObject.transform.parent.gameObject.transform.position)) < 0.5)
        {
            wanderF = circleCenter;
        }

        Vector3 start = gameObject.transform.parent.gameObject.transform.position;

        steering = wanderF;

        steering = Truncate(steering, maxF);
        steering = steering / m;


        Vector3 wanderTmpV = currentV;
        if (targetCollisionFromAI.x != -1)
        {
            Vector3 tmpS = steering;
            Vector3 tmpRS = RunAwaySteering(targetCollisionFromAI);

            steering = tmpS + tmpRS;
        }
        currentV = Truncate(currentV + steering, maxV);

        //if (otherA)
        //{
        //    if ((otherA.transform.position - start).magnitude < 0.5)
        //    {
        //        Vector3 tmpS = steering;
        //        Vector3 tmpES = EvadeSteering(otherA.GetComponent<OtherAI>().m);
        //        steering = tmpS + tmpES;                                        //subsume

        //    }
        //}
        //currentV = currentV + tmpV;
        Vector3 end = start + currentV;
        //print(steering);
        return steering;
    }

    public Vector3 WanderSteering()
    {
        Vector3 circleCenter;
        circleCenter = currentV;
        circleCenter = circleCenter.normalized;
        circleCenter *= ciricleDistance;

        Vector3 RandomPlace;
        RandomPlace = circleCenter.normalized * circleR;
        int angle = Random.Range(0, 360);
        RandomPlace = Quaternion.Euler(0, 0, angle) * RandomPlace;

        Vector3 wanderF = circleCenter + RandomPlace;

        Vector3 start = gameObject.transform.parent.gameObject.transform.position;

        steering = wanderF;

        steering = Truncate(steering, maxF);
        steering = steering / m;


         Vector3 wanderTmpV = currentV;
        if (targetCollisionFromAI.x != -100)
        {
            Vector3 tmpS = steering;
            Vector3 tmpRS = RunAwaySteering(targetCollisionFromAI);

            steering = tmpS + tmpRS;
        }
        // currentV = Truncate(currentV + steering, maxV);

        //if (otherA)
        //{
        //    if ((otherA.transform.position - start).magnitude < 0.5)
        //    {
        //        Vector3 tmpS = steering;
        //        Vector3 tmpES = EvadeSteering(otherA.GetComponent<OtherAI>().m);
        //        steering = tmpS + tmpES;                                        //subsume

        //    }
        //}
        //currentV = currentV + tmpV;
        Vector3 end = start + currentV;
        //print(steering);
        return steering;
    }
    void GoBackAStar(Vector3 target){
        AStarDriver.FindingPath(gameObject.transform.parent.gameObject.transform.position, target);
    }


    public Vector3 GoBackPoint(Vector3 target)
    {
        Vector3 start = gameObject.transform.parent.gameObject.transform.position;
        desiredV = target - start;
        float distance = (desiredV).magnitude;

        if (distance < slowingRadius)
        {
            // Inside the slowing area
            desiredV = desiredV.normalized * maxV * (distance / slowingRadius);
        }
        else
        {
            // Outside the slowing area.
            desiredV = (target - start).normalized * maxV;
        }


        steering = desiredV - currentV;

        steering = Truncate(steering, maxF);
        steering = steering / m;

        //currentV = Truncate(currentV + steering, maxV);
        Vector3 end = start + currentV;
        //gameObject.transform.parent.gameObject.transform.position = end;
        //return currentV;
        //return end;
        //print(currentV);

        return steering;
    }

    void InformOthers()
    {

    }

    public void Moving(State a, Vector3 target)
    {
        int state = (int)a.stateName;
        if (state == 0)
        {
            rb.AddForce(10 * RunAwaySteering(target));

        }
        else if (state == 1)
        {
            rb.AddForce(10 * EvadeSteering(target));

        }
        else if (state == 2)
        {
            GoBackAStar(target);
            rb.AddForce(10 * GoBackPoint(target));
        }
        else if (state == 3)
        {

        }
        else if (state == 4)
        {
            GoBackAStar(target);
            rb.AddForce(10 * GoBackPoint(target));
        }
        else if (state == 5)
        {
            rb.AddForce(15 * WanderSteering());
        }
    }

    Vector3 Truncate(Vector3 v, float max)
    {
        float i;
        i = max / v.magnitude;
        if (i >= 1) i = 1;
        v = v * i;

        return v;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Obstacle")
        {
            print(other.tag);
            targetCollisionFromAI = other.gameObject.transform.position;
        }
    }
}
