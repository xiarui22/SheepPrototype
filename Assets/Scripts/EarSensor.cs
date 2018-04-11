using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarSensor : MonoBehaviour {

    public bool hearPlayer;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            hearPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            hearPlayer = false;
        }
    }
}
