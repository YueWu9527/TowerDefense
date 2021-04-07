using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A2
{
    abstract public class InitiativeEquipment : Equipment, IInitiativeEquipment
    {
        public InitiativeEquipment()
        {
            isInitiative = true;
        }

        virtual public void FunctionBtnInput(Character character, BtnType btnType, BtnInputType btnInputType){}
        abstract public System.String GetShowOnUI();
    }

    public interface IInitiativeEquipment {
        void FunctionBtnInput(Character character, BtnType btnType, BtnInputType btnInputType);
        System.String GetShowOnUI();
    }
}