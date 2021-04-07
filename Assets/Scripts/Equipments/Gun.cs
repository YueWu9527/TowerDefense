using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace A2
{
    public enum BulletType
    {
        HitScan = 1,
        Rigid = 2
    }

    public enum ReloadMode
    {
        FullCip = 1,
        Single = 2,
    }

    public enum AutoReloadMode
    {
        CheckBeforeFire = 1,
        CheckAfterFire = 2
    }

    public enum FireMode
    {
        Bolt = 1,
        Auto = 2
    }

    public enum GunType
    {
        Pistol = 1,
        SMG = 2,
        Rifle = 3,
        Shotgun = 4,
        Launcher = 5,
        Sniper = 6
    }

    public class Gun : InitiativeEquipment
    {
        public Action<bool> actionShoot;
        public Action<bool,bool> actionReload;
        public Action<bool> actionAim;

        public BulletType bulletType = BulletType.HitScan;
        public FireMode fireMode = FireMode.Bolt;

        public ReloadMode reloadMode = ReloadMode.FullCip;
 
        public Bullet bullet = new Bullet(5, 5, 0, 0, 0);

        public float fireInterval = 0.3f;
        public int magazineSize = 10;
        private int bulletInMagazine = 0;

        public float reloadTime = 1;
        public float reloadEmptyTime = 1;

        Coroutine autoFireCoroutine=null;
        Coroutine reloadCoroutine=null;
        Coroutine changeWeaponCoroutine=null;
        Coroutine aimCoroutine=null;

        
        public bool isInfAmmo = false;
        public bool isBreakReloadShoot = false;
        private float attackTime = 0;
        public float accuracyDefault = 0.97f;
        private float accuracy;
        public float recoilV = 0.2f;
        public float recoilH = 0.2f;
        public int multishot = 1;
        public float rocketFlightForce = 20f;
        public float rocketMaxFlightTime = 5f;
        public Transform bulletSpawnTransform;
        public float criticalChance = 0.3f;
        public float criticalDamage = 1.5f;
        public AutoReloadMode autoReloadMode = AutoReloadMode.CheckBeforeFire;
        public GunType gunType = GunType.Rifle;

        public float aimSpeed = 0.2f;
        public float aimMoveSpeedReduceRatio = 0.5f;

        private bool hasAimCommand = false;
        private bool isAiming = false;
        private bool isAim = false;
        private bool isReloading = false;

        private bool runLockAim = false;
        private bool runLockShoot = false;
        public float moveSpeedAffect = 1;

        override public void FunctionBtnInput(Character character, BtnType btnType, BtnInputType btnInputType)
        {
            if(btnType == BtnType.Main1)
            {
                if(btnInputType == BtnInputType.Down)
                {
                    PullTrigger(character);
                }

                if(btnInputType == BtnInputType.Up)
                {
                    ReleaseTrigger(character);
                }
            }

            if(btnType == BtnType.Main2)
            {
                if(btnInputType == BtnInputType.Down)
                {
                    BeginAim(character);
                }

                if(btnInputType == BtnInputType.Press)
                {
                    Aim(character);
                }

                if(btnInputType == BtnInputType.Up)
                {
                    StopAim(character);
                }
            }

            if(btnType == BtnType.Sub1)
            {
                if(btnInputType == BtnInputType.Down)
                {
                    Reload(character);
                }
            }
        }

        override public System.String GetShowOnUI()
        {
            if(isInfAmmo) return "Inf";
            else return bulletInMagazine.ToString();
        }

        override public bool GetRunnable()
        {
            return !runLockAim && !runLockShoot;
        }

        override public float GetSpeedAffect()
        {
            return moveSpeedAffect;
        }

        private void Aim(Character character)
        {
            if(!isReloading && hasAimCommand && !isAiming && !isAim) aimCoroutine = StartCoroutine(coroutineAim(character, aimSpeed));
        }

        private void BeginAim(Character character)
        {
            hasAimCommand = true;
        }

        private void StopAim(Character character)
        {
            hasAimCommand = false;
            isAiming = false;
            isAim = false;
            runLockAim = false;
            moveSpeedAffect = 1;
            if(aimCoroutine !=null)
            {
                if(actionAim!=null)
                {
                    actionAim(false);
                }
                StopCoroutine(aimCoroutine);
            }
        }

        IEnumerator coroutineAim(Character character, float seconds){
            isAiming = true;
            runLockAim = true;
            if(actionAim!=null && CheckShootAvaliable())
            {
                actionAim(true);
            }
            yield return new WaitForSeconds(seconds);
            isAiming = false;
            isAim = true;
            moveSpeedAffect = aimMoveSpeedReduceRatio;
        }

        private void PullTrigger(Character character)
        {
            if(autoFireCoroutine!=null)
            {
                StopCoroutine(autoFireCoroutine);
            }
            if(isBreakReloadShoot)
            {
                if(CheckBreakReloadShootAvaliable())
                {
                    if(bulletInMagazine>0)
                    {
                        BreakReload();
                    }
                    if(fireMode == FireMode.Bolt)
                    {
                        Shoot(character);
                    }
                    else if(fireMode == FireMode.Auto)
                    {
                        runLockShoot = true;
                        autoFireCoroutine = StartCoroutine(coroutineAutoShoot(fireInterval,character));
                    }
                }
            }
            else
            {
                if(CheckShootAvaliable())
                {
                    if(fireMode == FireMode.Bolt)
                    {
                        Shoot(character);
                    }
                    else if(fireMode == FireMode.Auto)
                    {
                        runLockShoot = true;
                        autoFireCoroutine = StartCoroutine(coroutineAutoShoot(fireInterval,character));
                    }
                }
            }
        }

        private void ReleaseTrigger(Character character)
        {
            if(fireMode == FireMode.Auto)
            {
                runLockShoot = false;
                if(autoFireCoroutine!=null)
                {
                    StopCoroutine(autoFireCoroutine);
                }
            }
        }

        private void BreakReload()
        {
            StopReload();
            if(reloadCoroutine!=null)
            {
                StopCoroutine(reloadCoroutine);
            }
        }

        private void Reload(Character character)
        {
            if(bulletInMagazine < magazineSize && !isReloading)
            {
                runLockAim = false;
                runLockShoot = false;
                moveSpeedAffect = 1;
                if(reloadMode == ReloadMode.Single)
                {
                    reloadCoroutine = StartCoroutine(coroutineSingleReloadBegin());
                }
                else
                {
                    reloadCoroutine = StartCoroutine(coroutineFullCipReloadBegin());
                }
            }
        }

        private void StopReload()
        {
            isReloading = false;
        }

        private int GetBulletInMagazine()
        {
            return bulletInMagazine;
        }

        private void Shoot(Character character)
        {
            if(!CheckShootInteval())
            {
                return;
            }

            if(bulletInMagazine <= 0)
            {
                Reload(character);
                return;
            }

            //开火时发生开火事件
            if(actionShoot!=null)
            {
                if(isAim) actionShoot(true);
                else actionShoot(false);
            }

            //减子弹数
            if(!isInfAmmo)
            {
                bulletInMagazine--;
            }

            float _roll = UnityEngine.Random.Range(0f,1f);
            Bullet _bullet = null;
            if(_roll<criticalChance)
            {
                _bullet = bullet.Critical(criticalDamage);
            }
            else
            {
                _bullet = bullet.Clone();
            }

            //根据发射模式执行动作
            if (bulletType == BulletType.HitScan)
            {
                for(int i=0;i<multishot;i++)
                {
                    HitScanShoot(_bullet,character);
                }
            }
            if (bulletType == BulletType.Rigid)
            {
                for(int i=0;i<multishot;i++)
                {
                    RigidShoot(_bullet,character);
                }
            }

            //后坐力
            if(isAim) character.Rotate(UnityEngine.Random.Range(-recoilH,recoilH), -recoilV);

            if(bulletInMagazine <= 0)
            {
                if(autoReloadMode == AutoReloadMode.CheckAfterFire)
                {
                    Reload(character);
                }
            }

        }

        private Vector3 GetShootDir()
        {
            float accuracy = accuracyDefault;
            if(isAim) accuracy = 1;
            return bulletSpawnTransform.forward+bulletSpawnTransform.up*UnityEngine.Random.Range(accuracy - 1,1 - accuracy)+bulletSpawnTransform.right*UnityEngine.Random.Range(accuracy - 1,1 - accuracy);
        }

        private void RigidShoot(Bullet Bullet, Character character)
        {
            Vector3 dir = GetShootDir();
            GameObject rigidBullet = RigidBulletPool.Instance.rocketPool.New();
            rigidBullet.SetActive(true);
            rigidBullet.transform.position = bulletSpawnTransform.position;
            rigidBullet.transform.rotation = Quaternion.LookRotation(dir);
            rigidBullet.GetComponent<RigidBullet>().SetHitSender(character.hitSender);
            rigidBullet.GetComponent<RigidBullet>().SetBullet(Bullet);
            rigidBullet.GetComponent<Rigidbody>().AddForce(dir * rocketFlightForce, ForceMode.Impulse);
        }

        private void HitScanShoot(Bullet Bullet, Character character)
        {
            int layerMask = 1 << 10;
            layerMask = ~layerMask;
            Vector3 dir = GetShootDir();
            RaycastHit hit;
            if(Physics.Raycast(bulletSpawnTransform.position,dir,out hit,100f,layerMask))
            {
                GameObject hitObj = hit.collider.gameObject; 
                HitReciever hitReciever = hitObj.GetComponent<HitReciever>();
                //简化为发射射线
                character.hitSender.DealDamage(hitReciever,hit.point, Bullet);
                HitFeedBack hitFeedBack = hitObj.GetComponent<HitFeedBack>();
                if(hitFeedBack!=null)
                {
                    hitFeedBack.Reflect(hit.point,Quaternion.LookRotation(Vector3.Lerp(hit.normal,-dir,0.7f)));
                }
            }
        }

        IEnumerator coroutineFullCipReloadBegin(){
            isReloading = true;
            if(bulletInMagazine == 0)
            {
                if(actionReload!=null)
                {
                    actionReload(false,true);
                }
                yield return new WaitForSeconds(reloadEmptyTime);
            }
            else
            {
                if(actionReload!=null)
                {
                    actionReload(false,false);
                }
                yield return new WaitForSeconds(reloadTime);
            }
            bulletInMagazine = magazineSize;
            StopReload();
        }

        IEnumerator coroutineSingleReloadBegin(){
            isReloading = true;
            if(actionReload!=null)
            {
                actionReload(true,true);
            }
            yield return new WaitForSeconds(0.3f);
            yield return coroutineSingleReload();
        }

        IEnumerator coroutineSingleReload(){
            yield return new WaitForSeconds(reloadTime);
            if(bulletInMagazine<magazineSize)
            {
                if(actionReload!=null)
                {
                    actionReload(true,false);
                }
                bulletInMagazine++;
                yield return coroutineSingleReload();
            }
            else
            {
                StopReload();
            }
        }

        IEnumerator coroutineAutoShoot(float seconds, Character character){
            Shoot(character);
            yield return new WaitForSeconds(seconds);
            yield return coroutineAutoShoot(seconds, character);
        }

        private bool CheckShootInteval()
        {
            if(Time.time-attackTime>fireInterval && CheckShootAvaliable())
            {
                attackTime=Time.time;
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private bool CheckShootAvaliable()
        {
            if(!isReloading)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckBreakReloadShootAvaliable()
        {
            return true;
        }
    }
}
