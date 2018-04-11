using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSensor : MonoBehaviour {

    public bool seePlayer;
    public GameObject player;

    public bool seeCompanion;
    GameObject companion;

    GameObject enemyAI;
    Vector3 enemyStartDir;
    Vector3 enemyDir;
    Vector3 playerPosToEnemy;
    Vector3 companionPosToEnemy;

    // Use this for initialization
    void Start () {
        enemyAI = gameObject.transform.parent.gameObject;
        enemyStartDir = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            playerPosToEnemy = player.transform.position - enemyAI.transform.position;
            if (CalculateAngle(playerPosToEnemy) > 0.5)
            {
                enemyAI.GetComponent<EnemyAI>().playerPos = player.transform.position;
                seePlayer = true;
            }
        }
        if (other.tag == "Enemy")
        {
            companionPosToEnemy = enemyAI.transform.position - other.gameObject.transform.position;
            if (CalculateAngle(companionPosToEnemy) > 0.5)
            {
                enemyAI.GetComponent<EnemyAI>().companion = other.gameObject;
                seeCompanion = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            seePlayer = false;
        }
        if (other.tag == "Enemy")
        {
            seeCompanion = false;
        }
    }

    double CalculateAngle(Vector3 a)
    {
        a = Vector3.Normalize(a);
        enemyDir = Vector3.Normalize(Quaternion.Euler(0, enemyAI.transform.rotation.eulerAngles.y, 0) * enemyStartDir);
        double ret = Vector3.Dot(a, enemyDir);
        return ret;
    }


}
