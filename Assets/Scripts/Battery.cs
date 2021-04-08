using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace A2
{
    public class Battery : MonoBehaviour
    {
        public float secondsToDestorySelf;
        public int power;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CoroutineDestroySelf());
        }

        // Update is called once per frame
        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                PlayerBag.Instance.GetPowerPack(power);
                Destroy(this.gameObject);
            }
        }

        IEnumerator CoroutineDestroySelf()
        {
            yield return new WaitForSeconds(secondsToDestorySelf);
            Destroy(this.gameObject);
        }
    }
}