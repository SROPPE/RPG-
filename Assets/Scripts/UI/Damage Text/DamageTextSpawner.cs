using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageText;

        public void Spawn(float damageAmount)
        {
            var damageTextInstance = Instantiate<DamageText>(damageText,gameObject.transform);
            damageTextInstance.SetValue(damageAmount);
        }
    }
}