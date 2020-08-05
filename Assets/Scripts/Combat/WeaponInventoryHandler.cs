using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponInventoryHandler : MonoBehaviour
    {
        List<WeaponConfig> weaponsInventory = new List<WeaponConfig>();
        [SerializeField] int currentWeaponIndex = 0;
        public void TakeNewWeaponInInventory(WeaponConfig newWapon)
        {
            weaponsInventory.Add(newWapon);
        }
        public WeaponConfig ChooseWeapon(int index)
        {
            if (index < weaponsInventory.Count && index >= 0)
                currentWeaponIndex = index;
            else
                return weaponsInventory[currentWeaponIndex];

            return weaponsInventory[index];
        }
    }
}