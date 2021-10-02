using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject persistentObjectPrefab;

    static bool hasSpawnedPersistentObjects = false;
    private void Awake()
    {
        if (hasSpawnedPersistentObjects) return;

        SpawnPersistentObjects();

        hasSpawnedPersistentObjects = true;
    }

    private void SpawnPersistentObjects()
    {
        GameObject persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}
