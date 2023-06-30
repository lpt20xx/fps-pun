using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject spawnPointPrefab;

    private void Awake()
    {
        spawnPointPrefab.SetActive(false);
    }
}
