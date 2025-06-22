using PuckHelpers.Extensions;
using PuckHelpers.Functions.FaceOffPositions;

namespace PuckHelpers.Functions.Commands;

/// <summary>
/// Command for starting custom face-off.
/// </summary>
public class StartFaceOffCommand
{
    private static bool HandleCommand(Player sender, ArraySegment<string> args, out string response)
    {
        if (!sender.IsPermitted())
        {
            response = "You do not have the required permissions to invoke this command.";
            return false;
        }

        if (args.Count < 1)
        {
            response = "Missing arguments! Usage: /startfaceoff <Position Name>";
            return false;
        }

        if (!FaceOffPositionManager.Positions.TryGetValue(args.At(0), out var position))
        {
            response = $"Unknown custom position name: {args.At(0)}";
            return false;
        }
        
        FaceOffHelper.StartCustom(position);

        response = $"Started face-off with a custom position: {position.Name}";
        return true;
    }
    
    internal static void RegisterCommand()
    {
        CustomCommands.RegisterCommand("startfaceoff", HandleCommand);
        CustomCommands.RegisterCommand("startfo", HandleCommand);
    }
}