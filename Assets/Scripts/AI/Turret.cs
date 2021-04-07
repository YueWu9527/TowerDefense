using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A2
{
    public class Turret : MonoBehaviour
    {
        public Character character;

        public InitiativeEquipment initiativeEquipment;
        public GameObject initiativeEquipmentRoot;
        public TeamPlayer teamPlayer;
        private TeamPlayer targetTeamPlayer;
        public int targetTeamIndex;
        public GameObject eyes;
        private Vector3 initPos;
        private bool reset = false;
        private bool readyToAttack = false;
        public float searchRange = 10;
        public float attackRange = 7;
        public float stopRange = 5;
        public float speed = 3;

        public float aimSpeed = 0.75f;
        private float _aimSpeed
        {
            get
            {
                return aimSpeed / 50f;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            initiativeEquipment.FunctionBtnInput(character,BtnType.Sub1,BtnInputType.Down);
            initPos = character.transform.position;
            StartCoroutine(waitFindTarget(0.2f));
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(targetTeamPlayer == null) 
            {
                readyToAttack = false;
                initiativeEquipment.FunctionBtnInput(character, BtnType.Main1, BtnInputType.Up);
                return;
            }
            Vector3 targetPos = targetTeamPlayer.gameObject.transform.position;
            float mag = (targetPos-character.transform.position).magnitude;
            bool inAttackRange = mag < attackRange;
            bool inBestAttackRange = mag < stopRange;

            if (inAttackRange)
            {
                if(readyToAttack)
                {
                    Vector3 dir = targetPos - character.transform.position;

                    RaycastHit hit;
                    if (Physics.Raycast(eyes.transform.position, dir, out hit))
                    {
                        if (hit.collider.gameObject == targetTeamPlayer.gameObject)
                        {
                            if (initiativeEquipment != null)
                            {
                                Vector3 currentDir = Vector3.Lerp(initiativeEquipmentRoot.transform.forward, dir, _aimSpeed);
                                initiativeEquipmentRoot.transform.rotation = Quaternion.LookRotation(currentDir);
                                //瞄准玩家攻击
                                initiativeEquipment.FunctionBtnInput(character, BtnType.Main1, BtnInputType.Down);

                                currentDir = Vector3.Lerp(transform.forward, dir, _aimSpeed);
                                currentDir.y = 0;
                                character.transform.rotation = Quaternion.LookRotation(currentDir);
                            }
                        }
                        else
                        {
                            initiativeEquipment.FunctionBtnInput(character, BtnType.Main1, BtnInputType.Up);
                        }
                    }
                    else
                    {
                        initiativeEquipment.FunctionBtnInput(character, BtnType.Main1, BtnInputType.Up);
                    }
                }
                else
                {
                    if(inBestAttackRange)
                    {
                        readyToAttack = true;
                    }
                    else
                    {
                        readyToAttack = false;
                        initiativeEquipment.FunctionBtnInput(character, BtnType.Main1, BtnInputType.Up);
                    }
                }
            }
            else
            {
                readyToAttack = false;
                initiativeEquipment.FunctionBtnInput(character, BtnType.Main1, BtnInputType.Up);
            }
        }

        IEnumerator waitOneSecond(){
            yield return new WaitForSeconds(1.0f);
        }

        IEnumerator waitFindTarget(float interval){
            yield return new WaitForSeconds(interval);
            
            targetTeamPlayer = CharacterMap.Instance.GetTargetByTeamIndex(teamPlayer,searchRange,targetTeamIndex);
            yield return waitFindTarget(interval);
        }
    }
}