using RPG.Control;
using RPG.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        public Scheduler scheduler;
        public GameObject player;
        public PlayerController controller;
        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
            scheduler = player.GetComponent<Scheduler>();
            controller = player.GetComponent<PlayerController>();
        }
        void DisableControl(PlayableDirector pd)
        {
            scheduler.CancelCurrentAction();
            controller.enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            controller.enabled = true;

        }
    }
}