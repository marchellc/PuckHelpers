using PuckHelpers.Extensions;
using PuckHelpers.Functions.FaceOffPositions;

namespace PuckHelpers.Functions.Commands;

/// <summary>
/// Command that removes custom face-off positions.
/// </summary>
public class RemovePositionCommand
{
    private static bool HandleCommand(Player sender, ArraySegment<string> args, out string response)
    {
        if (!sender.IsPermitted())
        {
            response = "You do not have permission to use this command.";
            return true;
        }

        var name = args.At(0);

        if (!FaceOffPositionManager.Positions.Remove(name))
        {
            response = $"Position '{name}' does not exist.";
            return true;
        }
        
        FaceOffPositionManager.SavePositions();

        response = $"Removed position '{name}'";
        return true;
    }
    
    internal static void RegisterCommand()
    {
        CustomCommands.RegisterCommand("removepos", HandleCommand);
    }
}