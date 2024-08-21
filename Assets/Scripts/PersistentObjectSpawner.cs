using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
    private static bool isSpawned = false;

    [SerializeField] private GameObject persistantObjectPrefab;

    private void Awake()
    {
        if (!isSpawned)
        {
            isSpawned = true;
            GameObject persistantObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}
