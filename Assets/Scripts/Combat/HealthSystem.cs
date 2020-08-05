using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class HealthSystem : MonoBehaviour, ISaveable
    {
        [SerializeField] LazyValue<float> healthPoints;
        public TakeDamageEvent takeDamage;
        public UnityEvent onDie;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }
        BaseStats stats;
        LazyValue<float> maxHP;
        bool isDead = false;
        private void Awake()
        {
            stats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(SetHealth);
            maxHP = new LazyValue<float>(SetHealth);
        }
        private float SetHealth()
        {
            return stats.GetStat(Stat.Health);
        }
        private void Start()
        {
            healthPoints.ForceInit();
            maxHP.ForceInit();
            if (healthPoints.value <= 0) return;
        }
        private void OnEnable()
        {
            stats.onLevelUp += RegenerateHealth;
        }
        private void OnDisable()
        {
            stats.onLevelUp -= RegenerateHealth;
        }
        private void RegenerateHealth()
        {
            if (stats)
            {
                maxHP.value = healthPoints.value = stats.GetStat(Stat.Health);
            }
        }
        public int GetNormilizedHP()
        {
            return (int)Mathf.Round((healthPoints.value / maxHP.value) * 100);
        }
        public void TakeDamage(float damageValue)
        {
            if (!isDead)
            {
                healthPoints.value = Mathf.Max(healthPoints.value - damageValue, 0);
                takeDamage.Invoke(damageValue);
                if (healthPoints.value == 0)
                {
                    onDie?.Invoke();
                    ExperienceReward();
                    Death();
                }
            }
        }
        public void Heal(float amount)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + amount, maxHP.value);
        }
        private void Death()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<Animator>().SetTrigger("Death");
        }
        private void ExperienceReward()
        {
            if (stats)
                GameObject.FindWithTag("Player").GetComponent<Experience>().AddExp(stats.GetStat(Stat.ExperienceReward));
        }

        public bool IsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value <= 0)
            {
                Death();
            }
        }
    }
}