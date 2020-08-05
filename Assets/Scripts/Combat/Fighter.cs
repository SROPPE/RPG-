using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {

        HealthSystem target;
        [SerializeField] Transform rightHandTransfom;
        [SerializeField] Transform leftHandTransform;
        [SerializeField] WeaponConfig defaultWeaponConfig;
        [SerializeField] string defaultWeaponName = "Sword";
     
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        float timeFromLastAttack = Mathf.Infinity;
        bool isTargetInAttackRange;
        public UnityEvent onHit;
        public HealthSystem GetTarget() => target;
        private void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }
        private void Start()
        {
            currentWeaponConfig = defaultWeaponConfig;
            currentWeapon.ForceInit();
        }
        private void Update()
        {
            timeFromLastAttack += Time.deltaTime;        
            if (target)
            {
                if (target.IsDead()) { Cancel(); return; }
                AttackStateHandler();
            }

        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }
        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeaponConfig);
        }
        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            var currentWeapon = weapon.SpawnWeapon(rightHandTransfom, leftHandTransform, GetComponent<Animator>());
            onHit.AddListener(currentWeapon.OnHit);
            return currentWeapon;
        }

        private void AttackStateHandler()
        {
            isTargetInAttackRange = Vector3.Distance(
                transform.position,
                target.transform.position) < currentWeaponConfig.GetMeleeAttackRange();

            if (isTargetInAttackRange)
                RichTarget();
            else
                MoveToTarget();
        }

        private void RichTarget() 
        {
            transform.LookAt(target.transform);
            if (timeFromLastAttack >= currentWeaponConfig.GetTimeBetweenAttack())
            {
                GetComponent<Animator>().ResetTrigger("stopAttack");
                GetComponent<Animator>().SetTrigger("Attack");
                
                timeFromLastAttack = 0f;
            }
            GetComponent<BasicMovement>().Cancel();
        }

        public void StartAttackAction(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<HealthSystem>();      
        }
        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<BasicMovement>().Cancel();
            target = null;   
        }
        private void MoveToTarget()
        {
            GetComponent<BasicMovement>().MoveTo(target.transform.position, currentWeaponConfig.GetMeleeAttackRange());
        }
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.BonusDamage)
            {
                yield return currentWeaponConfig.GetDamageValue();
            }
        }
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.BonusDamage)
            {
                yield return currentWeaponConfig.GetWeaponCondition();
            }
        }
 
        #region Animation
        public void Hit()
        {
            if (target == null) return;
            var damage = GetComponent<BaseStats>().GetStat(Stat.BonusDamage);
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(target, leftHandTransform, rightHandTransfom, damage);
            }
            else if (target)
            {
                onHit?.Invoke();
                target.TakeDamage(damage);
            }   
        }

        public void Shoot()
        {       
            Hit();
        }
        #endregion
        public object CaptureState()
        {
            if (currentWeaponConfig)
                return currentWeaponConfig.name;
            else return defaultWeaponName; 
        }

        public void RestoreState(object state)
        {
            if (tag == "Player")
                EquipWeapon(Resources.Load<WeaponConfig>((string)state));
            else
                EquipWeapon(Resources.Load<WeaponConfig>(defaultWeaponName));
        }

        
    }
}
