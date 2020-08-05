using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;

        [SerializeField] float xDistance;
        [SerializeField] float yDistance;
        [SerializeField] float zDistance;


        void LateUpdate()
        {
            gameObject.transform.position = new Vector3
                (target.position.x + xDistance,
                target.position.y + yDistance,
                target.position.z + zDistance);
        }
    }
}