using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointGizmoRadius = 0.2f;
        private void OnDrawGizmos()
        {          
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypointPosition(i),waypointGizmoRadius);
                int j = GetNextIndex(i);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(j));
            }
        }
      
        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount) return 0;
            return i+1;
        }
        public Vector3 GetWaypointPosition(int index)
        {
            return transform.GetChild(index).position;
        }
    }
}
