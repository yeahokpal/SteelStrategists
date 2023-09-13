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
        child = this.GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        canInteract = true;
        child.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canInteract = false;
        child.enabled = false;
    }

    public void PlayerInteracted()
    {
        playerInteracted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract) spriteRenderer.color = new Color(0.5f, 0.75f, 1f);
        else spriteRenderer.color = Color.white;
        interactionArea.radius = interactionRadius;
        if ((canInteract == true) && (playerInteracted))
        {
            Debug.Log("Player Interacted");
            playerInteracted = false;
        }
    }
}
