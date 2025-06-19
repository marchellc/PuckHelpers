using Newtonsoft.Json;

using UnityEngine;

namespace PuckHelpers.Configs;

/// <summary>
/// Represents JSON-serializable Quaternion.
/// </summary>
public class JsonQuaternion
{
    private Quaternion? cachedQuaternion;
    
    /// <summary>
    /// Gets or sets the X coordinate.
    /// </summary>
    [JsonProperty("x")]
    public float X { get; set; } = 0f;
    
    /// <summary>
    /// Gets or sets the Y coordinate.
    /// </summary>
    [JsonProperty("y")]
    public float Y { get; set; } = 0f;
    
    /// <summary>
    /// Gets or sets the Z coordinate.
    /// </summary>
    [JsonProperty("z")]
    public float Z { get; set; } = 0f;

    /// <summary>
    /// Gets or sets the W coordinate.
    /// </summary>
    [JsonProperty("w")]
    public float W { get; set; } = 0f;

    /// <summary>
    /// Gets the converted Quaternion.
    /// </summary>
    [JsonIgnore]
    public Quaternion Quaternion
    {
        get
        {
            if (!cachedQuaternion.HasValue || cachedQuaternion.Value.x != X
                                       || cachedQuaternion.Value.y != Y
                                       || cachedQuaternion.Value.z != Z
                                       || cachedQuaternion.Value.w != W)
                cachedQuaternion = new(X, Y, Z, W);

            return cachedQuaternion.Value;
        }
    }
}