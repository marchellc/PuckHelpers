using Newtonsoft.Json;

using UnityEngine;

namespace PuckHelpers.Configs;

/// <summary>
/// Represents JSON-serializable Vector3.
/// </summary>
public class JsonVector3
{
    private Vector3? cachedVector;
    
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
    /// Gets the converted vector.
    /// </summary>
    [JsonIgnore]
    public Vector3 Vector
    {
        get
        {
            if (!cachedVector.HasValue || cachedVector.Value.x != X
                                       || cachedVector.Value.y != Y
                                       || cachedVector.Value.z != Z)
                cachedVector = new(X, Y, Z);

            return cachedVector.Value;
        }
    }
}