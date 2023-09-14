/*
 * Programmer: Caden Mesina
 * Purpose: Make objects this script is attached to execute an action when the player is within a certain distance and presses interact
 * Inputs:
 *  - When a player enters and exits the interaction area
 *  - The radius of the interaction area
 *  - The player interaction key
 * Outputs: Whatever function is needed by the object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactables : MonoBehaviour
{
    [SerializeField] private CircleCollider2D interactionArea;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float interactionRadius;
    private SpriteRenderer child;
    private bool canInteract;
    private bool playerInteracted;

    private void Awake()
    {
        interactionArea.radius = interactionRadius;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        child = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        canInteract = true;
        child.enabled = true;
        spriteRenderer.color = new Color(0.5f, 0.75f, 1f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canInteract = false;
        child.enabled = false;
        spriteRenderer.color = Color.white;
    }

    public void PlayerInteracted()
    {
        playerInteracted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if ((canInteract == true) && (playerInteracted))
        {
            Debug.Log("Player Interacted");
            playerInteracted = false;
        }
    }
}
