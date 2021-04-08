using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject enemy;
    public float secondsPerRound = 15;
    public int round { get;private set; }

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<10;i++)
        {
            //Spawn();
        }
        StartCoroutine(waitSpawn(secondsPerRound));
    }

    IEnumerator waitSpawn(float interval)
    {
        yield return new WaitForSeconds(interval);
        Spawn();
        yield return waitSpawn(interval);
    }

    private void Spawn()
    {
        Debug.Log(GetEnemyNum(round));
        for(int i=0;i<GetEnemyNum(round);i++)
        {
            var obj = Instantiate(enemy);
            int x = Random.Range(-90,90);
            int y = Random.Range(-90,90);
            obj.transform.position = new Vector3(x,1,y);
        }
        round++;
    }

    private int GetEnemyNum(int roundNum)
    {
        return roundNum + 1;
    }
}