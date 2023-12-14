/* Programmer - Jack Gill
 * Purpose - Manage imdevidual bots and contain information revelent to them */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BotStatus { Idle, Gathering }
public enum Material { Wood, Steel, Electronics, None}
public class Bot : MonoBehaviour
{
    public BotStatus currentStatus = BotStatus.Idle;
    public Material currentMaterial = Material.None;
}
