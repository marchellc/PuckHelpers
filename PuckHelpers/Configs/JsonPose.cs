using Newtonsoft.Json;

namespace PuckHelpers.Configs;

/// <summary>
/// Represents a JSON-serializable Pose.
/// </summary>
public class JsonPose
{
    /// <summary>
    /// Gets or sets the JSON vector position.
    /// </summary>
    [JsonProperty("position")]
    public JsonVector3 Position { get; set; } = new();

    /// <summary>
    /// Gets or sets the JSON quaternion rotation.
    /// </summary>
    [JsonProperty("rotation")]
    public JsonQuaternion Rotation { get; set; } = new();
}