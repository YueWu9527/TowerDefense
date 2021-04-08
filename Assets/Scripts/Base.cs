using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A2
{
    public class Base : Singleton<Base>
    {
        public int power { get; private set;}
        public void Charge(int value)
        {
            power += value;
        }
    }
}