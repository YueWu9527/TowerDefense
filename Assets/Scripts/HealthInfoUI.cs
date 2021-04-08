using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace A2
{
    public class HealthInfoUI : MonoBehaviour
    {
        public Transform healthBarPlayer;
        public Transform healthBarBase;
        public HitReciever hitRecieverPlayer;
        public HitReciever hitRecieverBase;

        // Update is called once per frame
        void Update()
        {
            healthBarPlayer.gameObject.GetComponent<Slider>().value = hitRecieverPlayer.healthState.health/(float)hitRecieverPlayer.healthState.healthMax;
            healthBarBase.gameObject.GetComponent<Slider>().value = hitRecieverBase.healthState.health/(float)hitRecieverBase.healthState.healthMax;
        }
    }
}
