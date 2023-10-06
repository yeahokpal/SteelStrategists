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
    [SerializeField] private CraftingTable ct;
    [SerializeField] private GameObject InteractScript; //should be whatever you want to do as a result of interacting
    [SerializeField] GameObject child;
    private bool canInteract;

    private void Awake()
    {
        interactionArea.radius = interactionRadius;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        canInteract = true;
        if (child != null) { child.SetActive(true); }
        spriteRenderer.color = new Color(0.5f, 0.75f, 1f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canInteract = false;
        if (child != null) { child.SetActive(false); }
        spriteRenderer.color = Color.white;
    }

    public void PlayerInteracted()
    {
        Debug.Log("Interact True");
        if (ct != null)
        {
            ct.Craft();
        }
        StartCoroutine(DisableInteract());
    }

    IEnumerator DisableInteract()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Interact False");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
