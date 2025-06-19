using HarmonyLib;

using PuckHelpers.Core;

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
    public static bool IsDisabled
    {
        get => field;
        set
        {
            field = value;
            HelpersPlugin.LogInfo("PuckHelpers / API", $"DisablePuckSpawn changed to {value}");
        }
    }

    [HarmonyPrefix]
    public static bool Prefix()
    {
        if (IsDisabled)
        {
            HelpersPlugin.LogInfo("PuckHelpers / API", "Prevented puck spawn");
            return false;
        }

        HelpersPlugin.LogInfo("PuckHelpers / API", "Allowed puck spawn");
        return true;
    }
}