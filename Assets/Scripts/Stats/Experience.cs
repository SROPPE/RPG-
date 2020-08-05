using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace RPG.Stats
{
  
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float currentExp;
        public event Action AddingExp;
        public float GetExp() => currentExp;
      
        public void AddExp(float expCost)
        {
            currentExp += expCost;
            AddingExp?.Invoke();
        }

        public object CaptureState()
        {
            return currentExp;
        }

        public void RestoreState(object state)
        {
            currentExp = (float)state;
        }
    }
}