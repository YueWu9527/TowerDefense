using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A2
{
    public class DropLootWhenDeath : MonoBehaviour
    {
        public GameObject healthPack;
        public HealthState healthState;
        void Start()
        {
            healthState.actionDeath += Death;
        }

        void Death()
        {
            int count = Random.Range(0,3);
            for(int i=0;i<count;i++)
            {
                var obj = Instantiate(healthPack);
                obj.transform.position = transform.position;
            }
        }
    }
}