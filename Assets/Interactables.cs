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
    [SerializeField] private float interactionRadius;
    [SerializeField] private CircleCollider2D interactionArea;
    private bool canInteract;
    private bool playerInteracted;

    private void OnTriggerEnter2D(Collider2D other)
    {
        canInteract = true;
        Debug.Log("Player entered interaction distance");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canInteract = false;
        Debug.Log("Player exited interaction distance");
    }

    public void OnInteract()
    {
        playerInteracted = true;
        Debug.Log("Key Pressed");
    }

    // Update is called once per frame
    void Update()
    {
        interactionArea.radius = interactionRadius;
        if ((canInteract == true) && (playerInteracted))
        {
            Debug.Log("interact");
            playerInteracted = false;
        }
    }
}
