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
using TMPro;

public class CraftingTable : MonoBehaviour
{
    [SerializeField] Sprite craftableItem;
    [SerializeField] int requiredWood;
    [SerializeField] int requiredSteel;
    [SerializeField] int requiredElectronics;
    public TextMeshProUGUI requiredText1;
    public TextMeshProUGUI requiredText2;

    bool canCraft = false;
    public PlayerMovement player;

    int playerWood;
    int playerSteel;
    int playerElectronics;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        // Deciding what integers to put in the text fields - Can be condensed later
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

    private void CheckResources()
    {
        playerWood = player.woodAmount;
        playerSteel = player.steelAmount;
        playerElectronics = player.electronicsAmount;
        if (playerWood >= requiredWood && playerSteel >= requiredSteel && playerElectronics >= requiredElectronics)
        {
            canCraft = true;
        }
    }

    public void Craft()
    {
        if (canCraft)
        {
            // Removing used materials
            player.woodAmount -= requiredWood;
            player.steelAmount -= requiredSteel;
            player.electronicsAmount -= requiredElectronics;

            // Giving Player new item
            canCraft = false;
            CheckResources();
        }
    }
}
