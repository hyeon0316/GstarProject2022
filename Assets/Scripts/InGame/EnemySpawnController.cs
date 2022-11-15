using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawns;


    private void ActiveSpawnArea()
    {
        foreach (var spawn in _spawns)
        {
            spawn.SetActive(true);
        }
    }
    
    private void InActiveSpawnArea()
    {
        foreach (var spawn in _spawns)
        {
            spawn.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("들어옴");
            ActiveSpawnArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("나감");
            InActiveSpawnArea();
        }
    }
}
