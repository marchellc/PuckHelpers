using PuckHelpers.Core;

namespace PuckHelpers.Extensions;

/// <summary>
/// Extensions targeting the player.
/// </summary>
public static class PlayerExtensions
{
    /// <summary>
    /// Whether or not a player is an administrator.
    /// </summary>
    /// <param name="player">The target player.</param>
    /// <returns>true if the player is an administrator</returns>
    public static bool IsPermitted(this Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));

        var manager = NetworkBehaviourSingleton<ServerManager>.Instance;

        foreach (var steamId in manager.AdminSteamIds)
        {
            if (steamId == player.SteamId.Value)
            {
                HelpersPlugin.LogInfo("PuckHelpers / API", $"Found SteamID '{player.SteamId.Value}' in server admin Steam IDs.");
                return true;
            }
        }

        HelpersPlugin.LogInfo("PuckHelpers / API", $"Player '{player?.Username?.Value ?? "(null)"}' - '{player.SteamId.Value}' is not an administrator.");
        return false;
    }
    
    /// <summary>
    /// Sends a chat message to a specific player.
    /// </summary>
    /// <param name="player">The target player.</param>
    /// <param name="message">The message.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void SendChat(this Player player, string message)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));

        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException(nameof(message));
        
        NetworkBehaviourSingleton<UIChat>.Instance.Server_SendSystemChatMessage(message, player.OwnerClientId);
    }
}