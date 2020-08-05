using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RGP.Utilities
{
    public class DestroyAferEffect : MonoBehaviour
    {
        [SerializeField] GameObject objectToDestroy;
        ParticleSystem currentParticle;
        private void Awake()
        {
            currentParticle = GetComponent<ParticleSystem>();
        }
        void Update()
        {
            if (!currentParticle.IsAlive())
            { if (objectToDestroy == null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(objectToDestroy);
                }
            }
        }
    }
}