using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A2
{
    public class TeamPlayer : MonoBehaviour
    {
        public int teamIndex;
        // Start is called before the first frame update
        void Start()
        {
            CharacterMap.Instance.teams[teamIndex].players.Add(this);
        }

        void OnDestroy()
        {
            CharacterMap.Instance.teams[teamIndex].players.Remove(this);
        }
    }
}
