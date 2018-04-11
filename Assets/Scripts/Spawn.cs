using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public int enemyNumSpawned;
    public int wallsNum;
    public GameObject enemy;
    public GameObject wall;
    GameObject[] enemies;

    GameObject[] wallsV;
    GameObject[] wallsH;
    // Use this for initialization
    void Start () {
        GenerateWalls();
        GenerateEnemies();
    }
	
	// Update is called once per frame


    void GenerateEnemies()
    {
        enemies = new GameObject[enemyNumSpawned];
        for(int i = 0; i < enemyNumSpawned; i++)
        {
            float px = Random.Range(-14f, 14f);
            float py = 0.0f;
            float pz = Random.Range(-4f, 24f);
            Vector3 position = new Vector3(px,py,pz);
            //Vector3 position = new Vector3(0, py, 2);
            float rx = 0.0f;
            float ry = Random.Range(0, 360);
            float rz = 0.0f;
            Quaternion rotation = Quaternion.Euler(rx, ry, rz);
            enemies[i] = (GameObject)Instantiate(enemy, position, rotation);
        }
    }

    void GenerateWalls()
    {
        wallsV = new GameObject[62];
        for (int i = 0; i < 31; i++)
        {
           // float px = Random.Range(-15f, 15f);
            float py = 0.0f;
          //  float pz = Random.Range(-5f, 25f);
            Vector3 position = new Vector3(i-15, py, -5);
            wallsV[i] = (GameObject)Instantiate(wall, position, Quaternion.identity);
            position = new Vector3(i - 15, py, 25);
            wallsV[i+30] = (GameObject)Instantiate(wall, position, Quaternion.identity);

        }
        wallsH = new GameObject[60];
        for (int i = 0; i < 30; i++)
        {
            // float px = Random.Range(-15f, 15f);
            float py = 0.0f;
            //  float pz = Random.Range(-5f, 25f);
            Vector3 position = new Vector3(-15, py, i-4);
            wallsH[i] = (GameObject)Instantiate(wall, position, Quaternion.identity);
            position = new Vector3(15, py, i-4);
            wallsH[i+30] = (GameObject)Instantiate(wall, position, Quaternion.identity);

        }
    }
}
