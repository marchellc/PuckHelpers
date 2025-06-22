using PuckHelpers.Extensions;
using PuckHelpers.Functions.FaceOffPositions;

namespace PuckHelpers.Functions.Commands;

/// <summary>
/// Removes the custom position of a puck for face-off.
/// </summary>
public class RemovePuckPositionCommand
{
    private static bool HandleCommand(Player sender, ArraySegment<string> args, out string response)
    {
        if (!sender.IsPermitted())
        {
            response = "You are not allowed to use this command.";
            return false;
        }

        if (args.Count < 1)
        {
            response = "Missing arguments! Usage: /removepuckpos <Custom Position Name>";
            return false;
        }

        var name = args.At(0);

        if (!FaceOffPositionManager.Positions.TryGetValue(name, out var position))
        {
            response = $"Unknown position: {name}";
            return false;
        }

        position.HasPuckPosition = false;

        position.PuckPosition.X = 0f;
        position.PuckPosition.Y = 0f;
        position.PuckPosition.Z = 0f;
        
        FaceOffPositionManager.SavePositions();

        response = $"Removed puck position of '{name}'";
        return true;
    }

    internal static void RegisterCommand()
    {
        CustomCommands.RegisterCommand("removepuckpos", HandleCommand);
    }
}