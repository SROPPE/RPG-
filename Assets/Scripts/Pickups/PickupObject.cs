using RPG.Control;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class PickupObject : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon;
        [SerializeField] float takingDistance = 5f;
        [SerializeField] float healAmount = 0f;
        [SerializeField] float pickupRespawnTime = 2f;

        SphereCollider pickupCollider;
        private void Awake()
        {
            pickupCollider = GetComponent<SphereCollider>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag =="Player")
            {
                KindOfPickup(other.gameObject);
            }
        }

        private void KindOfPickup(GameObject other)
        {
            if (weapon)
            {
                GiveWeapon(other);
            }
            if (healAmount > 0)
            {
                Heal(other);
            }
            StartCoroutine(HideForSeconds(pickupRespawnTime));
        }

        private void Heal(GameObject gameObject)
        {
            gameObject.GetComponent<HealthSystem>().Heal(healAmount);
        }

        public bool HandleRaycast(GameObject callingController)
        {
            if (Vector3.Distance(callingController.transform.position, gameObject.transform.position) < takingDistance)
            {
                if (Input.GetMouseButton(0))
                {
                    KindOfPickup(callingController);
                }
                return true;
            }
            return false;
        }
        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
        private void GiveWeapon(GameObject other)
        { 
            other.GetComponent<Fighter>().EquipWeapon(weapon);
        }

        IEnumerator HideForSeconds(float seconds)
        {
            IsActive(false);
            yield return new WaitForSeconds(seconds);
            IsActive(true);
        }
        private void IsActive(bool state)
        {
            pickupCollider.enabled = state;
            
            var childrens = GetComponentsInChildren<MeshRenderer>();
            foreach (var children in childrens)
            {
                children.enabled = state;
            }
        }
    }
}