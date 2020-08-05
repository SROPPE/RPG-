using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
namespace RPG.SceneManagment
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int numberOfLoadScene;
        [SerializeField] Transform spawnPoint;
        [SerializeField] PortalIndex portalIndex;
        [SerializeField] float loadScreenWait;
        [SerializeField] float timeIn;
        [SerializeField] float timeOut;
        SavingWrapper savingWrapper;
        LoadScreen loadScreen;
        public event Action onLoadStart;
        public event Action onLoadEnd;
        private void Start()
        {
            loadScreen = FindObjectOfType<LoadScreen>();
            savingWrapper = FindObjectOfType<SavingWrapper>();
        }
        public enum PortalIndex
        {
            A,B,C,D,E
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            onLoadStart?.Invoke();
            yield return loadScreen.LoadScreenIn(timeIn);
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(numberOfLoadScene);
            onLoadStart?.Invoke();

            savingWrapper.Load();
            PlayerPositionUpdate(GetSpawnPoint());
            savingWrapper.Save();

            yield return new WaitForSeconds(loadScreenWait);
            onLoadEnd?.Invoke();
            yield return loadScreen.LoadScreenOut(timeOut);
            Destroy(gameObject);
        }

        private Transform GetSpawnPoint()
        {
            var portals = GameObject.FindObjectsOfType<Portal>();
            foreach (var portal in portals)
            {
                if (portal.portalIndex == portalIndex && !ReferenceEquals(this,portal))
                {
                    return portal.spawnPoint;
                }
            }
            return null;
        }
        
        private void PlayerPositionUpdate(Transform spawnPosition)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(spawnPosition.position);
            player.transform.position = spawnPosition.position;
         
        
        }
    }
}
