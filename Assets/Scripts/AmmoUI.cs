using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace A2
{
    public class AmmoUI : MonoBehaviour
    {
        public Text equipInfo;
        public Character player;
        // Start is called before the first frame update
        void Update()
        {
            equipInfo.text = player.initiativeEquipment.GetShowOnUI();
        }
    }
}
