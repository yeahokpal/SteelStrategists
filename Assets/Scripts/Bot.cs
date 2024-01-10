/* Programmer - Jack Gill
 * Purpose - Manage imdevidual bots and contain information revelent to them */

public class Bot
{
    public BotStatus currentStatus = BotStatus.Idle;
    public Material currentMaterial = Material.None;

    public Bot()
    {
        currentStatus = BotStatus.Idle;
        currentMaterial = Material.None;
    }
}
