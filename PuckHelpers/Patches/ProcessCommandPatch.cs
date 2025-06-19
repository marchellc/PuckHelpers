using System;

using HarmonyLib;

using PuckHelpers.Core;
using PuckHelpers.Functions;
using PuckHelpers.Extensions; 

namespace PuckHelpers.Patches;

/// <summary>
/// Implements custom commands.
/// </summary>
[HarmonyPatch(typeof(UIChat), nameof(UIChat.Server_ProcessPlayerChatMessage))]
public static class ProcessCommandPatch
{
    [HarmonyPrefix]
    public static bool Prefix(Player player, string message, ulong clientId, bool useTeamChat, bool isMuted, ref bool __runOriginal)
    {
        if (message.Length > 0 && message[0] == '/')
        {
            var args = message.Split([' '],  StringSplitOptions.RemoveEmptyEntries);

            if (args.Length > 0)
            {
                var command = args[0];
                var segment = new ArraySegment<string>(args, 1, args.Length - 1);
                
                HelpersPlugin.LogInfo("PuckHelpers / CustomCommands",
                    $"Received command '{command}' from '{player?.Username?.Value ?? "(null)"} ({clientId})' ({string.Join(" ", segment)})");

                if (CustomCommands.TryInvokeCommand(command, player, segment, out var response))
                {
                    HelpersPlugin.LogInfo("PuckHelpers / CustomCommands", $"Handled custom command, response: {response}");

                    player.SendChat(response);

                    __runOriginal = false;
                    return false;
                }
            }
        }

        __runOriginal = true;
        return true;
    }
}