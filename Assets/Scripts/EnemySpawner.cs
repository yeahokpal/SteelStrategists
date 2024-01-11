/*
 * Programmers: Jack Gill
 * Purpose: Spawning Enemies based on the timer's status
 * Inputs: Timer from GameManager.cs hits 0:00
 * Outputs: Start spawning enemies
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    #region Global Variables
    [SerializeField] GameObject enemyPrefab;
    private float modifier = 1f;

    // Interval between spawning enemies
    private float spawnDelay = 3f;
    #endregion

    #region Default Methods
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            StartCoroutine(StartSpawning());
            modifier = 0;
        }
    }
    #endregion

    #region Custom Methods
    // Recursive IEnumerator because it has to wait for spawnDelay between spawns
    public IEnumerator StartSpawning()
    {
        Debug.Log("Spawn an enemy");
        yield return new WaitForSeconds(spawnDelay);
        Enemy enemy = Instantiate(enemyPrefab, gameObject.transform).GetComponent<Enemy>();
        enemy.SetDamage(modifier);
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            enemy.goUp = true;
        }
        modifier *= 1.05f;
        StartCoroutine(StartSpawning());
    }
    #endregion
}
