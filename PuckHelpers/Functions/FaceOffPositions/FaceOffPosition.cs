using Newtonsoft.Json;

using PuckHelpers.Configs;

namespace PuckHelpers.Functions.FaceOffPositions;

public class FaceOffPosition
{
    /// <summary>
    /// Gets or sets the name of the position.
    /// </summary>
    [JsonProperty("name")] 
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the position of the puck.
    /// </summary>
    [JsonProperty("custom_puck_position")]
    public JsonVector3 PuckPosition { get; set; } = new();
    
    /// <summary>
    /// Whether or not this position has a custom puck position.
    /// </summary>
    [JsonProperty("has_custom_puck")]
    public bool HasPuckPosition { get; set; }

    /// <summary>
    /// Gets or sets the position list for the Blue team.
    /// </summary>
    [JsonProperty("blue_positions")]
    public Dictionary<FaceOffPositionName, JsonPose> BluePositions { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the position list for the Red team.
    /// </summary>
    [JsonProperty("red_positions")]
    public Dictionary<FaceOffPositionName, JsonPose> RedPositions { get; set; } = new();
}