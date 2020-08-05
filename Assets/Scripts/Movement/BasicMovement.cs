using RPG.Core;
using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace RPG.Movement
{
    public class BasicMovement : MonoBehaviour,IAction,ISaveable
    {
        NavMeshAgent navMeshAgent;
        [SerializeField] float maxSpeed;
        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
 
        void Update()
        {
            UpdateAnimator();
        }
        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
        public void StartMoveAction(Vector3 destanation, float speed = Mathf.Infinity)
        {
            
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destanation,speed:speed);
        }
        private void SetSpeed(float speed)
        {
            if (speed >= maxSpeed || speed <= 0) navMeshAgent.speed = maxSpeed;
            else navMeshAgent.speed = speed;
        }
        public void MoveTo(Vector3 destination, float attackRange = 0f, float speed = Mathf.Infinity)
        {
            SetSpeed(speed);
            navMeshAgent.stoppingDistance = attackRange;
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            data["position"] = new SerializableVector3(transform.position);
            return data;
        }

        public void RestoreState(object state)
        {
            if (navMeshAgent)
            {
                var data = (Dictionary<string,object>)state;
                navMeshAgent.enabled = false;
                transform.position = (data["position"] as SerializableVector3).ToVector();
                transform.eulerAngles = (data["rotation"] as SerializableVector3).ToVector();
                navMeshAgent.enabled = true;
            }
        }
    }
}