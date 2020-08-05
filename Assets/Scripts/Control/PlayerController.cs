using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.Experimental.UIElements;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        HealthSystem health;
        [SerializeField] float maxNavMeshProjectionDistance = 2f;
        [SerializeField] float maxDistanceOfPath = 40f;
        [System.Serializable]
        struct CursorMapping 
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        [SerializeField]CursorMapping[] cursorMapping;

        private void Awake()
        {
            health = GetComponent<HealthSystem>();
        }
        private void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponents()) return;
            if (InteractWithMovement()) return;
                SetCursor(CursorType.None);
            
        }

        private bool InteractWithComponents()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (var hit in hits)
            {
                var interactableObjects = hit.transform.GetComponents<IRaycastable>();
                foreach (var interactableObject in interactableObjects)
                {
                    if (interactableObject.HandleRaycast(gameObject))
                    {
                        SetCursor(interactableObject.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }


        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] raycastDistances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                raycastDistances[i] = Vector3.Distance(hits[i].point, Input.mousePosition);
            }
            Array.Sort(raycastDistances, hits);
            return hits;
        }

        private bool InteractWithUI()
        {
            bool overUI = EventSystem.current.IsPointerOverGameObject();
            if(overUI)
            {
                SetCursor(CursorType.UIInteract);
                return overUI;
            }
            return overUI;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            if (RaycastNavMesh(out target))
            {
                SetCursor(CursorType.Movement);
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    GetComponent<BasicMovement>().StartMoveAction(target); 
                }
                return true;
            }
            return false;
        }
        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition
                (hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            NavMeshPath navMeshPath = new NavMeshPath();
            bool hasCalculated = NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, navMeshPath);

            if (!hasCalculated ||
                navMeshPath.status != NavMeshPathStatus.PathComplete ||
                !IsRealToFollowPath(navMeshPath)) return false;

            if (navMeshPath.status != NavMeshPathStatus.PathComplete) return false;
            
            target = navMeshHit.position;
            return true;
        }
        private bool IsRealToFollowPath(NavMeshPath navMeshPath)
        {
            Vector3[] distances = navMeshPath.corners;
            float result = 0f;
            if (navMeshPath.corners.Length < 2) return false;
            for (int i = 0; i < distances.Length - 1; i++)
            {
                result += Vector3.Distance(distances[i + 1], distances[i]);
            }
            return result < maxDistanceOfPath;
        }
        private void SetCursor(CursorType type)
        {
            var cursorMap = GetCursorMapping(type);
            Cursor.SetCursor(cursorMap.texture,cursorMap.hotspot,CursorMode.Auto);
        }
        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (var cursorMap in cursorMapping)
            {
                if(cursorMap.type == type)
                {
                    return cursorMap;
                }
            }
            return cursorMapping[0];
        }
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

