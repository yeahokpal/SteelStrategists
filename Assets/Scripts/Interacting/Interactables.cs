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
using UnityEngine;
using UnityEngine.UI;

public class Interactables : MonoBehaviour
{
    #region Variables
    [SerializeField] private CircleCollider2D interactionArea;
    [SerializeField] private GameObject MapCanvas;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float interactionRadius;
    [SerializeField] private CraftingTable ct;
    [SerializeField] private GameObject InteractScript; //should be whatever you want to do as a result of interacting
    [SerializeField] byte botNum;
    public Sprite[] materialSprites = new Sprite[3];
    public GameObject child;
    PlayerManager player;
    GameManager gameManager;
    private bool canInteract;
    private bool playerInteracted;
    public Material heldMaterial = Material.None;
    #endregion

    #region Default Methods
    // Defining variables
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        interactionArea.radius = interactionRadius;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    // The player can interact when they enter a certain radius of the interactable object
    private void OnTriggerEnter2D(Collider2D other)
    {
        canInteract = true;
        if (child != null) { child.SetActive(true); }
        spriteRenderer.color = new Color(0.5f, 0.75f, 1f);
    }

    // The player cant interact when they exit a certain radius of the interactable object
    private void OnTriggerExit2D(Collider2D other)
    {
        canInteract = false;
        if (child != null && botNum == 0) { child.SetActive(false); }
        spriteRenderer.color = Color.white;
    }
    #endregion

    #region Custom Methods
    public void PlayerInteracted()
    {
        playerInteracted = true;
        if (canInteract && playerInteracted)
        {
            Debug.Log("Interacted");
            GameObject childCanvas = FindChildWithTag(gameObject, "Dialog Object");

            if (childCanvas != null)
            {
                childCanvas.GetComponent<CanvasManager>().SelectDialog();
            }

            // If the object is a crafting table, then craft
            if (ct != null)
            {
                ct.Craft();
            }
            // If the object is the map, then open the map

            if (this.name == "Map")
            {
                this.GetComponent<CanvasManager>().OpenMapMenu();
                Debug.Log("Player Opened Map");
            }
            StartCoroutine(DisableInteract());

            // Things related to bot management
            if (botNum != 0)
            {
                Bot bot = gameManager.bots[botNum - 1];

                if (bot.currentStatus == BotStatus.WaitingToGather)
                {
                    Debug.Log("Colleting Materials from: " + this.name);
                    System.Random random = new System.Random();
                    switch (bot.currentMaterial)
                    {
                        case Material.Wood:
                            player.woodAmount += random.Next(1, 5);
                            break;
                        case Material.Steel:
                            player.steelAmount += random.Next(1, 5);
                            break;
                        case Material.Electronics:
                            player.electronicsAmount += random.Next(1, 5);
                            break;
                    }
                    bot.currentMaterial = Material.None;
                    bot.currentStatus = BotStatus.Idle;
                    UpdateBot();

                    child.GetComponent<SpriteRenderer>().sprite = null;

                    GameObject.Find("Map").GetComponent<MapManager>().startButton.GetComponent<CanvasInteractions>().RobotSprites[botNum - 1].GetComponent<Image>().color = Color.white;
                    GameObject.Find("Map").GetComponent<MapManager>().startButton.GetComponent<CanvasInteractions>().RobotSprites[botNum - 1].GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    IEnumerator DisableInteract()
    {
        yield return new WaitForSeconds(0.2f);
        playerInteracted = false;
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

    // Updating the bot sprites based on current status and material
    public void UpdateBot()
    {
        if (botNum != 0)
        {
            switch (gameManager.bots[botNum - 1].currentStatus)
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
                    switch (gameManager.bots[botNum - 1].currentMaterial)
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
    #endregion
}
