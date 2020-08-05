using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 1)]
    public class Progression : ScriptableObject
    {
        [SerializeField] List<ProgressionCharacterClass> charactersProgression;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;
        public float GetStat(CharacterClass character,Stat stat,int level)
        {
            BuildLookup();
            float[] levels =  lookupTable[character][stat];
            if(levels.Length<= level-1)
            {
                return 1;
            }
            return levels[level - 1];
        }

        public float GetMaxLevel(Stat stat,CharacterClass characterClass)
        {
            var levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (var characterClass in charactersProgression)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();
                foreach (var progression in characterClass.progressionStat)
                {
                    statLookupTable[progression.stat] = progression.levels;

                }
                lookupTable[characterClass.character] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass character;
            public ProgressionStat[] progressionStat;
        }
        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }


    }
}