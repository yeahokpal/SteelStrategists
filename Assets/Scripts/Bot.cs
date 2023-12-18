/* Programmer - Jack Gill
 * Purpose - Manage imdevidual bots and contain information revelent to them */
public enum BotStatus { Idle, Gathering, WaitingToGather }
public enum Material { Wood, Steel, Electronics, None}
public class Bot
{
    public BotStatus currentStatus = BotStatus.Idle;
    public Material currentMaterial = Material.None;
}
