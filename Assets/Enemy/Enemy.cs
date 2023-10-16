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
    // Raycast for detecting a wall in front of it
    RaycastHit2D ray;
    bool attacking = false;

    void Start()
    {
        ray = Physics2D.Raycast(new Vector3(transform.position.x - .5f, transform.position.y, transform.position.z), -Vector3.right, .25f);
        rb.velocity = new Vector2(-2f, 0f);
    }

    void Update()
    {

        // Checking the Raycast to try to detect a wall
        Debug.DrawRay(new Vector3(transform.position.x - .5f, transform.position.y, transform.position.z), -Vector3.right, Color.red, .25f, false);

        // If it finds a wall, start attacking
        if (ray.collider.tag == "Building" && !attacking)
        {
            rb.velocity = new Vector2(0f, 0f);
            StartCoroutine(AttackBuildings());
        }
    }

    IEnumerator AttackBuildings()
    {
        attacking = true;
        // Add attacking code once buildings are made

        yield return new WaitForSeconds(1f);
        attacking = false;
    }
}
