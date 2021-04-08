using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A2
{
    public class BaseChargeBtn : MonoBehaviour
    {

        // Update is called once per frame
        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                PlayerBag.Instance.ChargeToBase();
            }
        }
    }
}