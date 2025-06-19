using HarmonyLib;

using PuckHelpers.Core;

namespace PuckHelpers.Patches;

/// <summary>
/// Disables character respawn.
/// </summary>
[HarmonyPatch(typeof(Player), nameof(Player.Server_RespawnCharacter))]
public static class DisableRespawnPatch
{
    /// <summary>
    /// Whether or not respawning is disabled.
    /// </summary>
    public static bool IsDisabled
    {
        get => field;
        set
        {
            field = value;
            
            HelpersPlugin.LogInfo("PuckHelpers / API", $"DisableRespawn changed to {value}");
        }
    }

    [HarmonyPrefix]
    public static bool Prefix()
    {
        if (IsDisabled)
        {
            HelpersPlugin.LogInfo("PuckHelpers / API", "Prevented respawn");
            return false;
        }
        
        HelpersPlugin.LogInfo("PuckHelpers / API", "Allowed respawn");
        return true;
    }
}