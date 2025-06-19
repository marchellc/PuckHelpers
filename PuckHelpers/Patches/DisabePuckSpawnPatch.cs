using HarmonyLib;

namespace PuckHelpers.Patches;

/// <summary>
/// Disables spawning of pucks.
/// </summary>
[HarmonyPatch(typeof(PuckManager), nameof(PuckManager.Server_SpawnPuck))]
public static class DisablePuckSpawnPatch
{
    /// <summary>
    /// Whether or not the spawn is disabled.
    /// </summary>
    public static bool IsDisabled { get; set; }

    [HarmonyPrefix]
    public static bool Prefix() => !IsDisabled;
}