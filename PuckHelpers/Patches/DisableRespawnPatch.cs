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
    public static bool IsDisabled { get; set; }

    [HarmonyPrefix]
    public static bool Prefix()
    {
        if (IsDisabled)
            return false;

        return true;
    }
}