using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace A2
{
    public class ThirdPersonAnimCtrl : MonoBehaviour
    {
        public Character character;
        public Gun gun;
        
        public Animator thirdPersonAnim;

        private void Start() 
        {
            init();
        }

        public void init()
        {
            character.actionReportAxis += ReportAxis;

            gun.actionShoot += Shoot;
            gun.actionReload += Reload;
        }

        private void Shoot(bool isAim)
        {
            //thirdPersonAnim.Play("Fire", 0, 0f);
        }

        private void Reload(bool isSingle, bool isUniqueMode)
        {
            thirdPersonAnim.Play("Reload", 1, 0f);
        }

        private void ReportAxis(float v, float h)
        {
            thirdPersonAnim.SetFloat ("Vertical", v, 0, Time.deltaTime);
			thirdPersonAnim.SetFloat ("Horizontal", h, 0, Time.deltaTime);
        }
    }
}
