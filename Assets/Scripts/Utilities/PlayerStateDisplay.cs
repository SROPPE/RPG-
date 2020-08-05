using RPG.Combat;
using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateDisplay : MonoBehaviour
{
    [SerializeField] Text playerHealthHolder;
    [SerializeField] Text enemyHealthHolder;
    [SerializeField] Text playerExpHolder;
    [SerializeField] Text playerLevelHolder;

    HealthSystem playerHealth;
    Fighter playerFighter;
    Experience playerExp;
    BaseStats stats;
    private void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();
        playerFighter = player.GetComponent<Fighter>();
        playerExp = player.GetComponent<Experience>();
        stats = player.GetComponent<BaseStats>();
    }
    private void Update()
    {
        playerHealthHolder.text = String.Format("{0}%", playerHealth.GetNormilizedHP());
        if (playerFighter.GetTarget())
            enemyHealthHolder.text = String.Format("{0}%", playerFighter.GetTarget().GetNormilizedHP());
        else
            enemyHealthHolder.text = "N/A";
        playerExpHolder.text = $"{playerExp.GetExp()}";
        playerLevelHolder.text = $"{stats.GetLevel()}";
    }
}
