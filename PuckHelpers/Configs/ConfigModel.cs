using Newtonsoft.Json;

namespace PuckHelpers.Configs;

/// <summary>
/// Represents the model of the plugin's config.
/// </summary>
public class ConfigModel
{
    /// <summary>
    /// Whether or not to disable the vote kick command.
    /// </summary>
    [JsonProperty("disable_vote_kick")]
    public bool DisableVoteKick { get; set; }
}