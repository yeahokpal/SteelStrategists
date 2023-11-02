/*
 * Programmers: Jack Gill
 * Purpose: Control the animations on the door leading outside
 * Inputs: Player enters circle collider on door
 * Outputs: Change current door animation
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] Animator anim;

    [SerializeField] Slider slider;

    public float Health = 100;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        slider.value = Health / 100;
        if (Health <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("Close");
        }
    }
}
