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
using System;

public class Interactables : MonoBehaviour
{
    [SerializeField] private CircleCollider2D interactionArea;
    [SerializeField] private GameObject MapCanvas;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float interactionRadius;
    [SerializeField] private CraftingTable ct;
    [SerializeField] private GameObject InteractScript; //should be whatever you want to do as a result of interacting
    [SerializeField] byte botNum;
    [SerializeField] Sprite[] materialSprites = new Sprite[3];
    [SerializeField] GameObject child;
    PlayerManager player;
    GameManager gameManager;
    private bool canInteract;
    private bool playerInteracted;

    private void Awake()
    {
        interactionArea.radius = interactionRadius;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
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
        if (child != null && botNum == 0) { child.SetActive(false); }
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

            // Things related to bot management
            if (botNum != 0)
            {
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();  
                Bot bot = gameManager.bots[botNum - 1];

                if (bot.currentStatus == BotStatus.WaitingToGather)
                {
                    System.Random random = new System.Random();
                    switch (bot.currentMaterial)
                    {
                        case Material.Wood:
                            player.woodAmount += random.Next(1, 5);
                            break;
                        case Material.Steel:
                            player.woodAmount += random.Next(1, 5);
                            break;
                        case Material.Electronics:
                            player.woodAmount += random.Next(1, 5);
                            break;
                    }
                    bot.currentMaterial = Material.None;
                    bot.currentStatus = BotStatus.Idle;
                    UpdateBot();
                }
            }
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

    public void UpdateBot()
    {
        if (botNum != 0)
        {
            switch (gameManager.bots[botNum + 1].currentStatus)
            {
                case BotStatus.Idle:
                    child.GetComponent<SpriteRenderer>().sprite = null;
                    spriteRenderer.enabled = true;
                    break;
                case BotStatus.Gathering:
                    child.GetComponent<SpriteRenderer>().sprite = null;
                    spriteRenderer.enabled = true;
                    break;
                case BotStatus.WaitingToGather:
                    switch (gameManager.bots[botNum + 1].currentMaterial)
                    {
                        case Material.Wood:
                            child.GetComponent<SpriteRenderer>().sprite = materialSprites[0];
                            spriteRenderer.enabled = true;
                            break;
                        case Material.Steel:
                            child.GetComponent<SpriteRenderer>().sprite = materialSprites[1];
                            spriteRenderer.enabled = true;
                            break;
                        case Material.Electronics:
                            child.GetComponent<SpriteRenderer>().sprite = materialSprites[2];
                            spriteRenderer.enabled = true;
                            break;
                    }
                    break;
            }
        }
    }

}
