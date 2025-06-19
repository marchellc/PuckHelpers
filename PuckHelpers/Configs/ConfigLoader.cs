using Newtonsoft.Json;

namespace PuckHelpers.Configs;

/// <summary>
/// Manages the config file.
/// </summary>
public static class ConfigLoader
{
    /// <summary>
    /// Loads a config file.
    /// </summary>
    /// <param name="configName">The name of the config file to load.</param>
    /// <param name="defaultConfig">The default config value.</param>
    /// <typeparam name="T">The type of the config file.</typeparam>
    /// <returns>The loaded config instance.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T LoadConfig<T>(string configName, T defaultConfig)
    {
        if (string.IsNullOrEmpty(configName))
            throw new ArgumentNullException(nameof(configName));

        var path = Path.Combine(MonoBehaviourSingleton<ModManagerV2>.Instance.pluginsPath,
            $"PuckHelpers_{configName}.json");

        if (!File.Exists(path))
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(defaultConfig, Formatting.Indented));
            return defaultConfig;
        }

        return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
    }

    /// <summary>
    /// Saves a config file.
    /// </summary>
    /// <param name="configName">The name of the config file.</param>
    /// <param name="config">The value of the config.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void SaveConfig(string configName, object config)
    {
        if (string.IsNullOrEmpty(configName))
            throw new ArgumentNullException(nameof(configName));
        
        if (config is null)
            throw new ArgumentNullException(nameof(config));

        File.WriteAllText(
            Path.Combine(MonoBehaviourSingleton<ModManagerV2>.Instance.pluginsPath, $"PuckHelpers_{configName}.json"),
            JsonConvert.SerializeObject(config, Formatting.Indented));
    }
}