/*  
 *  Programmer: Jack Gill
 *  Purpose: Manage crafting tables to trade resources for items that can be used
 *  Inputs:
 *  - Player interacts with the table and has enough resources
 *  Outputs: 
 *  - Gives player crafted item and removes used resources
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingTable : MonoBehaviour
{
    // Lots of UI elements and materials needed for crafting
    [SerializeField] Sprite craftableItem;
    [SerializeField] Sprite materialSprite1;
    [SerializeField] Sprite materialSprite2;
    [SerializeField] TextMeshProUGUI requiredText1;
    [SerializeField] TextMeshProUGUI requiredText2;
    [SerializeField] GameObject material1;
    [SerializeField] GameObject material2;
    [SerializeField] int requiredWood;
    [SerializeField] int requiredSteel;
    [SerializeField] int requiredElectronics;

    // Tplayer's current resources
    int playerWood;
    int playerSteel;
    int playerElectronics;

    bool canCraft = false;
    public PlayerMovement player;

    private void Awake()
    {
        material1.GetComponent<Image>().sprite = materialSprite1;
        material2.GetComponent<Image>().sprite = materialSprite2;

        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        // These If-Else statements decide what integers to put in the text fields - Can be condensed later
        if (requiredWood == 0)
        {
            if (requiredSteel == 0)
            {
                requiredText1.text = requiredElectronics.ToString();
            }
            else
            {
                requiredText1.text = requiredSteel.ToString();
                if (requiredElectronics > 0)
                {
                    requiredText2.text = requiredElectronics.ToString();
                }
            }
        }
        else if (requiredSteel == 0)
        {
            requiredText1.text = requiredWood.ToString();
            if (requiredElectronics > 0)
            {
                requiredText2.text = requiredElectronics.ToString();
            }
        }
        else 
        {
            requiredText1.text = requiredWood.ToString();
            if (requiredSteel > 0)
            {
                requiredText2.text = requiredSteel.ToString();
            }
            if (requiredElectronics > 0)
            {
                requiredText2.text = requiredElectronics.ToString();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckResources();
    }

    // Checking to see if the player has enough resources to craft the item
    private void CheckResources()
    {
        playerWood = player.woodAmount;
        playerSteel = player.steelAmount;
        playerElectronics = player.electronicsAmount;
        if (playerWood >= requiredWood && playerSteel >= requiredSteel && playerElectronics >= requiredElectronics)
        {
            Debug.Log("Can Craft");
            canCraft = true;
        }
    }

    // Gaining the item and removing the required materials from the player's inventory
    public void Craft()
    {
        if (canCraft)
        {
            // Removing used materials
            player.woodAmount -= requiredWood;
            player.steelAmount -= requiredSteel;
            player.electronicsAmount -= requiredElectronics;
            Debug.Log("Crafted!");

            // Giving Player new item
            canCraft = false;
            CheckResources();
        }
    }
}
