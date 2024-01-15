/*
 * Programmers: Jack Gill and Caden Mesina
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
    System.Random random = new System.Random();
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
        //calls ChangePosition() every 0.5 seconds
        InvokeRepeating("ChangePosition", 0, 0.5f);
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

    //moves the position of the enemy spawner every time it is called
    private void ChangePosition()
    {
        this.transform.position = new Vector3(36f, random.Next(-3, 4), 0f);
    }
    #endregion
}
