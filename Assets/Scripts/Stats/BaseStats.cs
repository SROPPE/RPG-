using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Analytics;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1, 25)] int level = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;
        [SerializeField] GameObject levelUpVFX;
        [SerializeField] bool canUseModifiers =false;
        Experience experience;
        public event Action onLevelUp;
        private void Awake()
        {
            experience = GetComponent<Experience>();
        }
        private void OnEnable()
        {
            if (experience)
            {
                experience.AddingExp += LevelProcessing;
            }
        }
        private void OnDisable()
        {
            if (experience)
            {
                experience.AddingExp -= LevelProcessing;
            }
        }
        public int GetLevel() => level;
        public float GetStat(Stat stat)
        {
            if(canUseModifiers)
                return (progression.GetStat(characterClass, stat, level) + GetResultAdditiveModifier(stat)) * (1 + GetResultPercentageModifier(stat)/100);

            return progression.GetStat(characterClass, stat, level);
        }
        public void LevelProcessing()
        {
            if (experience == null) return;
            var maxLevel = progression.GetMaxLevel(Stat.ExperienceRequired, characterClass);
            if (maxLevel == level) return;
            float currentXP = experience.GetExp();
            while(level!=maxLevel)
            { 
                var requiredXp = GetStat(Stat.ExperienceRequired);
                if (currentXP - requiredXp >= 0)
                {
                    currentXP -= requiredXp;
                    LevelUp();
                }
                else break;
            }
        }

        private void LevelUp()
        {
            level++;
            if (levelUpVFX)
                Instantiate(levelUpVFX, transform);
            onLevelUp?.Invoke();
        }

        public float GetResultAdditiveModifier(Stat stat)
        {
            float result = 0f;
            foreach (var modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in modifierProvider.GetAdditiveModifiers(stat))
                {
                    result += modifier;  
                }
            }
            return result;
        }
        private float GetResultPercentageModifier(Stat stat)
        {
            float result = 0f;
            foreach (var modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in modifierProvider.GetPercentageModifiers(stat))
                {
                    result += modifier;
                }
            }
            return result;
        }
    }
}