using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A2
{
    public class DropLootWhenDeath : MonoBehaviour
    {
        public GameObject objPerfab;
        public int percent;
        public HealthState healthState;
        void Start()
        {
            healthState.actionDeath += Death;
        }

        void Death()
        {
            int count = Random.Range(0,100);
            if(count<percent)
            {
                var obj = Instantiate(objPerfab);
                obj.transform.position = transform.position;
            }
        }
    }
}