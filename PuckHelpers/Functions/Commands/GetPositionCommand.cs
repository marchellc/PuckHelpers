using PuckHelpers.Extensions;
using UnityEngine;

namespace PuckHelpers.Functions.Commands;

internal static class GetPositionCommand
{
    private static bool HandleCommand(Player sender, ArraySegment<string> args, out string response)
    {
        if (!sender.IsPermitted())
        {
            response = "You do not have permission to use this command.";
            return false;
        }

        var str = string.Empty;

        if (sender.Stick != null)
            str += $"\n{VectorToString("Blade Position: ", sender.Stick.BladeHandlePosition)}";

        if (sender.PlayerBody != null && sender.PlayerBody.Movement != null)
        {
            if (sender.PlayerBody.Movement != null)
                str += $"\n{VectorToString("Movement Position: ", sender.PlayerBody.Movement.MovementDirection.position)}";

            if (sender.PlayerBody.Rigidbody != null)
                str += $"\n{VectorToString("Rigidbody Position: ", sender.PlayerBody.Rigidbody.position)}";
        }

        str += $"\n{VectorToString("Transform Position: ", sender.transform.position)}";
            
        response = str;
        return true;
    }

    internal static void RegisterCommand()
    {
        CustomCommands.RegisterCommand("getpos", HandleCommand);
    }

    /// <summary>
    /// Converts a vector to a string.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="vector">The vector.</param>
    /// <returns>The converted vector.</returns>
    public static string VectorToString(string header, Vector3 vector)
    {
        return string.Concat(header,
            "X = ", vector.x,
            "; Y = ", vector.y,
            "; Z = ", vector.z);
    }
}