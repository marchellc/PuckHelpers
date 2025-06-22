using PuckHelpers.Extensions;
using PuckHelpers.Functions.FaceOffPositions;

namespace PuckHelpers.Functions.Commands;

/// <summary>
/// Command to set face-off puck position.
/// </summary>
public class SetPuckPositionCommand
{
    private static bool HandleCommand(Player sender, ArraySegment<string> args, out string response)
    {
        if (!sender.IsPermitted())
        {
            response = "You do not have the permission to use this command.";
            return false;
        }

        if (args.Count < 1)
        {
            response = $"Missing arguments! /setpuckpos <Custom Position Name>";
            return false;
        }

        var name = args.At(0);
        
        if (!FaceOffPositionManager.Positions.TryGetValue(name, out var position))
            FaceOffPositionManager.Positions[name] = position = new();

        var vectorPos = sender.Stick.BladeHandlePosition;
        
        position.HasPuckPosition = true;

        position.PuckPosition.X = vectorPos.x;
        position.PuckPosition.Y = vectorPos.y;
        position.PuckPosition.Z = vectorPos.z;
        
        FaceOffPositionManager.SavePositions();

        response =
            $"Set puck position of '{name}' to '{GetPositionCommand.VectorToString(string.Empty, position.PuckPosition.Vector)}'";
        return true;
    }

    internal static void RegisterCommand()
    {
        CustomCommands.RegisterCommand("setpuckpos", HandleCommand);
    }
}