using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<10;i++)
        {
            Spawn();
        }
        StartCoroutine(waitSpawn(10));
    }

    IEnumerator waitSpawn(float interval)
    {
        yield return new WaitForSeconds(interval);
        Spawn();
        yield return waitSpawn(interval);
    }

    private void Spawn()
    {
        var obj = Instantiate(enemy);
        int x = Random.Range(-90,90);
        int y = Random.Range(-90,90);
        obj.transform.position = new Vector3(x,1,y);
    }
}
