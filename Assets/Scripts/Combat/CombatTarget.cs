using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
    [RequireComponent(typeof(HealthSystem))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(GameObject gameObject)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                gameObject.GetComponent<Fighter>().StartAttackAction(this);

            }
                return true;
        }
    }
}