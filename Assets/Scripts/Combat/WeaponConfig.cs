using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon",menuName = "Weapon/Create New Weapon",order = 0)]    
    public class WeaponConfig:ScriptableObject
    {
        const string weaponName = "Weapon";

        [SerializeField] float _meleeAttackRange;
        [SerializeField] float _timeBetweenAttack = 1f;
        [SerializeField] float _damage = 5f;
        [SerializeField] float _weaponCondition = 0;
        [SerializeField] Weapon _weaponPrefab;
        [SerializeField] AnimatorOverrideController _animatorOverride;
        [SerializeField] bool _isRigthHanded = true;
        [SerializeField] Projectile _projectile = null;
        public float GetMeleeAttackRange() => _meleeAttackRange;
        public float GetTimeBetweenAttack() => _timeBetweenAttack;
        public float GetDamageValue() => _damage;
        public float GetWeaponCondition() => _weaponCondition;
        public Projectile GetProjectile() => _projectile;
        
        public Weapon SpawnWeapon(Transform rightHand,Transform leftHand, Animator animatorToOverride)
        {
            DestroyOldWeapon(rightHand, leftHand);
            var currentWeapon = Instantiate(_weaponPrefab, GetCurrentHandTransform(rightHand, leftHand));
            currentWeapon.name = weaponName;
            ResetAnimator(animatorToOverride);
            return currentWeapon;
        }

        private void ResetAnimator(Animator animatorToOverride)
        {
            if (_animatorOverride)
                animatorToOverride.runtimeAnimatorController = _animatorOverride;
            else
            {
                var overrideController = animatorToOverride.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController)
                    animatorToOverride.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private static void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon != null)
            {
                oldWeapon.name = "Destoying";
                Destroy(oldWeapon.gameObject);
            }
        }

        private Transform GetCurrentHandTransform(Transform rightHand, Transform leftHand)
        {
            if (_isRigthHanded)
            {
                return rightHand;
            }
            return leftHand;       
        }

        public bool HasProjectile()
        {
            return _projectile != null; 
        }
        public void LaunchProjectile(HealthSystem target,Transform lHand,Transform rHand, float resultDamage)
        {
            var projectile = Instantiate(_projectile.gameObject,
                GetCurrentHandTransform(lHand,rHand).position,
                Quaternion.identity);
            projectile.GetComponent<Projectile>().SetTarget(target, resultDamage);
        }
    }
}
