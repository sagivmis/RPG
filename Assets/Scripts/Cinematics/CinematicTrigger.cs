using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        public bool triggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !triggered)
            {
                triggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }

}