using PuckHelpers.Extensions;
using PuckHelpers.Functions.FaceOffPositions;

namespace PuckHelpers.Functions.Commands;

/// <summary>
/// Lists all loaded face-off positions.
/// </summary>
public class ListPositionsCommand
{
    private static bool HandleCommand(Player sender, ArraySegment<string> args, out string response)
    {
        if (!sender.IsPermitted())
        {
            response = "You do not have permission to use this command.";
            return true;
        }

        if (FaceOffPositionManager.Positions.Count == 0)
        {
            response = "No face-off positions are saved.";
            return true;
        }

        response = $"Positions ({FaceOffPositionManager.Positions.Count}):";

        foreach (var position in FaceOffPositionManager.Positions)
            response += $"\n{position.Key}";

        return true;
    }
    
    internal static void RegisterCommand()
    {
        CustomCommands.RegisterCommand("listpositions", HandleCommand);
    }
}