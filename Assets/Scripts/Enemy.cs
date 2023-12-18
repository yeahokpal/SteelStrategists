/*
 * Programmers: Jack Gill
 * Purpose: Manage enemy behavior
 * Inputs: If enemy is facing a structure or not
 * Outputs: Either attack or keep walking
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameManager gm;
    [SerializeField] new AudioSource audio;
    // Raycast for detecting a wall in front of it
    public float Health = 5;

    [SerializeField] float damage = 1;
    void Start()
    {
        rb.velocity = new Vector2(-2f, 0f);
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Increasing damage and health for every enemy spawned
    public void SetDamage(float modifier)
    {
        // Increasing Damage
        damage = (damage + modifier) * damage;

        // Increasing Health
        Health += (modifier * 5);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
            gm.score += 100;
            gm.UpdateScoreTxt();
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
