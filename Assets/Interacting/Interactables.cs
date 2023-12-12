/*
 * Programmer: Caden Mesina
 * Purpose: Make objects this script is attached to execute an action when the player is within a certain distance and presses interact
 * Input:
 *  - When a player enters and exits the interaction area
 *  - The radius of the interaction area
 *  - The player interaction key
 * Output: Whatever function is needed by the object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactables : MonoBehaviour
{
    [SerializeField] private CircleCollider2D interactionArea;
    [SerializeField] private GameObject MapCanvas;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float interactionRadius;
    [SerializeField] private CraftingTable ct;
    [SerializeField] private GameObject InteractScript; //should be whatever you want to do as a result of interacting
    [SerializeField] GameObject child;
    private bool canInteract;
    private bool playerInteracted;

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
        playerInteracted = true;
        if (canInteract && playerInteracted)
        {
        GameObject child = FindChildWithTag(gameObject, "Dialog Object");
        if (child != null)
        {
            child.GetComponent<CanvasManager>().SelectDialog();
        }
        if (ct != null)
        {
            ct.Craft();
        }
        if (this.name == "Map")
            {
                this.GetComponent<CanvasManager>().OpenMapMenu();
                Debug.Log("Player Opened Map");
            }
        StartCoroutine(DisableInteract());
        }
    }

    IEnumerator DisableInteract()
    {
        yield return new WaitForSeconds(0.2f);
        playerInteracted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject child = null;

        foreach (Transform transform in parent.transform)
        {
            if (transform.CompareTag(tag))
            {
                child = transform.gameObject;
                break;
            }
        }
        return child;
    }
}
