/*
 * Programmers: Jack Gill
 * Purpose: Manage enemy behavior
 * Inputs: If enemy is facing a structure or not
 * Outputs: Either attack or keep walking
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class Enemy : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameManager gm;
    [SerializeField] new AudioSource audio;
    public bool goUp;
    float timerStart;
    // Raycast for detecting a wall in front of it
    public float Health = 5;
    float speed = 2f;
    [SerializeField] float damage = 1;
    void Start()
    {
        timerStart = Time.realtimeSinceStartup;

        // Setting velocity if in tutorial of main game
        if (!goUp)
        {
            rb.velocity = new Vector2(-1 * speed, 0f);
        }
        else
        {
            rb.velocity = new Vector2(0f, speed);
        }

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup - timerStart >= 100000)
        {
            Destroy(gameObject);
        }
    }

    // Increasing damage and health for every enemy spawned
    public void SetDamage(float modifier)
    {
        // Increasing Damage
        damage = (damage + modifier) * damage;

        // Increasing Speed
        speed += modifier;

        // Increasing Health
        Health += (modifier * 5);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        // Death
        if (Health <= 0)
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                SceneManager.LoadScene("StartingMenu");
            }

            gm.score += 100;
            gm.UpdateScoreTxt();
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door" && collision.gameObject.GetComponent<Door>().Health > 0)
        {
            rb.velocity = new Vector2(0f, 0f);
            StartCoroutine(StartAttackingBase(collision));
        } 
    }

    // Code that executes when the enemy is attacking the base
    IEnumerator StartAttackingBase(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door" && collision.gameObject.GetComponent<Door>().Health > 0)
        {
            collision.gameObject.GetComponent<Door>().TakeDamage(damage);
            audio.Play();
            yield return new WaitForSeconds(1f);
            StartCoroutine(StartAttackingBase(collision));
        }
    }
}
