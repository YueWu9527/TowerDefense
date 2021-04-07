using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace A2
{
    abstract public class Equipment : MonoBehaviour
    {
        public bool isInitiative { get; protected set; }
        public abstract bool GetRunnable();
        public abstract float GetSpeedAffect();
    }
}