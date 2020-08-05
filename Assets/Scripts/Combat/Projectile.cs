using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        HealthSystem target;
        [SerializeField] float speed = 1f;
        [SerializeField] GameObject hitEffect;
        [SerializeField] bool isHoming = false;
        [SerializeField] List<GameObject> destroyList;
        [SerializeField] float missTargetDestroyingTime = 1f;
        [SerializeField] float destroyAfterImpact = 2f;
        [SerializeField] UnityEvent onHit;
        [SerializeField] UnityEvent onProjectileLaunch;
        float damage;

        private void Start()
        {
            transform.LookAt(target.transform);
            onProjectileLaunch?.Invoke();
        }
        void Update()
        {
            if (target == null) Destroy(gameObject, missTargetDestroyingTime);
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(target.transform);
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        public void SetTarget(HealthSystem target,float damage)
        {  
            this.target = target;
            this.damage = damage;
        }
        private void OnTriggerEnter(Collider other)
        {

            if (other.GetComponent<HealthSystem>() != target) return;
            if (target.IsDead()) return;

            Instantiate(hitEffect, target.transform.position, transform.rotation);
            onHit?.Invoke();
            target.TakeDamage(damage);
            foreach (var item in destroyList)
            {
                Destroy(item);
            }
            Destroy(gameObject,destroyAfterImpact);
        }
    }
}