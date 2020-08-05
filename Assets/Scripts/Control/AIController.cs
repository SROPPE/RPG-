using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {   [Header("Fight stats")]
        [SerializeField] float chaseRange = 10f;
        [SerializeField] float attackRange = 4f;
        [Header("Patrol settings")]
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellingTime = 2f;
        [SerializeField] float suspiciousTime = 4f;
        [SerializeField] PatrolPath patrolPath;
        [Header("Speed")]
        [SerializeField] float patrolSpeed = 3f;
        [SerializeField] float chaseSpeed = 5f;
        int currentWaypointIndex;
        
        GameObject player;
        Fighter fighter;
        HealthSystem health;
        BasicMovement movement;
      
        Vector3 currentWaypoint;

        float timeScinceLastSawPlayer = Mathf.Infinity;
        float timeScinceGetWaypoint = 0f;
        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<HealthSystem>();
            movement = GetComponent<BasicMovement>();
        }
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            currentWaypoint = transform.position;
        }
        private void Update()
        {
            if (!health.IsDead())
            {
                BehaviorState();
            }
        }
        private void BehaviorState()
        {
            if(EnoughDistance())
            {
                ChasePlayer();
             
            }
            else if(timeScinceLastSawPlayer < suspiciousTime)
            {
                Suspicious();
            }
            else
            {
                Patrol();
            }
        }
        private bool EnoughDistance()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseRange;
            
        }
        private void ChasePlayer()
        {
          
            timeScinceLastSawPlayer = 0f;
            fighter.StartAttackAction(player.GetComponent<CombatTarget>());
        }
        private void Suspicious()
        {
            fighter.Cancel();
            timeScinceLastSawPlayer += Time.deltaTime;
        }


        private void Patrol()
        {
          
            fighter.Cancel();

            if(patrolPath)
            {
                if(AtWaypoint())
                {
                    CycleWaypoint();
                }
                currentWaypoint = GetCurrentWaypoint();
            }
            movement.StartMoveAction(currentWaypoint,patrolSpeed);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypointPosition(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            timeScinceGetWaypoint = 0f;
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);     
        }

        private bool DwellingAtWaypoint()
        {
            timeScinceGetWaypoint += Time.deltaTime;
            return timeScinceGetWaypoint > waypointDwellingTime;      
        }

        private bool AtWaypoint()
        {
            bool atWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
            if (atWaypoint && DwellingAtWaypoint())
            {
                return atWaypoint;
            }
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}