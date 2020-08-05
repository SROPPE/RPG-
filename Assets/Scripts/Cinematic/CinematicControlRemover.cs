using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;
        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }
        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableController;
            GetComponent<PlayableDirector>().stopped += EnableController;
        }
        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableController;
            GetComponent<PlayableDirector>().stopped -= EnableController;
        }
        void EnableController(PlayableDirector director)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = true;
        }
        void DisableController(PlayableDirector director)
        {
            player.GetComponent<PlayerController>().enabled = false;
        }

    }
}