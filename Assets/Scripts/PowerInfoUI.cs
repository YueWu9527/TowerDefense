using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A2
{
    public class PowerInfoUI : MonoBehaviour
    {
        public UnityEngine.UI.Text basePower;
        public UnityEngine.UI.Text playerPower;
        // Update is called once per frame
        void Update()
        {
            basePower.text = "Base Power : " + Base.Instance.power.ToString();
            playerPower.text = "Player Power : " + PlayerBag.Instance.power.ToString();
        }
    }
}