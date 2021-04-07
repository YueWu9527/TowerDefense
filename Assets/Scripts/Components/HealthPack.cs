using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace A2
{
    public class HealthPack : MonoBehaviour
    {
        public bool isEnable = false;
        public int healthReverseValue = 10;
        void OnTriggerEnter(Collider collider)
        {
            var hitReciever = collider.gameObject.GetComponent<HitReciever>();
            if(hitReciever!=null)
            {
                if(hitReciever.ReverseHealth(healthReverseValue) > 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(waitEnable());
        }

        IEnumerator waitEnable()
        {
            yield return new WaitForSeconds(1);
            isEnable = true;
        }
    }
}
