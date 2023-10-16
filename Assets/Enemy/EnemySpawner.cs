/*
 * Programmers: Jack Gill
 * Purpose: Spawning Enemies based on the timer's status
 * Inputs: Timer from GameManager.cs hits 0:00
 * Outputs: Start spawning enemies
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    // Interval between spawning enemies
    float spawnDelay = 3f;

    // Recursive IEnumerator because it has to wait for spawnDelay between spawns
    public IEnumerator StartSpawning()
    {
        Debug.Log("Spawn an enemy");
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(enemy, gameObject.transform);
        StartCoroutine(StartSpawning());
    }
}
