using HarmonyLib;

using PuckHelpers.Core; 

namespace PuckHelpers.Patches;

/// <summary>
/// Disables the vote kick command.
/// </summary>
[HarmonyPatch(typeof(VoteManagerController), nameof(VoteManagerController.Event_Server_OnChatCommand))]
public static class DisableVoteKickPatch
{
    /// <summary>
    /// Prefix method targeting <see cref="VoteManagerController.Event_Server_OnChatCommand"/>.
    /// </summary>
    /// <param name="message">The received chat message.</param>
    /// <returns>true if the original method should be invoked</returns>
    [HarmonyPrefix]
    public static bool Prefix(Dictionary<string, object> message)
    {
        if (HelpersPlugin.Config is null || !HelpersPlugin.Config.DisableVoteKick)
            return true;
        
        var command = (string)message["command"];
        var clientId = (ulong)message["clientId"];
        var client =  NetworkBehaviourSingleton<PlayerManager>.Instance.GetPlayerByClientId(clientId);
        
        if (string.Equals("/vk", command, StringComparison.InvariantCultureIgnoreCase)
            || string.Equals("/votekick", command, StringComparison.InvariantCultureIgnoreCase))
        {
            HelpersPlugin.LogInfo("PuckHelpers / DisableVoteKick", $"Prevented vote-kick command ({command}; sent by {clientId} - {client?.Username?.Value ?? "null"})");
            return false;
        }
        
        return true;
    }
}