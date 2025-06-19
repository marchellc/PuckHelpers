using HarmonyLib;

using PuckHelpers.Configs;

using PuckHelpers.Functions;
using PuckHelpers.Functions.FaceOffPositions;

using UnityEngine;

namespace PuckHelpers.Core;

/// <summary>
/// The main plugin class.
/// </summary> 
public class HelpersPlugin : IPuckMod 
{
    /// <summary>
    /// Gets the active plugin instance.
    /// </summary>
    public static HelpersPlugin Instance { get; private set; }
    
    /// <summary>
    /// Gets the active Harmony instance.
    /// </summary>
    public static Harmony Harmony { get; private set; }
    
    /// <summary>
    /// Gets the active config instance.
    /// </summary>
    public static ConfigModel Config { get; private set; }

    /// <summary>
    /// Enables the plugin.
    /// </summary>
    /// <returns>true if the plugin was enabled</returns>
    public bool OnEnable()
    {
        Instance = this;
        
        LogInfo("Puck Helpers", "Loading ..");

        Config = ConfigLoader.LoadConfig("mainConfig", new ConfigModel());
        
        CustomCommands.RegisterCommands();
        FaceOffPositionManager.LoadPositions();
        
        Harmony = new("com.peanut.puckhelpers." + DateTime.Now.Ticks);
        Harmony.PatchAll();
        
        LogInfo("Puck Helpers", "Loaded!");
        return true;
    }

    /// <summary>
    /// Disables the plugin.
    /// </summary>
    /// <returns>true if the plugin was disabled</returns>
    public bool OnDisable()
    {
        Harmony?.UnpatchSelf();
        Harmony = null;
        
        return true;
    }

    internal static void LogInfo(string source, object message)
        => Debug.Log($"[{source}] {message}");
    
    internal static void LogWarn(string source, object message)
        => Debug.LogWarning($"[{source}] {message}");
    
    internal static void LogError(string source, object message)
        => Debug.LogError($"[{source}] {message}");
}
