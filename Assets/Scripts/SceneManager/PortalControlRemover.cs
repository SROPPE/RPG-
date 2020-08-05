using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
namespace RPG.SceneManagment
{

    public class PortalControlRemover : MonoBehaviour
    {
        GameObject player;
        Portal currentPortal;
        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            currentPortal = GetComponent<Portal>();
        }
        private void OnEnable()
        {
            currentPortal.onLoadStart += DisableController;
            currentPortal.onLoadEnd += EnableController;
        }
        private void OnDisable()
        {
            currentPortal.onLoadStart -= DisableController;
            currentPortal.onLoadEnd -= EnableController;     
        }
        void EnableController()
        {
            if (player)
            {
                player.GetComponent<ActionScheduler>().CancelCurrentAction();
                player.GetComponent<PlayerController>().enabled = true;
            }
        }
        void DisableController()
        {
            if(player)
            player.GetComponent<PlayerController>().enabled = false;
        }
    }
}
