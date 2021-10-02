using RPG.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{

    enum DestinationIdentifier
    {
        A, B, C, D, E, F
    }

    [SerializeField] int sceneToLoad = -1;
    [SerializeField] float fadeOutTime = 0.5f;
    [SerializeField] float fadedTime = 0.5f;
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationIdentifier destination;

    GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            StartCoroutine(nameof(Transition));
    }

    private IEnumerator Transition()
    {
        if(sceneToLoad < 0)
        {
            Debug.LogError("Scene to load not set.");
            yield break;
        }

        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeOutTime);

        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        // LOAD EVERYTHING HERE

        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);

        yield return new WaitForSeconds(fadedTime);


        yield return fader.FadeIn(fadeInTime);


        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        if (otherPortal==null) return;
        player = GameObject.FindWithTag("Player");

        player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        player.transform.rotation = otherPortal.spawnPoint.rotation;

    }

    private Portal GetOtherPortal()
    {
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if (portal.destination == this.destination)
                return portal;

        }
        return null;
    }
}
