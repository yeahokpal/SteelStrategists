using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    float spawnDelay = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartSpawning()
    {
        Debug.Log("Spawn an enemy");
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(enemy, gameObject.transform);
        StartCoroutine(StartSpawning());
    }

}
