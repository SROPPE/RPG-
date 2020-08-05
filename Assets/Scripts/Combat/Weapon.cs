using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]AudioSource audioKick;

        public void OnHit()
        {
            if(!audioKick.isPlaying)
            {
                audioKick.Play();
            }
        }
    }
}